using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    static class Misc
    {
        public static bool TokenValid(HttpListenerContext Context, ref ResponseObject ResponseObject)
        {
            if (Data.Objects.AuthToken.AuthTokenValid(Context.Request.Headers["AuthorizationToken"]))
            { ResponseObject.Message = "Token Is Valid"; ResponseObject.Status = 200; return true; }
            else
            { ResponseObject.Message = "Token Is InValid"; ResponseObject.Status = 500; return false; }
        }
    }
}
