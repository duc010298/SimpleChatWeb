using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class RegisterModel
    {
        //Prop for register
        public Boolean IsSignUp { get; set; }
        [Required(ErrorMessage = "Không được để trống tên đăng nhập")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Tên người dùng phải chứa ít nhất 6 kí tự")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Không được để trống mật khẩu")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Mật khẩu phải chứa ít nhất 6 kí tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Không khớp mật khẩu")]
        public string RePassword { get; set; }
        [Required(ErrorMessage = "Không được để trống email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống họ và tên")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Họ và tên không hợp lệ")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Không được để trống ngày sinh")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ"), DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birth { get; set; }
        [Required(ErrorMessage = "Không được để trống giới tính")]
        public string Gender { get; set; }
    }
}