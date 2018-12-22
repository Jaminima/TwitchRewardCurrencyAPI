using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data
{
    public static class Checks
    {
        static Char[] IDChars = "0123456789".ToCharArray();
        public static bool IsValidID(string ID)
        {
            foreach (Char C in ID)
            {
                if (!IDChars.Contains(C))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
