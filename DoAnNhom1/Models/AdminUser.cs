using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class AdminUser
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        public string NameUser { get; set; }

        public string RoleUser { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string PasswordUser { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare("PasswordUser", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }
}