using Quazal;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace RB2RPSRV
{
    public static class SecureServer
    {
        public static readonly object _sync = new object();
        public static bool _exit = false;
        private static UdpClient listener;
        private static ushort listenPort = 30826;



        public static void Start()
        {
            _exit = false;
            new Thread(kMainThread).Start();
        }

        public static void Stop()
        {
            lock (_sync)
            {
                _exit = true;
            }
            if (listener != null)
            {
                listener.Close();
            }
        }

        public static void kMainThread(object obj)
        {
            Logger.Info("[RB2RPSRV] [SECURE] Started on " + IPAddress.Loopback + ":" + listenPort);
            listener = new UdpClient(listenPort);
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 0);
            while (true) 
            {
                lock (_sync)
                {
                    if (_exit)
                        break;
                }
                try
                {
                    byte[] bytes = listener.Receive(ref ep);
                    ProcessPacket(bytes, ep);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                } 
            }
            Logger.Info("[RB2RPSRV] [SECURE] Server Stopped");
        }

        public static void ProcessPacket(byte[] data, IPEndPoint ep)
        {
            QPacket p = new QPacket(data);

            switch (p.type)
            {
                case QPacket.PACKETTYPE.SYN:
                    // Implement SYN
                    Logger.Warning("[RB2RPSRV] [SECURE] SYN Packet Type Not Implemeneted");
                    break;
                case QPacket.PACKETTYPE.CONNECT:
                    // Implement CONNECT
                    Logger.Warning("[RB2RPSRV] [SECURE] CONNECT Packet Type Not Implemeneted");
                    break;
                case QPacket.PACKETTYPE.DATA:
                    // Implement DATA
                    Logger.Warning("[RB2RPSRV] [SECURE] DATA Packet Type Not Implemeneted");
                    break;
                case QPacket.PACKETTYPE.DISCONNECT:
                    // Implement DISCONNECT
                    Logger.Warning("[RB2RPSRV] [SECURE] DISCONNECT Packet Type Not Implemeneted");
                    break;
                case QPacket.PACKETTYPE.PING:
                    // Implement PING
                    Logger.Warning("[RB2RPSRV] [SECURE] PING Packet Type Not Implemeneted");
                    break;
            }
        }
    }
}