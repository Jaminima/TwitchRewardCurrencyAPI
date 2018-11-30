using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.WebServer
{
    public class ResponseObject:Data.Objects.BaseObject
    {
        public int Status=400; // This status is independant to the actual StatusCode, as we need the ability to indicate an error has occured, without causing problems when determining if the server is alive.
        public string Message="Unknown Error"; // The message will give a simple explaination of any fault, is intended mainly for debugging
        public Newtonsoft.Json.Linq.JToken Data; // Will store the actual data we want to return, eg a list of Users
    }
}
