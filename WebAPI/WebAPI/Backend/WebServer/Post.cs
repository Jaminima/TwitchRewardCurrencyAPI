using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class Post
    {
        public static void Handler(HttpListenerContext Context,ref ResponseObject ResponseObject)
        {
            ResponseObject.Message = "Path Not Found"; ResponseObject.Status = 404;
        }
    }
}
