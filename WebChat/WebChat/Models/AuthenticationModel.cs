using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class AuthenticationModel
    {
        public LoginModel Login { get; set; }
        public RegisterModel Regesiter { get; set; }
    }
}