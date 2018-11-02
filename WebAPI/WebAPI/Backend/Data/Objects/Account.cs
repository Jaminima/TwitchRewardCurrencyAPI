using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class Account
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

    }
}
