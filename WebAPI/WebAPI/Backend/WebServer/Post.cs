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
            else if (SegmentedURL[1] == "auth" && SegmentedURL[2] == "check")
            {
                if (Data.Objects.AuthToken.AuthTokenExists(Context.Request.Headers["AuthToken"]))
                { ResponseObject.Message = "Token Is Valid"; ResponseObject.Status = 200; }
                else
                { ResponseObject.Message = "Token Is InValid"; ResponseObject.Status = 500; }
            }
            else { ResponseObject.Message = "Path Not Found"; ResponseObject.Status = 404; }
        }
    }
}
