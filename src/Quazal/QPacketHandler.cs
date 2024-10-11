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
              Logger.Debug("Creating new client data...");
                client = new Client
                {
                    ep = ep,
                    IDrecv = 0x00000000,
                    PID = Server.pidCounter++
                };
                Server.clients.Add(client);
            }

            QPacket reply = new QPacket
            {
                m_oSourceVPort = p.m_oDestinationVPort,
                m_oDestinationVPort = p.m_oSourceVPort,
                flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK },
                type = 0x0,
                m_bySessionID = p.m_bySessionID,
                m_uiSignature = p.m_uiSignature,
                uiSeqId = p.uiSeqId,
                m_uiConnectionSignature = client.IDrecv,
                payload = new byte[0]
            };

            Logger.Debug("Sending Data off to client: " + ep.ToString());
            Logger.Debug("Client object: " + (client != null ? "Initialized" : "Null"));
            Logger.Debug("Server.clients count: " + (Server.clients != null ? Server.clients.Count.ToString() : "Server.clients is null"));
            Logger.Debug("Client.sessionKey: " + (client.sessionKey != null ? "Initialized" : "Null"));
            Logger.Debug("Client m_oSourceVPort:" + p.m_oDestinationVPort);
            Logger.Debug("Client m_oDestinationVPort:" + p.m_oSourceVPort);
            Logger.Debug("Client flags:" + new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK });
            Logger.Debug("Client type:" + 0x0);
            Logger.Debug("Client m_bySessionID:" + p.m_bySessionID);
            Logger.Debug("Client m_uiSignature:" + p.m_uiSignature);
            Logger.Debug("Client uiSeqId:" + p.uiSeqId);
            Logger.Debug("Client m_uiConnectionSignature:" + client.IDrecv);

            return reply;
        }

        public static QPacket ProcessCONNECT(Client client, QPacket p)
        {
            client.IDsend = p.m_uiConnectionSignature;
            QPacket reply = new QPacket
            {
                m_oSourceVPort = p.m_oDestinationVPort,
                m_oDestinationVPort = p.m_oSourceVPort,
                flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK },
                type = QPacket.PACKETTYPE.CONNECT,
                m_bySessionID = p.m_bySessionID,
                m_uiSignature = client.IDsend,
                uiSeqId = p.uiSeqId,
                m_uiConnectionSignature = client.IDrecv
            };
            if (p.payload != null && p.payload.Length > 0)
                reply.payload = MakeConnectPayload(client, p);
            else
                reply.payload = new byte[0];
            return reply;
        }
        public static byte[] MakeConnectPayload(Client client, QPacket p)
          {
            MemoryStream m = new MemoryStream(p.payload);
            uint size = DataWriter.ReadUint32(m);
            byte[] buff = new byte[size];
            m.Read(buff, 0, (int)size);
            buff = new byte[size];
            m.Read(buff, 0, (int)size);
            buff = DataWriter.Decrypt(client.sessionKey, buff);
            m = new MemoryStream(buff);
            DataWriter.ReadUint32(m);
            DataWriter.ReadUint32(m);
            uint responseCode = DataWriter.ReadUint32(m);
            Logger.Debug("Got response code 0x" + responseCode.ToString("X8"));
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
                client = Server.GetClientByIDrecv(p.m_uiSignature);
              }

              Logger.Debug("Incoming packet type: " + p.type);

              switch (p.type)
              {
                case QPacket.PACKETTYPE.SYN:
                    Client.reset();
                    reply = ProcessSYN(p, ep, out client);
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
