using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class User
    {
        public uint UserId, TwitchId, DiscordId;
        public Account Account;

        public User(uint Id) { UserId = Id; }

        public static User FromId(uint UserId)
        {
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.TwitchID, UserData.DiscordID
FROM UserData
WHERE (((UserData.UserID)="+UserId+@"));
");
            if (UData.Count == 0) { return null; }
            User User = new User(UserId);
            User.TwitchId = uint.Parse(UData[0][0]);
            User.DiscordId = uint.Parse(UData[0][1]);
            User.Account = Account.FromUserId(User.UserId);
            return User;
        }

        public static User FromIdChild(uint UserId)
        {
            User User = FromId(UserId);
            User.Account = null;
            return User;
        }

    }
}
