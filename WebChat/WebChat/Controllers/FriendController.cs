using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class FriendController : Controller
    {
        [HttpGet]
        [CheckAuthorization]
        public ActionResult ListFriend()
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                ViewBag.Avatar = userInfo.avatar;
                ViewBag.Fullname = userInfo.fullname;
            }
            ViewBag.View = "friend";
            return View();
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetFriend(int page)
        {
            int numberFriendInOnePage = 30;
            Guid userID = (Guid)Session["UserID"];
            List<FriendForListFriend> FriendForListFriend = new List<FriendForListFriend>();
            using (var db = new WebChatEntities())
            {
                var listRelationShip = db.relationships.Where(s => (s.cus1_id == userID || s.cus2_id == userID) && (s.relationship_status == 1 || s.relationship_status == 2)).OrderBy(s => s.relationship_id)
                    .Skip(numberFriendInOnePage * (page - 1)).Take(numberFriendInOnePage).ToList();
                foreach (var rela in listRelationShip)
                {
                    if(rela.cus1_id.Equals(userID))
                    {
                        var friendInfo = db.customers.Where(s => s.app_user_id == rela.cus2_id).FirstOrDefault();
                        FriendForListFriend temp = new FriendForListFriend();
                        temp.Id = friendInfo.app_user_id.ToString();
                        temp.Avatar = friendInfo.avatar;
                        temp.Fullname = friendInfo.fullname;
                        temp.RelationshipStatus = rela.relationship_status;
                        FriendForListFriend.Add(temp);
                    } else
                    {
                        var friendInfo = db.customers.Where(s => s.app_user_id == rela.cus1_id).FirstOrDefault();
                        FriendForListFriend temp = new FriendForListFriend();
                        temp.Id = friendInfo.app_user_id.ToString();
                        temp.Avatar = friendInfo.avatar;
                        temp.Fullname = friendInfo.fullname;
                        temp.RelationshipStatus = rela.relationship_status;
                        FriendForListFriend.Add(temp);
                    }
                }
            }
            return Json(FriendForListFriend);
        }

        [HttpPost]
        [CheckAuthorization]
        public ActionResult GetFriendSearch(int page, string input)
        {
            int numberFriendInOnePage = 30;
            Guid userID = (Guid)Session["UserID"];
            List<FriendForListFriend> FriendForListFriend = new List<FriendForListFriend>();
            using (var db = new WebChatEntities())
            {
                var listRelationShip = db.relationships.Where(s => ((s.cus1_id == userID && s.customer1.fullname.Contains(input)) 
                || (s.cus2_id == userID && s.customer.fullname.Contains(input))) 
                && (s.relationship_status == 1 || s.relationship_status == 2)).OrderBy(s => s.relationship_id)
                    .Skip(numberFriendInOnePage * (page - 1)).Take(numberFriendInOnePage).ToList();
                foreach (var rela in listRelationShip)
                {
                    if (rela.cus1_id.Equals(userID))
                    {
                        var friendInfo = db.customers.Where(s => s.app_user_id == rela.cus2_id).FirstOrDefault();
                        FriendForListFriend temp = new FriendForListFriend();
                        temp.Id = friendInfo.app_user_id.ToString();
                        temp.Avatar = friendInfo.avatar;
                        temp.Fullname = friendInfo.fullname;
                        temp.RelationshipStatus = rela.relationship_status;
                        FriendForListFriend.Add(temp);
                    }
                    else
                    {
                        var friendInfo = db.customers.Where(s => s.app_user_id == rela.cus1_id).FirstOrDefault();
                        FriendForListFriend temp = new FriendForListFriend();
                        temp.Id = friendInfo.app_user_id.ToString();
                        temp.Avatar = friendInfo.avatar;
                        temp.Fullname = friendInfo.fullname;
                        temp.RelationshipStatus = rela.relationship_status;
                        FriendForListFriend.Add(temp);
                    }
                }
            }
            return Json(FriendForListFriend);
        }

        [HttpGet]
        [CheckAuthorization]
        public ActionResult Unfriend(string id)
        {
            Guid userID = (Guid)Session["UserID"];
            System.Diagnostics.Debug.WriteLine(id);
            Guid friendId = Guid.Parse(id);
            System.Diagnostics.Debug.WriteLine(friendId);
            using (var db = new WebChatEntities())
            {
                var relationShip = db.relationships.Where(s => (s.cus1_id == friendId && s.cus2_id == userID) || (s.cus1_id == userID && s.cus2_id == friendId)).FirstOrDefault();
                db.relationships.Remove(relationShip);
                db.SaveChanges();
            }
            return RedirectToAction("ListFriend", "Friend");
        }
    }
}