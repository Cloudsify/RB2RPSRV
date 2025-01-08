using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Quazal;

namespace RB2RPSRV
{
    public static class AuthServer
    {
        public static readonly object _sync = new object();
        public static bool _exit = false;
        private static UdpClient listener;
        private static ushort listenPort = 30825;



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
            Logger.Info("[RB2RPSRV] [AUTH] Started on " + IPAddress.Loopback + ":" + listenPort);
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                } 
            }
            Logger.Info("[RB2RPSRV] [AUTH] Server Stopped");
        }

        public static void ProcessPacket(byte[] data, IPEndPoint ep)
        {
            QPacket p = new QPacket(data);

            switch(p.type)
            {
                case QPacket.PACKETTYPE.SYN:
                    Client.reset();

                    Logger.Warning("[RB2RPSRV] [AUTH] SYN Packet Type Called");
                    QPacketHandler.ProcessPacket("AuthServer", data, ep, listener, 0, 0, true);
                    break;
                case QPacket.PACKETTYPE.CONNECT:
                    Logger.Warning("[RB2RPSRV] [AUTH] CONNECT Packet Type Called");
                    QPacketHandler.ProcessPacket("AuthServer", data, ep, listener, 0, 0, false);
                    break;
                case QPacket.PACKETTYPE.DATA:
                    Logger.Warning("[RB2RPSRV] [AUTH] DATA Packet Type Called");
                    QPacketHandler.ProcessPacket("AuthServer", data, ep, listener, 0, 0, true);
                    break;
                case QPacket.PACKETTYPE.DISCONNECT:
                    Logger.Warning("[RB2RPSRV] [AUTH] DISCONNECT Packet Type Called");
                    QPacketHandler.ProcessPacket("AuthServer", data, ep, listener, 0, 0, true);
                    break;
                case QPacket.PACKETTYPE.PING:
                    Logger.Warning("[RB2RPSRV] [AUTH] PING Packet Type Called");
                    QPacketHandler.ProcessPacket("AuthServer", data, ep, listener, 0, 0, true);
                    break;
            }
        }
    }
}