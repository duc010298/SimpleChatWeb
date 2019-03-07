using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendModel
    {
        public Guid FriendId { get; set; }
        public string FriendName { get; set; }
        public string Avatar { get; set; }
        public bool Status_online { get; set; }
        public string LastMessage { get; set; }
        public DateTimeOffset LastSendTime { get; set; }
    }
}