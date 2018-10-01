using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class HTTPServer
    {
        static HttpListener Listener = new HttpListener();
        public static void Start()
        {
            Listener.Prefixes.Add("http://+:1234/");
            Listener.Prefixes.Add("https://+:12345/");
            //MUST FIGURE OUT SSL
            Listener.Start();
            Listener.BeginGetContext(HandleRequest, null);
        }
        public static HttpListenerContext GetRequestData(IAsyncResult Request)
        {
            HttpListenerContext Data = Listener.EndGetContext(Request);
            Listener.BeginGetContext(HandleRequest, null);
            return Data;
        }

        public static void HandleRequest(IAsyncResult Request)
        {
            HttpListenerContext RequestData = HTTPServer.GetRequestData(Request);
            RecivedGET(RequestData);
        }

        static void RecivedGET(HttpListenerContext RequestData)
        {
            HttpListenerResponse Response = RequestData.Response;
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            string ResponseData = RequestHandler.Handler(RequestData);
            byte[] ByteResponseData = Encoding.UTF8.GetBytes(ResponseData);
            Response.OutputStream.Write(ByteResponseData, 0, ByteResponseData.Length);
            Response.OutputStream.Close();
        }
    }
}
