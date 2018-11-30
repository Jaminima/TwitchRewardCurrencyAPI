using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data
{
    public static class ConfigHandler
    {
        public static Newtonsoft.Json.Linq.JToken Config;
        public static void LoadConfig()
        {
            string Str = System.IO.File.ReadAllText("./Api.config.json"); // Read the config file and the parse it to JSON form
            Config = Newtonsoft.Json.Linq.JToken.Parse(Str);
        }
    }
}
