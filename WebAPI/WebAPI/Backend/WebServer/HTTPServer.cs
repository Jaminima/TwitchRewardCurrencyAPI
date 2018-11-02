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
            Listener.Prefixes.Add("http://+:80/");
            Listener.Prefixes.Add("https://+:81/");
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
            Console.WriteLine(Context.Request.RemoteEndPoint + " Visited " + Context.Request.RawUrl + " Using " + Context.Request.HttpMethod);
            HttpListenerResponse Response = Context.Response;
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            ResponseObject ResponseObject=new ResponseObject();
            if (Context.Request.HttpMethod == "GET") { Get.Handler(Context,ref ResponseObject); }
            else if (Context.Request.HttpMethod == "POST") { Post.Handler(Context,ref ResponseObject); }
            byte[] ByteResponseData = Encoding.UTF8.GetBytes(ResponseObject.ToJson().ToString());
            if (Context.Request.RawUrl.EndsWith("/favicon.ico")) { ByteResponseData = System.IO.File.ReadAllBytes("./icon.png"); }//Only visable on web browsers
            Response.OutputStream.Write(ByteResponseData, 0, ByteResponseData.Length);
            Response.OutputStream.Close();
        }
    }
}
