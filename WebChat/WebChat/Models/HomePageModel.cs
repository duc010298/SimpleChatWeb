using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class HomePageModel
    {
        public customer CurrentUser { get; set; }
        public List<FriendModel> LastFriendsContact { get; set; }
    }
}