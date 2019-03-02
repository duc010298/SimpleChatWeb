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
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public ActionResult Form()
        {
            var authenticationInfo = new AuthenticationModel();
            try
            {
                // If authenticated, go to index
                if (Request.IsAuthenticated)
                {
                    return RedirectToAction("Index", "WebChat");
                }
                return View(authenticationInfo);
            }
            catch
            {
                throw;
            }
        }

        [NonAction]
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthenticationModel entity)
        {
            string encrypt_password = string.Empty;
            try
            {
                using (var db = new WebChatEntities())
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                    {
                        return View("Form", entity);
                    }

                    //Retrive Stored Encrypt Password From Database According To Username (one unique field)
                    var userInfo = db.app_user.Where(s => s.username == entity.Login.Username.Trim()).FirstOrDefault();

                    //Verify password
                    bool isLogin;
                    if (userInfo != null)
                    {
                        encrypt_password = userInfo.encrypted_password;
                        isLogin = BCrypt.Net.BCrypt.Verify(entity.Login.Password, encrypt_password);
                    }
                    else
                    {
                        isLogin = false;
                    }


                    if (isLogin)
                    {
                        //Login Success
                        //For Set Authentication in Cookie (Remeber ME Option)
                        SignInRemember(entity.Login.Username, entity.Login.IsRemember);

                        //Set A Unique ID in session
                        Session["UserID"] = userInfo.app_user_id;

                        //Redirect to Index
                        return RedirectToAction("Index", "WebChat");
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Thông tin đăng nhập không đúng";
                        return View("Form", entity);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        //GET: Logout
        [HttpGet]
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

                //Redirect to index
                return RedirectToAction("Index", "WebChat");
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AuthenticationModel entity)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
            {
                return View("Form", entity);
            }
            return View("Form", entity);
        }
    }
}