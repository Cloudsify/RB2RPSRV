using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quazal
{
    public class QPacket
    {
        public enum STREAMTYPE
        {
            Unused,
            DO,
            RV,
            OldRVSec,
            SBMGMT,
            NAT,
            SessionDiscovery,
            NATEcho,
            Routing,
            Game,
            RVSecure,
            Relay
        }

        public enum PACKETFLAG
        {
            FLAG_ACK = 1,
            FLAG_RELIABLE = 2,
            FLAG_NEED_ACK = 4,
            FLAG_HAS_SIZE = 8,
            FLAG_UNKNOWN = 16
        }

        public enum PACKETTYPE
        {
            SYN,
            CONNECT,
            DATA,
            DISCONNECT,
            PING,
            NATPING
        }

        public class VPort
        {
            public STREAMTYPE type;
            public byte port;
            public VPort(byte b)
            {
                type = (STREAMTYPE)(b >> 4);
                port = (byte)(b & 0xF);
            }

            public override string ToString()
            {
                return "VPort[port=" + port.ToString("D2") + " type=" + type + "]";
            }

            public byte toByte()
            {
                byte result = port;
                result |= (byte)((byte)type << 4);
                return result;
            }
        }

        public VPort m_oSourceVPort;
        public VPort m_oDestinationVPort;
        public byte m_byPacketTypeFlags;
        public PACKETTYPE type;
        public List<PACKETFLAG> flags;
        public byte m_bySessionID;
        public uint m_uiSignature;
        public ushort uiSeqId;
        public uint m_uiConnectionSignature;
        public byte m_byPartNumber;
        public ushort payloadSize;
        public byte[] payload;
        public byte checkSum;
        public bool usesCompression = true;
        public uint realSize;

        public QPacket()
        {
        }

        public QPacket(byte[] data)
        {
            MemoryStream m = new MemoryStream(data);
            m_oSourceVPort = new VPort(DataWriter.ReadUint8(m));
            m_oDestinationVPort = new VPort(DataWriter.ReadUint8(m));
            m_byPacketTypeFlags = DataWriter.ReadUint8(m);
            type = (PACKETTYPE)(m_byPacketTypeFlags & 0x7);
            flags = new List<PACKETFLAG>();
            ExtractFlags();
            m_bySessionID = DataWriter.ReadUint8(m);
            m_uiSignature = DataWriter.ReadUint32(m);
            uiSeqId = DataWriter.ReadUint16(m);
            if (type == PACKETTYPE.SYN || type == PACKETTYPE.CONNECT)
                m_uiConnectionSignature = DataWriter.ReadUint32(m);
            if (type == PACKETTYPE.DATA)
                m_byPartNumber = DataWriter.ReadUint8(m);
            if (flags.Contains(PACKETFLAG.FLAG_HAS_SIZE))
                payloadSize = DataWriter.ReadUint16(m);
            else
                payloadSize = (ushort)(m.Length - m.Position - 1);
            MemoryStream pl = new MemoryStream();
            if (payloadSize != 0)
                for (int i = 0; i < payloadSize; i++)
                    pl.WriteByte(DataWriter.ReadUint8(m));
            payload = pl.ToArray();
            if (payload != null && payload.Length > 0 && type != PACKETTYPE.SYN && m_oSourceVPort.type != STREAMTYPE.NAT)
            {
                if (m_oSourceVPort.type == STREAMTYPE.OldRVSec)
                    payload = DataWriter.Decrypt(Server.key, payload);
                usesCompression = payload[0] != 0;
                if (usesCompression)
                {
                    MemoryStream m2 = new MemoryStream();
                    m2.Write(payload, 1, payload.Length - 1);
                    payload = DataWriter.Decompress(m2.ToArray());
                }
                else
                {
                    MemoryStream m2 = new MemoryStream();
                    m2.Write(payload, 1, payload.Length - 1);
                    payload = m2.ToArray();
                }
                payloadSize = (ushort)payload.Length;
            }
            checkSum = DataWriter.ReadUint8(m);
            realSize = (uint)m.Position;
        }

        public byte[] toBuffer()
        {
            MemoryStream m = new MemoryStream();
            DataWriter.WriteUint8(m, m_oSourceVPort.toByte());
            DataWriter.WriteUint8(m, m_oDestinationVPort.toByte());
            byte typeFlag = (byte)type;
            foreach (PACKETFLAG flag in flags)
                typeFlag |= (byte)((byte)flag << 3);
            DataWriter.WriteUint8(m, typeFlag);
            DataWriter.WriteUint8(m, m_bySessionID);
            DataWriter.WriteUint32(m, m_uiSignature);
            DataWriter.WriteUint16(m, uiSeqId);
            if (type == PACKETTYPE.SYN || type == PACKETTYPE.CONNECT)
                DataWriter.WriteUint32(m, m_uiConnectionSignature);
            if(type == PACKETTYPE.DATA)
                DataWriter.WriteUint8(m, m_byPartNumber);
            byte[] tmpPayload = payload;
            if (tmpPayload != null && tmpPayload.Length > 0 && type != PACKETTYPE.SYN && m_oSourceVPort.type != STREAMTYPE.NAT)
            {
                if (usesCompression)
                {
                    uint sizeBefore = (uint)tmpPayload.Length;
                    byte[] buff = DataWriter.Compress(tmpPayload);
                    byte count = (byte)(sizeBefore / buff.Length);
                    if ((sizeBefore % buff.Length) != 0)
                        count++;
                    MemoryStream m2 = new MemoryStream();
                    m2.WriteByte(count);
                    m2.Write(buff, 0, buff.Length);
                    tmpPayload = m2.ToArray();

                }
                else
                {
                    MemoryStream m2 = new MemoryStream();
                    m2.WriteByte(0);
                    m2.Write(tmpPayload, 0, tmpPayload.Length);
                    tmpPayload = m2.ToArray();
                }
                if (m_oSourceVPort.type == STREAMTYPE.OldRVSec)
                    tmpPayload = DataWriter.Encrypt(Server.key, tmpPayload);
            }
            if (flags.Contains(PACKETFLAG.FLAG_HAS_SIZE))
                DataWriter.WriteUint16(m, (ushort)tmpPayload.Length);
            m.Write(tmpPayload, 0, tmpPayload.Length);
            return AddCheckSum(m.ToArray());
        }

        private byte[] AddCheckSum(byte[] buff)
        {
            byte[] result = new byte[buff.Length + 1];
            for (int i = 0; i < buff.Length; i++)
                result[i] = buff[i];

            byte checksum = CalculateChecksum(buff, Client.signatureBase);

            result[buff.Length] = checksum;
            return result;
        }


        private static byte GetProtocolSetting(byte proto)
        {
            switch (proto)
            {
                case 3:
                    return 0xE3;
                case 1:
                case 5:
                default:
                    return 0x00;
            }
        }

        public static byte CalculateChecksum(byte[] data, uint signatureBase)
        {
            int steps = data.Length / 4;
            uint temp = 0;

            for (int i = 0; i < steps; i++)
            {
                int offset = i * 4;
                temp += BitConverter.ToUInt32(data, offset);
            }

            temp &= 0xFFFFFFFF;

            byte[] buff = new byte[4];
            BitConverter.GetBytes(temp).CopyTo(buff, 0);

            uint checksum = signatureBase;
            checksum += Client.Sum(data.Skip(data.Length & ~3).ToArray());
            checksum += Client.Sum(buff);

            return (byte) (checksum & 0xFF);
        }



        private void ExtractFlags()
        {
            byte v = (byte)(m_byPacketTypeFlags >> 3);
            int[] values = (int[])Enum.GetValues(typeof(PACKETFLAG));
            for (int i = 0; i < values.Length; i++)
                if ((v & values[i]) != 0)
                    flags.Add((PACKETFLAG)values[i]);
        }
    }
}