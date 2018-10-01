using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data
{
    public static class Get
    {
        public static int GetBalance(string ID,IDType IDt)
        {
            if (IDt == IDType.UserID) {
                return int.Parse(Init.SQLi.ExecuteReader(@"SELECT Balance.Balance, UserData.UserID
FROM UserData INNER JOIN Balance ON UserData.UserID = Balance.UserID
WHERE (((UserData.UserID)="+ID+@"));
")[0][0]);
            }
            else if (IDt == IDType.Discord)
            {
                return int.Parse(Init.SQLi.ExecuteReader(@"SELECT Balance.Balance, UserData.DiscordID
FROM UserData INNER JOIN Balance ON UserData.UserID = Balance.UserID
WHERE (((UserData.DiscordID)='"+ID+@"'));
")[0][0]);
            }
            else if (IDt == IDType.Twitch)
            {
                return int.Parse(Init.SQLi.ExecuteReader(@"SELECT Balance.Balance, UserData.TwitchID
FROM UserData INNER JOIN Balance ON UserData.UserID = Balance.UserID
WHERE (((UserData.UserID)='" + ID + @"'));
")[0][0]);
            }
            return 0;
        }
    }
}
