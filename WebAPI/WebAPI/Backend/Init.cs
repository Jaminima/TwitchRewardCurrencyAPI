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
        public static void Start()
        {
            Data.ConfigHandler.LoadConfig();
            WebServer.HTTPServer.Start();
            while (true) { Console.ReadLine(); }
        }
    }
}
