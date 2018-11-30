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
        public string RefreshToken,AuthorizationToken;
        public DateTime AuthorizationTokenCreated;


        public AuthToken(uint Id)
        {
            TokenId = Id;
        }

        public static AuthToken FromId(uint TokenId)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.AuthTokenID)="+TokenId+@"));
");
            if (TData.Count == 0) { return null; }
            AuthToken AuthToken = new AuthToken(TokenId);
            AuthToken.RefreshToken = TData[0][1];
            AuthToken.AuthorizationToken = TData[0][2];
            try { AuthToken.AuthorizationTokenCreated = DateTime.Parse(TData[0][3]); } catch { }
            return AuthToken;
        }

        public static AuthToken FromRefreshToken(string RefreshToken)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.RefreshToken)='" + RefreshToken + @"'));
");
            if (TData.Count == 0) { return null; }
            AuthToken AuthToken = new AuthToken(uint.Parse(TData[0][0]));
            AuthToken.RefreshToken = TData[0][1];
            Update(AuthToken);
            return FromId(AuthToken.TokenId);
        }

        public static bool AuthTokenValid(string AuthorizationToken)
        {
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.AuthToken)='"+AuthorizationToken+@"'));
");
            if (TData.Count == 0) { return false; }
            AuthToken AuthToken = new AuthToken(uint.Parse(TData[0][0]));
            AuthToken.AuthorizationToken = TData[0][2];
            try { AuthToken.AuthorizationTokenCreated = DateTime.Parse(TData[0][3]); }
            catch { return false; }
            if ((int)((TimeSpan)(DateTime.Now - AuthToken.AuthorizationTokenCreated)).TotalMinutes < 10) { return true; }
            else { return false; }
        }

        public static void Update(AuthToken AuthToken)
        {
            Init.SQLi.Execute(@"UPDATE AuthTokens SET AuthTokens.RefreshToken = '"+RandomString(32)+@"', AuthTokens.AuthToken = '"+RandomString(16)+@"', AuthTokens.AuthTokenCreated = '"+DateTime.Now.ToString()+@"'
WHERE(((AuthTokens.AuthTokenID) = "+AuthToken.TokenId+@"));
");
        }


        static string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        static string RandomString(int Length)
        {
            string String = "";
            for (int i = 0; i < Length; i++) { String += Alphabet[Init.Rnd.Next(0, Alphabet.Length)]; }
            return String;
        }

    }
}
