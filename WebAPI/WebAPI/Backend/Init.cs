using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend
{
    public static class Init
    {
        public static Data.SQL SQLi = new Data.SQL("./Database");
        public static Random Rnd = new Random();
        public static void Start()
        {
            Data.ConfigHandler.LoadConfig();
            WebServer.HTTPServer.Start();
            while (true)
            {
                string Command=Console.ReadLine().ToLower();
                if (Command == "clear") { Console.Clear(); }
            }
        }
    }
}
