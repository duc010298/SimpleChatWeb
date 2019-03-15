using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebChat.Models;

namespace WebChat.SignalR
{
    public class ChatHub : Hub
    {
        public void Connect()
        {
            System.Diagnostics.Debug.WriteLine("Connected");
            var user = Context.User;
            System.Diagnostics.Debug.WriteLine(user);
            System.Diagnostics.Debug.WriteLine(user.Identity.IsAuthenticated);
            System.Diagnostics.Debug.WriteLine(user.Identity.Name);
            System.Diagnostics.Debug.WriteLine("a");
        }

        public void SendMessageToUser(string toUserId, string message)
        {
            var currentUser = Context.User;
            string toUsername;
            string currentUserId;
            using (var db = new WebChatEntities())
            {
                Guid id = Guid.Parse(toUserId);
                var user = db.app_user.Where(s => s.app_user_id == id).FirstOrDefault();
                toUsername = user.username;
                user = db.app_user.Where(s => s.username == currentUser.Identity.Name).FirstOrDefault();
                currentUserId = user.app_user_id.ToString();
            }
            Clients.All.getMessages(currentUserId, message);
        }
    }
}