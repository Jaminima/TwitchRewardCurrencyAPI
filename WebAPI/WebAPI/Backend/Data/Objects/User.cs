using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace WebAPI.Backend.Data.Objects
{
    class NewUser : BaseObject
    {
        // NewUser does not have an UserId as it is not at reference to an item in the database
        public string TwitchId, DiscordId; // Create propertys to replicate the tables collumns

        public static void Save(NewUser NewUser)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("TwitchID",NewUser.TwitchId),new OleDbParameter("DiscordID",NewUser.DiscordId) };
            Init.SQLi.Execute(@"INSERT INTO UserData (TwitchID,DiscordID) VALUES (@TwitchID,@DiscordID)",Params);
            NewAccount NewAccount = new NewAccount();
            NewAccount.Balance = 0; NewAccount.User = User.FromNewUser(NewUser);
            NewAccount.Save(NewAccount);
        }
    }

    class User : BaseObject
    {
        public uint UserId; // Create propertys to replicate the tables collumns
        public string TwitchId, DiscordId;
        public Account Account;

        public User(uint Id) { UserId = Id; }

        public static User FromNewUser(NewUser NewUser)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("TwitchID",NewUser.TwitchId),new OleDbParameter("DiscordID",NewUser.DiscordId) };
            string WhereString = "";
            if (NewUser.TwitchId != null) { WhereString += @"((UserData.TwitchID) = @TwitchID)"; } // The Where statment must change based on if we have one or both of the Discord/Twitch ids
            if (NewUser.TwitchId != null && NewUser.DiscordId != null) { WhereString += " AND "; } // As we need it to only return data where both match, if both are given
            if (NewUser.DiscordId != null) { WhereString += @"((UserData.DiscordID)= @DiscordID)"; }
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.UserID,UserData.TwitchID,UserData.DiscordID
FROM UserData
WHERE ( " + WhereString+@" );
",Params); // Select User details where the previoulsy defined where statment applies
            if (UData.Count == 0) { return null; } // Check if we have a result
            User User = new User(uint.Parse(UData[0][0])); // Put the selected data into a new User object
            User.TwitchId = UData[0][1];
            User.DiscordId = UData[0][2];
            User.Account = Account.FromUserId(User.UserId);
            return User;
        }

        public static User FromId(uint UserId)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("UserID",UserId) };
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.TwitchID, UserData.DiscordID
FROM UserData
WHERE (((UserData.UserID)=@UserID));
",Params); // Select User details where the UserID matches
            if (UData.Count == 0) { return null; }
            User User = new User(UserId); // Put the selected data into a new Account object
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
"); // Select All User details
            List<User> Users = new List<User> { };
            foreach (String[] sUser in UData)
            {
                User User = new User(uint.Parse(sUser[0])); // Put the selected data into a new Account object
                User.TwitchId = sUser[1];
                User.DiscordId = sUser[2];
                User.Account = Account.FromUserId(User.UserId);
                Users.Add(User); // Add User object into the List
            }
            return Users.ToArray();
        }

        public static bool UserExists(string TwitchId="",string DiscordId="",uint Id=0)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("UserID",Id),new OleDbParameter("TwitchID",TwitchId),new OleDbParameter("DiscordID",DiscordId) };
            List<String[]> UData = Init.SQLi.ExecuteReader(@"SELECT UserData.UserID, UserData.TwitchID, UserData.DiscordID
FROM UserData
WHERE (((UserData.UserID)=@UserID)) AND (((UserData.TwitchID)=@TwitchID)) AND (((UserData.DiscordID)=@DiscordID));
",Params);
            return UData.Count != 0;
        }

        public static void Update(User User)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("TwitchID",User.TwitchId),new OleDbParameter("DiscordID",User.DiscordId),new OleDbParameter("UserID",User.UserId) };
            Init.SQLi.Execute(@"UPDATE UserData SET UserData.TwitchID = @TwitchID, UserData.DiscordID = @DiscordID
WHERE(((UserData.UserID) = @UserID));
",Params);
        }

        public static void Delete(User User)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("UserID",User.UserId) };
            Init.SQLi.Execute(@"DELETE UserData.UserID
FROM UserData
WHERE (((UserData.UserID)=@UserID));
",Params);
        }

    }
}
