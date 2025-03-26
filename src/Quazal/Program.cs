using Quazal;

namespace Quazal
{
    public static class Program {
        public static void Main(string[] args) {
            getVersion();
        }

        public static void getVersion() {
            Logger.Info("Quazal Rendez-Vous Version: 0.1");
        }
    }
}