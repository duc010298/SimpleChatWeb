using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class FindFriendController : Controller
    {
        [HttpGet]
        [CheckAuthorization]
        public ActionResult FindFriend()
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                ViewBag.Avatar = userInfo.avatar;
                ViewBag.Fullname = userInfo.fullname;
            }
            ViewBag.View = "findfriend";
            return View();
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult FindFriend(string input)
        {
            Guid userID = (Guid)Session["UserID"];
            List<FriendToFind> list = new List<FriendToFind>();
            using(var db = new WebChatEntities())
            {
                var cusomerList = db.customers.Where(s => (s.fullname.Contains(input) || s.app_user.username.Contains(input)
                || s.email.Contains(input) || s.phone.Contains(input)) && s.app_user_id != userID).ToList();
                foreach (var cus in cusomerList)
                {
                    FriendToFind friendToFind = new FriendToFind();
                    friendToFind.Id = cus.app_user_id.ToString();
                    friendToFind.Avatar = cus.avatar;
                    friendToFind.Name = cus.fullname;
                    friendToFind.Email = cus.email;
                    friendToFind.Phone = cus.phone;
                    friendToFind.Birth = cus.birth.ToString("dd/MM/yyyy");
                    friendToFind.Gender = cus.gender ? "Nam" : "Nữ";
                    friendToFind.City = cus.city;
                    friendToFind.Description = cus.customer_description;
                    var relation = db.relationships.Where(s => (s.cus1_id == userID && s.cus2_id == cus.app_user_id) || (s.cus1_id == cus.app_user_id && s.cus2_id == userID)).FirstOrDefault();
                    friendToFind.StatusRelation = relation == null ? 3 : relation.relationship_status;
                    list.Add(friendToFind);
                }
            }
            return Json(list);
        }
    }
}