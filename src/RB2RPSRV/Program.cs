using System;

namespace RB2RPSRV
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Quazal.Program.getVersion();
            AuthServer.Start();
            SecureServer.Start();
        }
    }
}
