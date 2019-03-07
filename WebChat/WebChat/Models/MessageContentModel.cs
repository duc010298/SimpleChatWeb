using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class MessageContentModel
    {
        public string Content { get; set; }
        public string Send_time { get; set; }
        public int Message_status { get; set; }
        public bool IsSend { get; set; }
    }
}