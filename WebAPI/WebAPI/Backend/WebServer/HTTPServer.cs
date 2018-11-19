using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class HTTPServer
    {
        static HttpListener Listener = new HttpListener();
        public static void Start()
        {
            Listener.Prefixes.Add("http://+:"+Data.ConfigHandler.Config["WebAPI"]["Port"]+"/");
            //MUST FIGURE OUT SSL
            Listener.Start();
            Listener.BeginGetContext(HandleRequest, null);
            Console.WriteLine("WebAPI Alive? "+Listener.IsListening);
        }

        public static void HandleRequest(IAsyncResult Request)
        {
            new Thread(() => RequestThread(Listener.EndGetContext(Request))).Start();
            Listener.BeginGetContext(HandleRequest, null);
        }

        public static void RequestThread(HttpListenerContext Context)
        {
            string Event = Context.Request.RemoteEndPoint + " Visited " + Context.Request.RawUrl + " Using " + Context.Request.HttpMethod;
            Console.WriteLine(Event);
            Log.AppendToLog(Event);
            HttpListenerResponse Response = Context.Response;
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            ResponseObject ResponseObject=new ResponseObject();
            if (Context.Request.HttpMethod == "GET") { Get.Handler(Context,ref ResponseObject); }
            else if (Context.Request.HttpMethod == "POST") { Post.Handler(Context,ref ResponseObject); }
            byte[] ByteResponseData = Encoding.UTF8.GetBytes(ResponseObject.ToJson().ToString());
            //try
            //{
            //    if (Context.Request.RawUrl.EndsWith("/favicon.ico")) { ByteResponseData = System.IO.File.ReadAllBytes("./site/icon.png"); }//Only visable on web browsers
            //    if (Context.Request.RawUrl == "/") { ByteResponseData = System.IO.File.ReadAllBytes("./site/index.html"); Response.ContentType = "text/html"; }
            //    if (Context.Request.RawUrl.EndsWith(".html") || Context.Request.RawUrl.EndsWith(".js") || Context.Request.RawUrl.EndsWith(".png")) { ByteResponseData = System.IO.File.ReadAllBytes("./site" + Context.Request.RawUrl); Response.ContentType = "text/html"; }
            //    if (Context.Request.RawUrl.EndsWith(".css")) { ByteResponseData = System.IO.File.ReadAllBytes("./site" + Context.Request.RawUrl); Response.ContentType = "text/css"; }
            //}
            //catch { Encoding.UTF8.GetBytes("HMS fucked"); }
            try
            {
                Response.OutputStream.Write(ByteResponseData, 0, ByteResponseData.Length);
                Response.OutputStream.Close();
            }
            catch { Console.WriteLine("Unable to send response too "+ Context.Request.RemoteEndPoint); }
        }
    }
}
