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
                message mes = new message
                {
                    id = Guid.NewGuid(),
                    cus_send_id = user.app_user_id,
                    cus_receive_id = id,
                    message1 = message,
                    message_status = 1,
                    send_time = DateTimeOffset.Now
                };
                db.messages.Add(mes);
                db.SaveChanges();
            }
            Clients.User(toUsername).getMessages(currentUserId, message);
        }

        public override Task OnConnected()
        {
            var currentUser = Context.User;
            using (var db = new WebChatEntities())
            {
                var user = db.app_user.Where(s => s.username == currentUser.Identity.Name).FirstOrDefault();
                user.customer.status_online = true;
                db.SaveChanges();
            }
            return base.OnDisconnected(true);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var currentUser = Context.User;
            using (var db = new WebChatEntities())
            {
                var user = db.app_user.Where(s => s.username == currentUser.Identity.Name).FirstOrDefault();
                user.customer.status_online = false;
                user.customer.last_online = DateTimeOffset.Now;
                db.SaveChanges();
            }
            return base.OnDisconnected(true);
        }
    }
}