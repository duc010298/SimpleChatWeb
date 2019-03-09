using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class WebChatController : Controller
    {
        [HttpGet]
        [CheckAuthorization]
        public ActionResult Index()
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                ViewBag.Avatar = userInfo.avatar;
                ViewBag.Fullname = userInfo.fullname;
            }
            return View();
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetCurrentFriend()
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var lastMessageSend = db.messages.Where(s => s.cus_send_id == userID).OrderBy(s => s.send_time).GroupBy(s => s.cus_receive_id);
                var lastMessageReceive = db.messages.Where(s => s.cus_receive_id == userID).OrderBy(s => s.send_time).GroupBy(s => s.cus_send_id);
                List<Guid> lastUserContact = new List<Guid>();
                foreach (var groupItem in lastMessageSend)
                {
                    lastUserContact.Add(groupItem.Key);
                }
                foreach (var groupItem in lastMessageReceive)
                {
                    lastUserContact.Add(groupItem.Key);
                }
                lastUserContact = lastUserContact.Distinct().ToList();
                List<FriendModelJson> FriendList = new List<FriendModelJson>();
                foreach (var friendId in lastUserContact)
                {
                    FriendModelJson friend = new FriendModelJson();
                    friend.FriendId = friendId.ToString();
                    var friendInfo = db.customers.Where(s => s.app_user_id == friendId).FirstOrDefault();
                    friend.FriendName = friendInfo.fullname;
                    friend.Avatar = friendInfo.avatar;
                    friend.Status_online = friendInfo.status_online;
                    var lastMessage = db.messages.Where(s => s.cus_send_id == friendId || s.cus_receive_id == friendId).OrderByDescending(s => s.send_time).FirstOrDefault();
                    friend.LastMessage = lastMessage.message1;
                    friend.LastSendTime = lastMessage.send_time.ToString("o");
                    FriendList.Add(friend);
                }
                return Json(FriendList);
            }
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetChatContent(string guid, int page)
        {
            int numberOfMessageInOnePage = 20;
            Guid friendId = Guid.Parse(guid);
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                FriendAndChatContentModel friendInfo = new FriendAndChatContentModel();
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                friendInfo.AvatarCurrent = userInfo.avatar;
                var temp = db.customers.Where(s => s.app_user_id == friendId).FirstOrDefault();
                friendInfo.FriendId = temp.app_user_id;
                friendInfo.AvatarFriend = temp.avatar;
                friendInfo.Fullname = temp.fullname;
                friendInfo.Status_online = temp.status_online;
                friendInfo.Last_online = temp.last_online.ToString("o");
                var messages = db.messages.Where(s => (s.cus_send_id == userID && s.cus_receive_id == friendId)
                || (s.cus_send_id == friendId && s.cus_receive_id == userID)).OrderByDescending(s => s.send_time)
                .Skip(numberOfMessageInOnePage * (page - 1)).Take(numberOfMessageInOnePage).OrderBy(s => s.send_time).ToList();
                friendInfo.Messages = new List<MessageContentModel>();
                foreach (var message in messages)
                {
                    MessageContentModel messageContent = new MessageContentModel();
                    messageContent.Content = message.message1;
                    messageContent.Send_time = message.send_time.ToString("o");
                    messageContent.Message_status = message.message_status;
                    messageContent.IsSend = message.cus_send_id == userID ? true : false;
                    friendInfo.Messages.Add(messageContent);
                }
                return Json(friendInfo);
            }
        }
    }
}