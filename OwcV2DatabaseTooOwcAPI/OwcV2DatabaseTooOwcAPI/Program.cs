using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwcV2DatabaseTooOwcAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            SQL SQLiOld = new SQL("Old");
            SQL SQLiNew = new SQL("New");

            List<String[]> UserData = SQLiOld.ExecuteReader(@"SELECT UserData.OwlCoinID, UserData.TwitchID, UserData.DiscordID
FROM UserData;
");
            List<String[]> AccountData = SQLiOld.ExecuteReader(@"SELECT Accounts.OwlCoinID, Accounts.Balance
FROM Accounts;
");
            foreach(String[] User in UserData)
            {
                SQLiNew.Execute(@"INSERT INTO UserData (UserID,TwitchID,DiscordID) VALUES (" + User[0] + @",'" + User[1] + @"','" + User[2] + @"');");
            }
            foreach(String[] Account in AccountData)
            {
                SQLiNew.Execute(@"INSERT INTO Account (Balance,UserID) VALUES (" + Account[1] + @"," + Account[0] + @");");
            }
        }
    }
}
