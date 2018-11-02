using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class NewAccount : BaseObject
    {
        public uint Balance;
        public User User;

        public static void Save(NewAccount NewAccount)
        {
            Init.SQLi.Execute(@"INSERT INTO Account (Balance,UserID) VALUES (0,"+NewAccount.User.UserId+@");");
        }
    }

    class Account:BaseObject
    {
        public uint AccountId;
        public uint Balance;
        public User User;

        public Account(uint Id)
        {
            AccountId = Id;
        }

        public static Account FromUserId(uint UserId)
        {
            List<string[]> AData = Init.SQLi.ExecuteReader(@"SELECT Account.Balance, Account.AccountID
FROM Account
WHERE (((Account.UserID)=" + UserId + @"));
");
            if (AData.Count == 0) { return null; }
            Account Account = new Account(uint.Parse(AData[0][1]));
            Account.Balance = uint.Parse(AData[0][0]);
            return Account;
        }

        public static Account FromId(uint AccountId)
        {
            List<string[]> AData = Init.SQLi.ExecuteReader(@"SELECT Account.Balance, Account.UserID
FROM Account
WHERE (((Account.AccountID)="+AccountId+@"));
");
            if (AData.Count == 0) { return null; }
            Account Account = new Account(AccountId);
            Account.Balance = uint.Parse(AData[0][0]);
            Account.User = User.FromIdChild(uint.Parse(AData[0][1]));
            return Account;
        }

        public static Account FromIdChild(uint AccountId)
        {
            Account Account = FromId(AccountId);
            Account.User = null;
            return Account;
        }

        public static Account[] AllAccounts()
        {
            List<string[]> AData = Init.SQLi.ExecuteReader(@"SELECT Account.AccountID, Account.Balance, Account.UserID
FROM Account
ORDER BY Account.Balance DESC;
");
            List<Account> Accounts = new List<Account> { };
            foreach (string[] sAccount in AData)
            {
                Account Account = new Account(uint.Parse(sAccount[0]));
                Account.Balance = uint.Parse(sAccount[1]);
                Account.User = User.FromIdChild(uint.Parse(sAccount[2]));
                Accounts.Add(Account);
            }
            return Accounts.ToArray();
        }

    }
}
