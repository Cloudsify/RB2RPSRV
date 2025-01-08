using System;
using System.Net.Sockets;
using System.Net;

namespace Quazal
{
    public static class RMC
    {
        public const uint MaxRMCPayloadSize = 963;
        public static void HandlePacket(UdpClient udp, QPacket p)
        {
            Client client = Server.GetClientByIDrecv(p.m_uiSignature);
            if (client == null)
                return;
            client.sessionID = p.m_bySessionID;
            if (p.uiSeqId > client.sequenceIDIn)
                client.sequenceIDIn = p.uiSeqId;
            client.udp = udp;
            if (p.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
                return;
            RMCP rmc = new RMCP();
            if (rmc.isRequest)
                HandleRequest(client, p, rmc);
            else
                HandleResponse(client, p, rmc);
        }

        public static void HandleResponse(Client client, QPacket p, RMCP rmc)
        {
            ProcessResponse(client, p, rmc);
            // Maybe we can do other things here aswell. Logging perhaps?
        }

        public static void ProcessResponse(Client client, QPacket p, RMCP rmc)
        {
            MemoryStream m = new MemoryStream(p.payload);
            m.Seek(rmc._afterProtocolOffset, 0);
            rmc.success = m.ReadByte() == 1;
            
            if (rmc.success)
            {
                rmc.callID = DataWriter.ReadUint32(m);
                rmc.methodID = DataWriter.ReadUint32(m);
            }
            else
            {
                rmc.error = DataWriter.ReadUint32(m);
                rmc.callID = DataWriter.ReadUint32(m);
            }
        }

        public static void HandleRequest(Client client, QPacket p, RMCP rmc)
        {
            ProcessRequest(client, p, rmc);

            if (rmc.callID > client.callCounterRMC)
                client.callCounterRMC = rmc.callID;
            
            switch (rmc.proto)
            {
                case RMCP.PROTOCOL.AuthenticationService:
                    AuthenticationService.HandleAuthenticationServiceRequest(p, rmc, client);
                    break;
                default:
                    Logger.Warning("RMCP not implemented " + rmc.proto);
                    break;
            }
        }

        public static void ProcessRequest(Client client, QPacket p, RMCP rmc)
        {
            MemoryStream m = new MemoryStream(p.payload);
            m.Seek(rmc._afterProtocolOffset, 0);
            rmc.callID = DataWriter.ReadUint32(m);
            rmc.methodID = DataWriter.ReadUint32(m);

            switch (rmc.proto)
            {
                case RMCP.PROTOCOL.AuthenticationService:
                    AuthenticationService.ProcessAuthenticationServiceRequest(m, rmc);
                    break;
            }
        }

        public static void SendResponseWithACK(UdpClient udp, QPacket p, RMCP rmc, Client client, RMCPResponse reply, bool useCompression = true, uint error = 0)
        {
            SendACK(udp, p, client);
            SendResponsePacket(udp, p, rmc, client, reply, useCompression, error);
        }

        private static void SendACK(UdpClient udp, QPacket p, Client client)
        {
            QPacket qp = new QPacket(p.toBuffer())
            {
                flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK },
                m_oSourceVPort = p.m_oDestinationVPort,
                m_oDestinationVPort = p.m_oSourceVPort,
                m_uiSignature = client.IDsend,
                payload = new byte[0],
                payloadSize = 0
            };
            Send(udp, qp, client);
        }

        private static void SendResponsePacket(UdpClient udp, QPacket p, RMCP rmc, Client client, RMCPResponse reply, bool useCompression, uint error)
        {
            MemoryStream m = new MemoryStream();
            if ((ushort)rmc.proto < 0x7F)
                DataWriter.WriteUint8(m, (byte)rmc.proto);
            else
            {
                DataWriter.WriteUint8(m, 0x7F);
                DataWriter.WriteUint16(m, (ushort)rmc.proto);
            }
            byte[] buff;
            if (error == 0)
            {
                DataWriter.WriteUint8(m, 0x1);
                DataWriter.WriteUint32(m, rmc.callID);
                DataWriter.WriteUint32(m, rmc.methodID | 0x8000);
                buff = reply.ToBuffer();
                if(buff != null) m.Write(buff, 0, buff.Length);                
            }
            else
            {
                DataWriter.WriteUint8(m, 0);
                DataWriter.WriteUint32(m, error);
                DataWriter.WriteUint32(m, rmc.callID);
            } 
            buff = m.ToArray();
            m = new MemoryStream();
            DataWriter.WriteUint32(m, (uint)buff.Length);
            m.Write(buff, 0, buff.Length);
            QPacket np = new QPacket(p.toBuffer())
            {
                flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_NEED_ACK },
                m_oSourceVPort = p.m_oDestinationVPort,
                m_oDestinationVPort = p.m_oSourceVPort,
                m_uiSignature = client.IDsend
            };
            MakeAndSend(client, np, m.ToArray());
        }

        public static void SendRequestPacket(UdpClient udp, QPacket p, RMCP rmc, Client client, RMCPResponse packet, bool useCompression, uint error)
        {
            MemoryStream m = new MemoryStream();
            if ((ushort)rmc.proto < 0x7F)
                DataWriter.WriteUint8(m, (byte)((byte)rmc.proto | 0x80));
            else
            {
                DataWriter.WriteUint8(m, 0xFF);
                DataWriter.WriteUint16(m, (ushort)rmc.proto);
            }
            byte[] buff;
            if (error == 0)
            {
                DataWriter.WriteUint32(m, rmc.callID);
                DataWriter.WriteUint32(m, rmc.methodID);
                buff = packet.ToBuffer();
                m.Write(buff, 0, buff.Length);
            }
            else
            {
                DataWriter.WriteUint32(m, error);
                DataWriter.WriteUint32(m, rmc.callID);
            }
            buff = m.ToArray();
            m = new MemoryStream();
            DataWriter.WriteUint32(m, (uint)buff.Length);
            m.Write(buff, 0, buff.Length);
            QPacket np = new QPacket(p.toBuffer())
            {
                flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_NEED_ACK },
                m_uiSignature = client.IDsend
            };
            MakeAndSend(client, np, m.ToArray());
        }

        public static void MakeAndSend(Client client, QPacket np, byte[] data)
        {
            MemoryStream m = new MemoryStream(data);
            if (data.Length < MaxRMCPayloadSize)
            {
                np.uiSeqId++;
                np.payload = data;
                np.payloadSize = (ushort)np.payload.Length;
                Send(client.udp, np, client);
            }
            else
            {
                np.flags.Add(QPacket.PACKETFLAG.FLAG_RELIABLE);
                int pos = 0;
                m.Seek(0, 0);
                np.m_byPartNumber = 0;
                while (pos < data.Length)
                {
                    np.uiSeqId = client.seqCounterReliable++;
                    bool isLast = false;
                    int len = (int)MaxRMCPayloadSize;
                    if (len + pos >= data.Length)
                    {
                        len = data.Length - pos;
                        isLast = true;
                    }
                    if (!isLast)
                        np.m_byPartNumber++;
                    else
                        np.m_byPartNumber = 0;
                    byte[] buff = new byte[len];
                    m.Read(buff, 0, len);
                    np.payload = buff;
                    np.payloadSize = (ushort)np.payload.Length;
                    Send(client.udp, np, client);
                    pos += len;
                }
            }
        }

        public static void Send(UdpClient udp, QPacket p, Client client)
        {
            byte[] data = p.toBuffer();
            udp.Send(data, data.Length, client.ep);
        }
    }
}