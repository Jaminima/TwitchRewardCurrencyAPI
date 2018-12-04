using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace WebAPI.Backend.Data.Objects
{
    class AuthToken : BaseObject
    {
        public uint TokenId; // Create propertys to replicate the tables collumns
        public string RefreshToken,AuthorizationToken;
        public DateTime AuthorizationTokenCreated;


        public AuthToken(uint Id)
        {
            TokenId = Id;
        }

        public static AuthToken FromId(uint TokenId)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("TokenID",TokenId) };
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.AuthTokenID)=@TokenID));
",Params); // Select AuthToken information where the Id matches
            if (TData.Count == 0) { return null; } // Check if we have a result
            AuthToken AuthToken = new AuthToken(TokenId); // Put the selected data into a new AuthToken object
            AuthToken.RefreshToken = TData[0][1];
            AuthToken.AuthorizationToken = TData[0][2];
            try { AuthToken.AuthorizationTokenCreated = DateTime.Parse(TData[0][3]); } catch { }
            return AuthToken;
        }

        public static AuthToken FromRefreshToken(string RefreshToken)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("RefreshToken",RefreshToken) };
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.RefreshToken)=@RefreshToken));
",Params); // Select AuthToken information where RefreshToken matches
            if (TData.Count == 0) { return null; } // Check if we have a result
            AuthToken AuthToken = new AuthToken(uint.Parse(TData[0][0])); // Put the selected data into a new AuthToken object
            AuthToken.RefreshToken = TData[0][1];
            Update(AuthToken);
            return FromId(AuthToken.TokenId);
        }

        public static bool AuthTokenValid(string AuthorizationToken)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("AuthToken",AuthorizationToken) };
            List<String[]> TData = Init.SQLi.ExecuteReader(@"SELECT AuthTokens.AuthTokenID, AuthTokens.RefreshToken, AuthTokens.AuthToken, AuthTokens.AuthTokenCreated
FROM AuthTokens
WHERE (((AuthTokens.AuthToken)=@AuthToken));
", Params); // Select AuthToken information where AuthToken matches
            if (TData.Count == 0) { return false; } // Check if we have a result
            AuthToken AuthToken = new AuthToken(uint.Parse(TData[0][0])); // Put the selected data into a new AuthToken object
            AuthToken.AuthorizationToken = TData[0][2];
            try { AuthToken.AuthorizationTokenCreated = DateTime.Parse(TData[0][3]); }
            catch { return false; }
            if ((int)((TimeSpan)(DateTime.Now - AuthToken.AuthorizationTokenCreated)).TotalMinutes < 10) { return true; } // check if the authoken was created in the last 10 mins
            else { return false; }
        }

        public static void Update(AuthToken AuthToken)
        {
            List<OleDbParameter> Params = new List<OleDbParameter> { new OleDbParameter("RefreshToken",RandomString(32)),new OleDbParameter("AuthToken",RandomString(16)),
                new OleDbParameter("AuthTokenCreated",DateTime.Now.ToString()), new OleDbParameter("AuthTokenID",AuthToken.TokenId) };
            Init.SQLi.Execute(@"UPDATE AuthTokens SET AuthTokens.RefreshToken = @RefreshToken, AuthTokens.AuthToken = @AuthToken, AuthTokens.AuthTokenCreated = @AuthTokenCreated
WHERE(((AuthTokens.AuthTokenID) = @AuthTokenID));
", Params);
        }


        static string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        static string RandomString(int Length)
        {
            string String = "";
            for (int i = 0; i < Length; i++) { String += Alphabet[Init.Rnd.Next(0, Alphabet.Length)]; } // Iterate from 0 to length, populating the String as we go
            return String;
        }

    }
}
