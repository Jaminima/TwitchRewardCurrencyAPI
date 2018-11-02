using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class AuthToken : BaseObject
    {
        public uint TokenId;
        public string Token;
        public User User;

        public AuthToken(uint Id)
        {
            TokenId = Id;
        }

        public static AuthToken FromId(uint TokenId)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthToken, AuthTokens.UserID
FROM AuthTokens
WHERE (((AuthTokens.AuthTokenID)="+TokenId+@"));
");
            if (TData.Count == 0) { return null; }
            AuthToken AuthToken = new AuthToken(TokenId);
            AuthToken.Token = TData[0][0];
            AuthToken.User = User.FromId(uint.Parse(TData[0][1]));
            return AuthToken;
        }
    }
}
