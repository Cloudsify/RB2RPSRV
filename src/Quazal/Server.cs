using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quazal
{
    public static class Server
    {
        public static readonly string key = "CD&ML"; // Quazal Encryption & Decryption key
        public static readonly string accessKey = "Ey6Ma18"; // (TODO) Use the access key from the config file
        public static string serverAddress = "127.0.0.1"; // Bind the server to this specific address
        public static uint idCounter = 0x56; // Not sure
        public static uint pidCounter = 0x1F4; // Not sure
        public static string sessionURL = "prudp:/address=127.0.0.1;port=0000;RVCID=1"; // Change this to Rock Band 2's sessionURL (TODO) grab data from config
        public static List<Client> clients = new List<Client>();

        public static Client GetClientByEndPoint(IPEndPoint ep)
        {
            Logger.Debug($"GetClientByEndPoint: Searching for client with endpoint {ep.Address}:{ep.Port}");

            foreach (Client c in clients)
            {
                if (c.ep.Address.ToString() == ep.Address.ToString() && c.ep.Port == ep.Port)
                {
                    Logger.Debug($"GetClientByEndPoint: Found client with endpoint {c.ep.Address}:{c.ep.Port}");
                    return c;
                }
            }

            Logger.Debug("GetClientByEndPoint: No client found with the specified endpoint.");
            return null;
        }

        public static Client GetClientByIDsend(uint id)
        {
            Logger.Debug($"GetClientByIDsend: Searching for client with IDsend {id}");

            foreach (Client c in clients)
            {
                if (c.IDsend == id)
                {
                    Logger.Debug($"GetClientByIDsend: Found client with IDsend {c.IDsend}");
                    return c;
                }
            }

            Logger.Debug($"GetClientByIDsend: No client found with IDsend {id}");
            return null;
        }

        public static Client GetClientByIDrecv(uint id)
        {
            Logger.Debug($"GetClientByIDrecv: Searching for client with IDrecv {id}");

            foreach (Client c in clients)
            {
                if (c.IDrecv == id)
                {
                    Logger.Debug($"GetClientByIDrecv: Found client with IDrecv {c.IDrecv}");
                    return c;
                }
            }

            Logger.Debug($"GetClientByIDrecv: No client found with IDrecv {id}");
            return null;
        }
    }
}