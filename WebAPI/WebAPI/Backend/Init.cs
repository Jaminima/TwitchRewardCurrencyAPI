using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend
{
    public static class Init
    {
        public static void Start()
        {
            WebServer.HTTPServer.Start();
            while (true) { Console.ReadLine(); }
        }
    }
}
