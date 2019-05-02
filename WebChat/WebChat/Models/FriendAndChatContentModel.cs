using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendAndChatContentModel
    {
        public string AvatarCurrent { get; set; }
        public Guid FriendId { get; set; }
        public string AvatarFriend { get; set; }
        public string Fullname { get; set; }
        public bool Status_online { get; set; }
        public string Last_online { get; set; }

        public List<MessageContentModel> Messages { get; set; }
    }
}