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
            if (Data.Objects.AuthToken.AuthTokenValid(Context.Request.Headers["AuthorizationToken"])) // Determine the validity of the token provided in the request headers
            { ResponseObject.Message = "Token Is Valid"; ResponseObject.Status = 200; return true; } // Set the message and status based on the validity
            else
            { ResponseObject.Message = "Token Is InValid"; ResponseObject.Status = 500; return false; }
        }
    }
}
