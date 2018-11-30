using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class NewAccount : BaseObject
    {
        // NewAccount does not have an AccountId as it is not at reference to an item in the database
        public uint Balance; // Create propertys to replicate the tables collumns
        public User User;

        public static void Save(NewAccount NewAccount)
        {
            Init.SQLi.Execute(@"INSERT INTO Account (Balance,UserID) VALUES (0,"+NewAccount.User.UserId+@");");
        }
    }

    class Account:BaseObject
    {
        public uint AccountId; // Create propertys to replicate the tables collumns
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
"); // Select Account information where UserID matches
            if (AData.Count == 0) { return null; } // Check if we have a result
            Account Account = new Account(uint.Parse(AData[0][1])); // Put the selected data into a new Account object
            Account.Balance = uint.Parse(AData[0][0]);
            return Account;
        }

        public static Account FromId(uint AccountId)
        {
            List<string[]> AData = Init.SQLi.ExecuteReader(@"SELECT Account.Balance, Account.UserID
FROM Account
WHERE (((Account.AccountID)="+AccountId+@"));
"); // Select Account information where AccountID matches
            if (AData.Count == 0) { return null; } // Check if we have a result
            Account Account = new Account(AccountId); // Put the selected data into a new Account object
            Account.Balance = uint.Parse(AData[0][0]);
            Account.User = User.FromIdChild(uint.Parse(AData[0][1]));
            return Account;
        }

        public static Account FromIdChild(uint AccountId)
        {
            Account Account = FromId(AccountId);
            Account.User = null; // Prevents a recursive loop where it attempts to keep defining itself in terms of its parent
            return Account;
        }

        public static Account[] AllAccounts()
        {
            List<string[]> AData = Init.SQLi.ExecuteReader(@"SELECT Account.AccountID, Account.Balance, Account.UserID
FROM Account
ORDER BY Account.Balance DESC;
"); // Select All Account information
            List<Account> Accounts = new List<Account> { };
            foreach (string[] sAccount in AData)
            {
                Account Account = new Account(uint.Parse(sAccount[0])); // Put the selected data into a new Account object
                Account.Balance = uint.Parse(sAccount[1]);
                Account.User = User.FromIdChild(uint.Parse(sAccount[2]));
                Accounts.Add(Account); // Add Account object into the List
            }
            return Accounts.ToArray();
        }

        public static void Update(Account Account)
        {
            Init.SQLi.Execute(@"UPDATE Account SET Account.Balance = "+Account.Balance+@"
WHERE (((Account.AccountID)="+Account.AccountId+@"));
");
        }

        public static void Delete(Account Account)
        {
            Init.SQLi.Execute(@"DELETE Account.AccountID
FROM Account
WHERE (((Account.AccountID)="+Account.AccountId+@"));
");
        }

    }
}
