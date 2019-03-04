using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class WebChatController : Controller
    {
        // GET: WebChat
        [HttpGet]
        [CheckAuthorization]
        public ActionResult Index()
        {
            return View();
        }
    }
}