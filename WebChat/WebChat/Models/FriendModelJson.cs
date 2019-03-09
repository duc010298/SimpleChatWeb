using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendModelJson
    {
        public string FriendId { get; set; }
        public string FriendName { get; set; }
        public string Avatar { get; set; }
        public bool Status_online { get; set; }
        public string LastMessage { get; set; }
        public string LastSendTime { get; set; }
    }
}