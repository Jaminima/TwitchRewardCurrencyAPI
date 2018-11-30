using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    public class BaseObject // The baseobject is inherited by the other objects, so we can have generic functions be shared, reducing code repeats.
    {
        public Newtonsoft.Json.Linq.JToken ToJson()
        {
            return Newtonsoft.Json.Linq.JToken.FromObject(this);
        }
    }
}
