using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class NewUser : BaseObject
    {
        public string TwitchId, DiscordId;

        public static void Save(NewUser NewUser)
        {
            Init.SQLi.Execute(@"INSERT INTO UserData (TwitchID,DiscordID) VALUES ('"+NewUser.TwitchId+@"','"+NewUser.DiscordId+@"')");
            NewAccount NewAccount = new NewAccount();
            NewAccount.Balance = 0; NewAccount.User = User.FromNewUser(NewUser);
            NewAccount.Save(NewAccount);
        }
    }

    class User : BaseObject
    {
        public uint UserId;
        public string TwitchId, DiscordId;
        public Account Account;

        public User(uint Id) { UserId = Id; }

        public static User FromNewUser(NewUser NewUser)
        {
            string WhereString = "";
            if (NewUser.TwitchId != null) { WhereString += @"((UserData.TwitchID) = '" + NewUser.TwitchId + @"')"; }
            if (NewUser.TwitchId != null && NewUser.DiscordId != null) { WhereString += " AND "; }
            if (NewUser.DiscordId != null) { WhereString += @"((UserData.DiscordID)='" + NewUser.DiscordId + @"')"; }
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.UserID
FROM UserData
WHERE ( "+WhereString+@" );
");
            if (UData.Count == 0) { return null; }
            User User = new User(uint.Parse(UData[0][0]));
            User.TwitchId = NewUser.TwitchId;
            User.DiscordId = NewUser.DiscordId;
            User.Account = Account.FromUserId(User.UserId);
            return User;
        }

        public static User FromId(uint UserId)
        {
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.TwitchID, UserData.DiscordID
FROM UserData
WHERE (((UserData.UserID)="+UserId+@"));
");
            if (UData.Count == 0) { return null; }
            User User = new User(UserId);
            User.TwitchId = UData[0][0];
            User.DiscordId = UData[0][1];
            User.Account = Account.FromUserId(User.UserId);
            return User;
        }

        public static User FromIdChild(uint UserId)
        {
            User User = FromId(UserId);
            User.Account = null;
            return User;
        }

        public static User[] AllUsers()
        {
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.UserID, UserData.TwitchID, UserData.DiscordID
FROM UserData;
");
            List<User> Users = new List<User> { };
            foreach (String[] sUser in UData)
            {
                User User = new User(uint.Parse(sUser[0]));
                User.TwitchId = sUser[1];
                User.DiscordId = sUser[2];
                User.Account = Account.FromUserId(User.UserId);
                Users.Add(User);
            }
            return Users.ToArray();
        }

    }
}
