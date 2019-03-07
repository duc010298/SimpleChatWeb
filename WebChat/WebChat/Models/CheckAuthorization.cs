using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebChat.Models
{
    public class CheckAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["UserID"] == null || !HttpContext.Current.Request.IsAuthenticated)
            {
                //if (filterContext.HttpContext.Request.IsAjaxRequest())
                //{
                //    filterContext.HttpContext.Response.StatusCode = 302; //Found Redirection to another page. Here- login page. Check Layout ajaxError() script.
                //    filterContext.HttpContext.Response.End();
                //}
                //else
                //{
                //    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl);
                //}


                //TODO config here to fix err_too_many_redirects
                filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl);
            }
            else
            {
                //Code HERE for page level authorization
            }
        }
    }
}