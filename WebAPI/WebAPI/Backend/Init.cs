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
            Console.WriteLine(Data.Get.GetBalance("1",Data.IDType.UserID));
            while (true) { Console.ReadLine(); }
        }
    }
}
