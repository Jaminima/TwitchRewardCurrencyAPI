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
            //WebServer.HTTPServer.Start();
            var Obj = Data.Objects.AuthToken.FromId(1);
            while (true) { Console.ReadLine(); }
        }
    }
}
