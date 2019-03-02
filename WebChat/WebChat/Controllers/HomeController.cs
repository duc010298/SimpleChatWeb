using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [CheckAuthorization]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var userInfo = new LoginModel();
            try
            {
                // We do not want to use any existing identity information
                EnsureLoggedOut();
                return View(userInfo);
            }
            catch
            {
                throw;
            }
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        //POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
                // First we clean the authentication ticket like always
                //required NameSpace: using System.Web.Security;
                FormsAuthentication.SignOut();

                // Second we clear the principal to ensure the user does not retain any authentication
                //required NameSpace: using System.Security.Principal;
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                //Redirect to Index function
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel entity)
        {
            string encrypt_password = string.Empty;
            try
            {
                using (var db = new WebChatEntities())
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                        return View(entity);

                    //Retrive Stored HASH Value From Database According To Username (one unique field)
                    var userInfo = db.app_user.Where(s => s.username == entity.Username.Trim()).FirstOrDefault();

                    //Assign HASH Value
                    bool isLogin;
                    if (userInfo != null)
                    {
                        encrypt_password = userInfo.encrypted_password;
                        isLogin = BCrypt.Net.BCrypt.Verify(entity.Password, encrypt_password);
                    }
                    else
                    {
                        isLogin = false;
                    }


                    if (isLogin)
                    {
                        //Login Success
                        //For Set Authentication in Cookie (Remeber ME Option)
                        SignInRemember(entity.Username, entity.IsRemember);

                        //Set A Unique ID in session
                        Session["UserID"] = userInfo.app_user_id;

                        //Redirect to Index function
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Thông tin đăng nhập không đúng";
                        return View(entity);
                    }
                }
            }
            catch
            {
                throw;
            }

        }
    }
}