using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendAndChatContentModel
    {
        public Guid App_user_id { get; set; }
        public string Avatar { get; set; }
        public string Fullname { get; set; }
        public bool Status_online { get; set; }
        public string Last_online { get; set; }

        public List<MessageContentModel> Messages { get; set; }
    }
}