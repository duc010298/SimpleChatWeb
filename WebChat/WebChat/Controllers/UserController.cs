using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [CheckAuthorization]
        public ActionResult Info()
        {
            Guid userID = (Guid)Session["UserID"];
            Info info = new Info();
            using (var db = new WebChatEntities())
            {
                var userInfo = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                ViewBag.Avatar = userInfo.avatar;
                ViewBag.Fullname = userInfo.fullname;
                info.Id = userInfo.app_user_id;
                info.Avatar = userInfo.avatar;
                info.Username = userInfo.app_user.username;
                info.LastChangePassword = userInfo.last_change_password;
                info.Fullname = userInfo.fullname;
                info.Birth = userInfo.birth;
                info.Gender = userInfo.gender;
                info.City = userInfo.city;
                info.ShortDescription = userInfo.customer_description;
            }
            ViewBag.View = "info";
            return View(info);
        }

        [HttpPost]
        [CheckAuthorization]
        public string ChangeAvatar(HttpPostedFileBase file)
        {
            Guid userID = (Guid)Session["UserID"];
            if (file != null && file.ContentLength > 0 && IsImage(file))
            {
                //TODO convert to png type, resize image and save it to database
                string directory = Server.MapPath("~/UploadedFiles/Avatar");
                string imageName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                string path = System.IO.Path.Combine(directory, imageName);

                using (Image image = Image.FromStream(file.InputStream, true, false))
                {
                    try
                    {
                        float thumbWidth = 500;
                        float thumbHeight = 500;
                        if (image.Width > image.Height)
                        {
                            thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                        }
                        else
                        {
                            thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                        }

                        int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                        int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                        var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                        thumbnailGraph.DrawImage(image, imageRectangle);
                        var ms = new MemoryStream();
                        thumbnailBitmap.Save(path, ImageFormat.Png);
                        ms.Position = 0;
                        GC.Collect();
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                using (var db = new WebChatEntities())
                {
                    var user = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                    if (user.avatar != null)
                    {
                        string filePath = System.IO.Path.Combine(directory, user.avatar);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    user.avatar = imageName;
                    db.SaveChanges();
                }
                return "Đổi ảnh đại diện thành công";
            }
            else
            {
                return "Xin hãy chọn file ảnh";
            }
        }

        [NonAction]
        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }
            string[] formats = new string[] { ".jpg", ".png", ".bmp" };
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPost]
        [CheckAuthorization]
        public string ChangePassword(string oldPassword, string newPassword)
        {
            Guid userID = (Guid)Session["UserID"];
            using (var db = new WebChatEntities())
            {
                var user = db.app_user.Where(s => s.app_user_id == userID).FirstOrDefault();
                if (BCrypt.Net.BCrypt.Verify(oldPassword.Trim().ToLower(), user.encrypted_password)) {
                    string encrypt_password = BCrypt.Net.BCrypt.HashPassword(newPassword.Trim().ToLower());
                    user.encrypted_password = encrypt_password;
                    var customer = db.customers.Where(s => s.app_user_id == userID).FirstOrDefault();
                    customer.last_change_password = DateTime.Now;
                    db.SaveChanges();
                    return "Đổi mật khẩu thành công";
                }
                else
                {
                    return "Mật khẩu cũ không đúng";
                }
            }
        }
    }
}