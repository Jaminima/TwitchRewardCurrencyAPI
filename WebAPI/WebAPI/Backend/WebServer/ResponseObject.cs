using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.WebServer
{
    public class ResponseObject:Data.Objects.BaseObject
    {
        public int Status=400;
        public string Message="Unknown Error";
        public Newtonsoft.Json.Linq.JToken Data;
    }
}
