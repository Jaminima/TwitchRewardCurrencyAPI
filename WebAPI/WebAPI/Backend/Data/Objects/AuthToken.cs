using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Backend.Data.Objects
{
    class NewAuthToken : BaseObject
    {
        public string Token;
    }

    class AuthToken : BaseObject
    {
        public uint TokenId;
        public string Token;

        public AuthToken(uint Id)
        {
            TokenId = Id;
        }

        public static AuthToken FromId(uint TokenId)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthToken
FROM AuthTokens
WHERE (((AuthTokens.AuthTokenID)="+TokenId+@"));
");
            if (TData.Count == 0) { return null; }
            AuthToken AuthToken = new AuthToken(TokenId);
            AuthToken.Token = TData[0][0];
            return AuthToken;
        }

        public static bool AuthTokenExists(string Token)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID
FROM AuthTokens
WHERE (((AuthTokens.AuthToken)='" + Token + @"'));
");
            return TData.Count != 0;
        }

    }
}
