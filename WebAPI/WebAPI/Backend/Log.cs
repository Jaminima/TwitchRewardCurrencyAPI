using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend
{
    public static class Log
    {
        //static DateTime LastUpdated = DateTime.Now;
        public static void AppendToLog(string Text)
        {
            //if ((int)((TimeSpan)(DateTime.Now - LastUpdated)).TotalSeconds > 30)
            //{
            //    System.IO.File.AppendAllText("./Log.txt", "\n" + Text);
            //    LastUpdated = DateTime.Now;
            //}
        }
    }
}
