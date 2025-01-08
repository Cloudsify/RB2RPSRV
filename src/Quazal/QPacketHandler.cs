using System;
using System.Net.Sockets;
using System.Net;

namespace Quazal
{
    public static class QPacketHandler
    {

          public static QPacket ProcessSYN(QPacket p, IPEndPoint ep, out Client client)
          {
            client = Server.GetClientByEndPoint(ep);
            if (client == null)
            {
              client = new Client(ep, Server.idCounter++, Server.pidCounter++);
              Server.clients.Add(client);
            }
            QPacket reply = new QPacket();
            reply.m_oSourceVPort = p.m_oDestinationVPort;
            reply.m_oDestinationVPort = p.m_oSourceVPort;
            reply.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK };
            reply.type = QPacket.PACKETTYPE.SYN;
            reply.m_bySessionID = p.m_bySessionID;
            reply.m_uiSignature = p.m_uiSignature;
            reply.uiSeqId = p.uiSeqId;
            reply.m_uiConnectionSignature = p.m_uiConnectionSignature;
            reply.payload = new byte[0];
            return reply;
          }

          public static QPacket ProcessCONNECT(Client client, QPacket p)
          {
            client.IDsend = p.m_uiConnectionSignature;
            QPacket reply = new QPacket();
            reply.m_oSourceVPort = p.m_oDestinationVPort;
            reply.m_oDestinationVPort = p.m_oSourceVPort;
            reply.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK };
            reply.type = QPacket.PACKETTYPE.CONNECT;
            reply.m_bySessionID = p.m_bySessionID;
            reply.m_uiSignature = p.m_uiSignature;
            reply.uiSeqId = p.uiSeqId;
            reply.m_uiConnectionSignature = client.IDsend;
            //if (p.payload != null && p.payload.Length > 0)
            reply.payload = MakeConnectPayload(client,p);
            //else
              //reply.payload = new byte[0];
            return reply; 
          }

          public static byte[] MakeConnectPayload(Client client, QPacket p)
          {
            MemoryStream m = new MemoryStream(p.payload);
            uint size = DataWriter.ReadUint32(m);
            byte[] buff = new byte[size];
            m.Read(buff, 0, (int)size);
            size = DataWriter.ReadUint32(m) - 16;
            buff = new byte[size];
            m.Read(buff, 0, (int)size);
            buff = DataWriter.Decrypt(client.sessionKey, buff);
            m = new MemoryStream(buff);
            DataWriter.ReadUint32(m);
            DataWriter.ReadUint32(m);
            uint responseCode = DataWriter.ReadUint32(m);
            m = new MemoryStream();
            DataWriter.WriteUint32(m, 4);
            DataWriter.WriteUint32(m, responseCode + 1);
            return m.ToArray();
          }

          public static QPacket ProcessDISCONNECT(Client client, QPacket p)
          {
            QPacket reply = new QPacket();
            reply.m_oSourceVPort = p.m_oDestinationVPort;
            reply.m_oDestinationVPort = p.m_oSourceVPort;
            reply.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK };
            reply.type = QPacket.PACKETTYPE.DISCONNECT;
            reply.m_bySessionID = p.m_bySessionID;
            reply.m_uiSignature = client.IDsend - 0x10000;
            reply.uiSeqId = p.uiSeqId;
            reply.payload = new byte[0];
            return reply;
          }

          public static QPacket ProcessPING(Client client, QPacket p)
          {
            QPacket reply = new QPacket();
            reply.m_oSourceVPort = p.m_oDestinationVPort;
            reply.m_oDestinationVPort = p.m_oSourceVPort;
            reply.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK };
            reply.type = QPacket.PACKETTYPE.PING;
            reply.m_bySessionID = p.m_bySessionID;
            reply.m_uiSignature = client.IDsend;
            reply.uiSeqId = p.uiSeqId;
            reply.m_uiConnectionSignature = client.IDrecv;
            reply.payload = new byte[0];
            return reply;
          }

          public static List<ulong> timeToIgnore = new List<ulong>();

          public static void ProcessPacket(string source, byte[] data, IPEndPoint ep, UdpClient listener, uint serverPID, ushort listenPort, bool removeConnectPayload = false)
          {
            while (true)
            {
              QPacket p = new QPacket(data);
              MemoryStream m = new MemoryStream();
              byte[] buff = new byte[(int)p.realSize];

              m.Seek(0, 0);
              m.Read(buff, 0, buff.Length);

              QPacket reply = null;
              Client client = null;

              if(p.type != QPacket.PACKETTYPE.SYN && p.type != QPacket.PACKETTYPE.NATPING)
              {
                client = Server.GetClientByEndPoint(ep);
              }

              switch (p.type)
              {
                case QPacket.PACKETTYPE.SYN:
                    reply = QPacketHandler.ProcessSYN(p, ep, out client);
                    break;
                case QPacket.PACKETTYPE.CONNECT:
                    if (client != null && !p.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
                          {
                              client.sPID = serverPID;
                              client.sPort = listenPort;
                              if (removeConnectPayload)
                              {
                                  p.payload = new byte[0];
                                  p.payloadSize = 0;
                              }
                              reply = QPacketHandler.ProcessCONNECT(client, p);
                              Logger.Debug("Replying to CONNECT packet");
                          }
                    else
                    {
                      Logger.Error("[PACKET-TYPE::CONNECT] Client is null or flag contains FLAG_ACK");
                    }
                    break;
                case QPacket.PACKETTYPE.DISCONNECT:
                    if (client != null)
                        reply = QPacketHandler.ProcessDISCONNECT(client, p);
                        break;
                case QPacket.PACKETTYPE.PING:
                    if (client != null)
                        reply = QPacketHandler.ProcessPING(client, p);
                        break;
                case QPacket.PACKETTYPE.NATPING:
                  ulong time = BitConverter.ToUInt64(p.payload, 5);
                  if (timeToIgnore.Contains(time))
                      timeToIgnore.Remove(time);
                  else
                  {
                    reply = p;
                    m = new MemoryStream();
                    byte b = (byte)(reply.payload[0] == 1 ? 0 : 1);
                    m.WriteByte(b);
                    DataWriter.WriteUint64(m, 0x1234); // RVCID , I think?
                    DataWriter.WriteUint64(m, time);
                    reply.payload = m.ToArray();
                    Send(source, reply, ep, listener);
                    m = new MemoryStream();
                    b = (byte)(b == 1 ? 0 : 1);
                    m.WriteByte(b);
                    DataWriter.WriteUint64(m, 0x1234); // RVCID Again, I think?
                    time = DataWriter.MakeTimestamp();
                    timeToIgnore.Add(time);
                    DataWriter.WriteUint64(m, DataWriter.MakeTimestamp());
                    reply.payload = m.ToArray();
                  }
                  break;
              }

              if (reply != null)
                Send(source, reply, ep, listener);
              
              if (p.realSize != data.Length)
              {
                m = new MemoryStream(data);
                int left = (int)(data.Length - p.realSize);
                byte[] newData = new byte[left];
                m.Seek(p.realSize, 0);
                m.Read(newData, 0, left);
                data = newData;
              }
              else
                  break;
            }
          }

          public static void Send(string source, QPacket p, IPEndPoint ep, UdpClient listener)
          {
            byte[] data = p.toBuffer();
            listener.Send(data, data.Length, ep);
          }
    }
}
