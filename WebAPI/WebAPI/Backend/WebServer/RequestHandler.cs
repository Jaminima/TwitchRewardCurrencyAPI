using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class RequestHandler
    {
        public static string Handler(HttpListenerContext RequestData)
        {
            return "Alive But Confused...";
        }
    }
}
