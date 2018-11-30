using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class Get
    {
        public static void Handler(HttpListenerContext Context, ref ResponseObject ResponseObject)
        {
            string[] SegmentedURL = Context.Request.RawUrl.Split("/".ToCharArray()); // Split the url at all / so /All/Users becomes {"","All","Users"} so we can reference each position in the url with greater ease
            if (SegmentedURL[1] == "user")
            {
                if (SegmentedURL.Length != 3) { ResponseObject.Message = "Missing Parameter"; ResponseObject.Status = 401; return; } // Check how many positions are present in the array, if it isnt 3, then the UserID is missing
                try { int.Parse(SegmentedURL[2]); } catch { ResponseObject.Message = "Invalid Parameter";ResponseObject.Status = 500; return;  } // Check if where UserID should be, a number is present, otherwise, we indicate an error and return
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[2])); // Fetch the User object indicated by the UserID present in the URL
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; } // If User is null we indicate the error and return
                ResponseObject.Data = User.ToJson(); // Set the ResponseObject's data to the JSON version of the User Object
                ResponseObject.Message = "Got User"; // Set a message and status to indicate success
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1]=="all"&& SegmentedURL[2] == "users")
            {
                Data.Objects.User[] User = Data.Objects.User.AllUsers(); // Get an array of all User Objects
                ResponseObject.Data = Newtonsoft.Json.Linq.JToken.FromObject(User); // Set the ResponseObject's data to the JSON version of the Array of User Objects
                ResponseObject.Message = "Got All Users"; // Set a message and status to indicate success
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "account")
            {
                if (SegmentedURL.Length != 3) { ResponseObject.Message = "Missing Parameter"; ResponseObject.Status = 401; return; }// Check how many positions are present in the array, if it isnt 3, then the AccountID is missing
                try { int.Parse(SegmentedURL[2]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }// Check if where AccountID should be, a number is present, otherwise, we indicate an error and return
                Data.Objects.Account Account = Data.Objects.Account.FromId(uint.Parse(SegmentedURL[2])); // Fetch the Account Object indicated by the AccountID present in the URL
                if (Account == null) { ResponseObject.Message = "Account doesnt exist"; ResponseObject.Status = 405; return; } // If Account is null we indicate the error and return
                ResponseObject.Data = Account.ToJson(); // Set the ResponseObject's data to the JSON version of the Account Object
                ResponseObject.Message = "Got Account"; // Set a message and status to indicate success
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "all" && SegmentedURL[2] == "accounts")
            {
                Data.Objects.Account[] Accounts = Data.Objects.Account.AllAccounts(); // Get an array of all Account Objects
                ResponseObject.Data = Newtonsoft.Json.Linq.JToken.FromObject(Accounts); // Set the ResponseObject's data to the JSON version of the Array of Account Objects
                ResponseObject.Message = "Got All Accounts"; // Set a message and status to indicate success
                ResponseObject.Status = 200;
            }
            else { ResponseObject.Message = "Path Not Found"; ResponseObject.Status = 404; } // Indicate that the URL did not match any API paths
        }
    }
}
