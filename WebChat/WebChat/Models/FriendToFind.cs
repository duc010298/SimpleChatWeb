using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class FriendToFind
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Birth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int StatusRelation { get; set; }
    }
}