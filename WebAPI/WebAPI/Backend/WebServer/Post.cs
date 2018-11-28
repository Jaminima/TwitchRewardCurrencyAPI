using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebAPI.Backend.WebServer
{
    public static class Post
    {
        public static void Handler(HttpListenerContext Context,ref ResponseObject ResponseObject)
        {
            string[] SegmentedURL = Context.Request.RawUrl.Split("/".ToCharArray());
            if (SegmentedURL[1] == "create" && SegmentedURL[2] == "user")
            {
                if (!Misc.TokenValid(Context, ref ResponseObject)) { return; }
                Data.Objects.NewUser NewUser = new Data.Objects.NewUser();
                NewUser.DiscordId = Context.Request.Headers["DiscordId"];
                NewUser.TwitchId = Context.Request.Headers["TwitchId"];
                if (Data.Objects.User.UserExists(NewUser.TwitchId, NewUser.DiscordId)) { ResponseObject.Message = "A User Already Exists"; ResponseObject.Status = 505; return; }
                Data.Objects.NewUser.Save(NewUser);
                ResponseObject.Message = "Created User"; ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1]=="user")
            {
                Data.Objects.NewUser NewUser = new Data.Objects.NewUser();
                NewUser.DiscordId = Context.Request.Headers["DiscordId"];
                NewUser.TwitchId = Context.Request.Headers["TwitchId"];
                Data.Objects.User User = Data.Objects.User.FromNewUser(NewUser);
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                ResponseObject.Data = User.ToJson();
                ResponseObject.Message = "Got User";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "update" && SegmentedURL[2] == "user")
            {
                if (!Misc.TokenValid(Context, ref ResponseObject)) { return; }
                string StreamString = new System.IO.StreamReader(Context.Request.InputStream).ReadToEnd();
                Newtonsoft.Json.Linq.JToken User = Newtonsoft.Json.Linq.JToken.Parse(StreamString);
                Data.Objects.User.Update(User.ToObject<Data.Objects.User>());
                ResponseObject.Message = "Updated User";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "delete" && SegmentedURL[2] == "user")
            {
                if (SegmentedURL.Length != 4) { ResponseObject.Message = "Missing Parameter"; ResponseObject.Status = 401; return; }
                try { int.Parse(SegmentedURL[3]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[3]));
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                Data.Objects.User.Delete(User);
                ResponseObject.Data = User.ToJson();
                ResponseObject.Message = "Deleted User";
                ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "account" && SegmentedURL[2] == "give")
            {
                if (!Misc.TokenValid(Context,ref ResponseObject)) { return; }
                try { int.Parse(SegmentedURL[3]); int.Parse(Context.Request.Headers["Value"]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[3]));
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                User.Account.Balance += uint.Parse(Context.Request.Headers["Value"]);
                Data.Objects.Account.Update(User.Account);
                ResponseObject.Message = "Adjusted Users Balance"; ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "account" && SegmentedURL[2] == "take")
            {
                if (!Misc.TokenValid(Context, ref ResponseObject)) { return; }
                try { int.Parse(SegmentedURL[3]); int.Parse(Context.Request.Headers["Value"]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[3]));
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                uint ChangeBy = uint.Parse(Context.Request.Headers["Value"]);
                if (User.Account.Balance < ChangeBy) { ResponseObject.Message = "Insufficient Balance"; ResponseObject.Status = 205; return; }
                User.Account.Balance -= ChangeBy;
                Data.Objects.Account.Update(User.Account);
                ResponseObject.Message = "Adjusted Users Balance"; ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "account" && SegmentedURL[2] == "set")
            {
                if (!Misc.TokenValid(Context, ref ResponseObject)) { return; }
                try { int.Parse(SegmentedURL[3]); int.Parse(Context.Request.Headers["Value"]); } catch { ResponseObject.Message = "Invalid Parameter"; ResponseObject.Status = 500; return; }
                Data.Objects.User User = Data.Objects.User.FromId(uint.Parse(SegmentedURL[3]));
                if (User == null) { ResponseObject.Message = "User doesnt exist"; ResponseObject.Status = 405; return; }
                User.Account.Balance = uint.Parse(Context.Request.Headers["Value"]);
                Data.Objects.Account.Update(User.Account);
                ResponseObject.Message = "Set Users Balance"; ResponseObject.Status = 200;
            }
            else if (SegmentedURL[1] == "auth" && SegmentedURL[2] == "check")
            {
                Misc.TokenValid(Context,ref ResponseObject);
            }
            else if (SegmentedURL[1] == "auth" && SegmentedURL[2] == "token")
            {
                Data.Objects.AuthToken AuthToken = Data.Objects.AuthToken.FromRefreshToken(Context.Request.Headers["RefreshToken"]);
                if (AuthToken != null) { ResponseObject.Data = AuthToken.ToJson(); ResponseObject.Message = "Succesfully performed 0Auth";ResponseObject.Status = 200; }
                else { ResponseObject.Message = "That RefreshToken isnt valid"; ResponseObject.Status = 400; }
            }
            else if (SegmentedURL[1] == "log")
            {
                if (!Misc.TokenValid(Context, ref ResponseObject)) { return; }
                ResponseObject.Message = "Read Log"; ResponseObject.Status = 200;
                ResponseObject.Data = System.IO.File.ReadAllText("./Log.txt");
            }
            else { ResponseObject.Message = "Path Not Found"; ResponseObject.Status = 404; }
        }
    }
}
