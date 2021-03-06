﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;
using WebChat.SignalR;

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
            ViewBag.View = "index";
            return View();
        }

        [HttpGet]
        [CheckAuthorization]
        public ActionResult StartChat(string id)
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                ViewBag.Avatar = userInfo.avatar;
                ViewBag.Fullname = userInfo.fullname;
            }
            ViewBag.StartChat = id;
            ViewBag.View = "index";
            return View("index");
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetCurrentFriend()
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                //TODO need pagging here
                //For pagging get 20 lastMessageSend
                var lastMessageSend = db.messages.Where(s => s.cus_send_id == userID).OrderByDescending(s => s.send_time).GroupBy(s => s.cus_receive_id);
                //For pagging get 20 lastMessageReceive
                var lastMessageReceive = db.messages.Where(s => s.cus_receive_id == userID).OrderByDescending(s => s.send_time).GroupBy(s => s.cus_send_id);
                List<Guid> lastUserContact = new List<Guid>();
                foreach (var groupItem in lastMessageSend)
                {
                    lastUserContact.Add(groupItem.Key);
                }
                foreach (var groupItem in lastMessageReceive)
                {
                    lastUserContact.Add(groupItem.Key);
                }
                // => lastUserContact have max 40
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
                    var lastMessage = db.messages.Where(s => (s.cus_send_id == friendId && s.cus_receive_id == userID)
                    || (s.cus_receive_id == friendId && s.cus_send_id == userID)).OrderByDescending(s => s.send_time).FirstOrDefault();
                    friend.LastMessage = lastMessage.message1;
                    if (lastMessage.cus_send_id.Equals(userID))
                    {
                        friend.IsSend = true;
                        friend.MessageStatus = 2;
                    }
                    else
                    {
                        friend.MessageStatus = lastMessage.message_status;
                    }
                    friend.LastSendTime = lastMessage.send_time.ToString("o");
                    FriendList.Add(friend);
                }
                FriendList = FriendList.OrderByDescending(s => s.LastSendTime).ToList();
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
                //Change status
                var listMessage = db.messages.Where(s => s.cus_send_id == friendId && s.cus_receive_id == userID && (s.message_status == 0 || s.message_status == 1)).ToList();
                foreach (var mes in listMessage)
                {
                    mes.message_status = 2;
                }
                db.SaveChanges();
                //Get content
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

        [HttpPost]
        [CheckAuthorization]
        public void MakeAllRead(string id)
        {
            Guid userID = (Guid)Session["UserID"];
            var toUserId = Guid.Parse(id);
            using (var db = new WebChatEntities())
            {
                var messageList = db.messages.Where(s => s.cus_send_id == toUserId && s.cus_receive_id == userID && (s.message_status == 0 || s.message_status == 1)).ToList();
                foreach(var message in messageList)
                {
                    message.message_status = 2;
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetCurrentFriendSearch(string input)
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                List<Guid> lastUserContact = new List<Guid>();
                var listUserId = db.customers.Where(s => s.fullname.Contains(input)).ToList();
                foreach(var userId in listUserId)
                {
                    var temp = db.messages.Where(s => (s.cus_send_id == userID && s.cus_receive_id == userId.app_user_id) 
                    || (s.cus_receive_id == userID && s.cus_send_id == userId.app_user_id)).FirstOrDefault();
                    if (temp != null) lastUserContact.Add(userId.app_user_id);
                }
                List<FriendModelJson> FriendList = new List<FriendModelJson>();
                foreach (var friendId in lastUserContact)
                {
                    FriendModelJson friend = new FriendModelJson();
                    friend.FriendId = friendId.ToString();
                    var friendInfo = db.customers.Where(s => s.app_user_id == friendId).FirstOrDefault();
                    friend.FriendName = friendInfo.fullname;
                    friend.Avatar = friendInfo.avatar;
                    friend.Status_online = friendInfo.status_online;
                    var lastMessage = db.messages.Where(s => (s.cus_send_id == friendId && s.cus_receive_id == userID)
                    || (s.cus_receive_id == friendId && s.cus_send_id == userID)).OrderByDescending(s => s.send_time).FirstOrDefault();
                    friend.LastMessage = lastMessage.message1;
                    if (lastMessage.cus_send_id.Equals(userID))
                    {
                        friend.IsSend = true;
                        friend.MessageStatus = 2;
                    }
                    else
                    {
                        friend.MessageStatus = lastMessage.message_status;
                    }
                    friend.LastSendTime = lastMessage.send_time.ToString("o");
                    FriendList.Add(friend);
                }
                FriendList = FriendList.OrderByDescending(s => s.LastSendTime).ToList();
                return Json(FriendList);
            }
        }

        [HttpPost]
        [CheckAuthorization]
        public string SendImageMessage(HttpPostedFileBase file, string id)
        {
            Guid userID = (Guid)Session["UserID"];
            if (file != null && file.ContentLength > 0 && IsImage(file))
            {
                string directory = Server.MapPath("~/UploadedFiles/Image");
                string imageName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                string path = System.IO.Path.Combine(directory, imageName);

                using (Image image = Image.FromStream(file.InputStream, true, false))
                {
                    try
                    {
                        var thumbnailBitmap = new Bitmap(image.Width, image.Height);
                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imageRectangle = new Rectangle(0, 0, image.Width, image.Height);
                        thumbnailGraph.DrawImage(image, imageRectangle);
                        var ms = new MemoryStream();
                        thumbnailBitmap.Save(path, ImageFormat.Png);
                        ms.Position = 0;
                        GC.Collect();
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                Guid toUserId = Guid.Parse(id);
                string toUsername;
                using (var db = new WebChatEntities())
                {
                    var user = db.app_user.Where(s => s.app_user_id == toUserId).FirstOrDefault();
                    toUsername = user.username;
                }
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.User(toUsername).getMessagesImage(userID, imageName);
                return imageName;
            }
            else
            {
                return "Xin hãy chọn file ảnh";
            }
        }

        [HttpPost]
        [CheckAuthorization]
        public string SendFileMessage(HttpPostedFileBase file, string id)
        {
            Guid userID = (Guid)Session["UserID"];
            if (file != null && file.ContentLength > 0)
            {
                string directory = Server.MapPath("~/UploadedFiles/File");
                string fileName = file.FileName;
                string path = System.IO.Path.Combine(directory, fileName);

                file.SaveAs(path);

                Guid toUserId = Guid.Parse(id);
                string toUsername;
                using (var db = new WebChatEntities())
                {
                    var user = db.app_user.Where(s => s.app_user_id == toUserId).FirstOrDefault();
                    toUsername = user.username;
                }
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.User(toUsername).getMessagesFile(userID, fileName);
                return fileName;
            }
            else
            {
                return "Xin hãy chọn file";
            }
        }

        [HttpGet]
        [CheckAuthorization]
        public ActionResult DownloadFile(string fileName)
        {
            string directory = Server.MapPath("~/UploadedFiles/File");
            string path = System.IO.Path.Combine(directory, fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [NonAction]
        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }
            string[] formats = new string[] { ".jpg", ".png", ".bmp" };
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}