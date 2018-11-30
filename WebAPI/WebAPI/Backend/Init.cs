using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend
{
    public static class Init
    {
        public static Data.SQL SQLi = new Data.SQL("./Database"); // Create an instance of the SQL class
        public static Random Rnd = new Random(); // Having one Random function will save slightly on memory
        public static void Start()
        {
            Data.ConfigHandler.LoadConfig(); // Load the configuration files
            WebServer.HTTPServer.Start(); // Start the WebServer
            while (true)
            { // Simple terminal commands
                string Command=Console.ReadLine().ToLower();
                if (Command == "clear") { Console.Clear(); } // Remove clutter from terminal
            }
        }
    }
}
