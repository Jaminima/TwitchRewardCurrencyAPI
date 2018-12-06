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
            Listener.Prefixes.Add("http://+:"+Data.ConfigHandler.Config["WebAPI"]["Port"]+"/"); // + indicates from anywhere, and we set the port to the value in the config file
            Listener.Start();
            Listener.BeginGetContext(HandleRequest, null); // Point the listener to the HandleRequest function
            Console.WriteLine("WebAPI Alive? "+Listener.IsListening);
        }

        public static void HandleRequest(IAsyncResult Request)
        {
            new Thread(() => RequestThread(Listener.EndGetContext(Request))).Start(); // Create a thread of RequestThread, this will prevent extended durations where the server cant respond
            Listener.BeginGetContext(HandleRequest, null); // Point the listener back too HandleRequest to allow for the server to respond
        }

        public static void RequestThread(HttpListenerContext Context)
        {
            string Event = Context.Request.RemoteEndPoint + " Visited " + Context.Request.RawUrl + " Using " + Context.Request.HttpMethod; // Create the message that will be put to console and the log file
            Console.WriteLine(Event);
            Log.AppendToLog(Event);
            HttpListenerResponse Response = Context.Response; // Store the listener-response to make it easier to point too
            Response.StatusCode = 200; // Indicate the status as 200, ie alive
            Response.ContentType = "application/json"; // Indicate that we will be sending a json
            ResponseObject ResponseObject=new ResponseObject(); // Create an instance of ResponseObject, which will be the main container for the data we will respond with
            try
            {
                if (Context.Request.HttpMethod == "GET") { Get.Handler(Context, ref ResponseObject); } // Run the Handler, that corresponds to the given method
                else if (Context.Request.HttpMethod == "POST") { Post.Handler(Context, ref ResponseObject); }
            }
            catch (Exception E) { Console.WriteLine(E); }
            byte[] ByteResponseData = Encoding.UTF8.GetBytes(ResponseObject.ToJson().ToString()); // Convert the ResponseObject to JSON and then to a Byte array
            try // Attempt to send the Response
            {
                Response.OutputStream.Write(ByteResponseData, 0, ByteResponseData.Length);
                Response.OutputStream.Close();
            } // Main reason it wont send is if the client has closed the connection
            catch { Console.WriteLine("Unable to send response too "+ Context.Request.RemoteEndPoint); }
        }
    }
}
