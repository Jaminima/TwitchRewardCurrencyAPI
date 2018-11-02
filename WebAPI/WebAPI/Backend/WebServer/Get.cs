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
            string[] SegmentedURL = Context.Request.RawUrl.Split("/".ToCharArray());
            if (SegmentedURL[1] == "user")
            {
                if (SegmentedURL.Length != 3) { ResponseObject.Message = "Missing Parameter"; ResponseObject.Status = 401; return; }
                try { int.Parse(SegmentedURL[2]); } catch { ResponseObject.Message = "Invalid Parameter";ResponseObject.Status = 500; return;  }
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[2]));
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                ResponseObject.Data = User.ToJson();
                ResponseObject.Message = "Got User";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1]=="all"&& SegmentedURL[2] == "users")
            {
                Data.Objects.User[] User = Data.Objects.User.AllUsers();
                ResponseObject.Data = Newtonsoft.Json.Linq.JToken.FromObject(User);
                ResponseObject.Message = "Got All Users";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "account")
            {
                if (SegmentedURL.Length != 3) { ResponseObject.Message = "Missing Parameter"; ResponseObject.Status = 401; return; }
                try { int.Parse(SegmentedURL[2]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }
                Data.Objects.Account Account = Data.Objects.Account.FromId(uint.Parse(SegmentedURL[2]));
                if (Account == null) { ResponseObject.Message = "Account doesnt exist"; ResponseObject.Status = 405; return; }
                ResponseObject.Data = Account.ToJson();
                ResponseObject.Message = "Got Account";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "all" && SegmentedURL[2] == "accounts")
            {
                Data.Objects.Account[] Accounts = Data.Objects.Account.AllAccounts();
                ResponseObject.Data = Newtonsoft.Json.Linq.JToken.FromObject(Accounts);
                ResponseObject.Message = "Got All Accounts";
                ResponseObject.Status = 200;
            }
            else { ResponseObject.Message = "Path Not Found"; ResponseObject.Status = 404; }
        }
    }
}
