using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendForListFriend
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Fullname { get; set; }
        public int RelationshipStatus { get; set; }
    }
}