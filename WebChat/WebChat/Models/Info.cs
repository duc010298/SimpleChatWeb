using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class Info
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public DateTimeOffset LastChangePassword { get; set; }
        public string Fullname { get; set; }
        public DateTime Birth { get; set; }
        public bool Gender { get; set; }
        public string City { get; set; }
        public string ShortDescription { get; set; }
    }
}