using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                adminUser.RoleUser = "User";
                db.AdminUsers.Add(adminUser);
                if (db.SaveChanges() > 0)
                {
                    ModelState.Clear();
                }
                return RedirectToAction("Index");
            }

            return View(adminUser);
        }
        [HttpPost]
        public ActionResult Login(AdminUser user)
        {
            var check = db.AdminUsers.Where(s => s.NameUser == user.NameUser && s.PasswordUser == user.PasswordUser && (s.RoleUser == "User" || s.RoleUser == "Admin")).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Tên đăng nhập hoặc tài khoản không đúng";
                return View("Index");

            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = user.NameUser;
                Session["UserRole"] = "User";
                Session["PasswordUser"] = user.PasswordUser;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult LoginAdmin(AdminUser user)
        {
            var check = db.AdminUsers.Where(s => s.NameUser == user.NameUser && s.PasswordUser == user.PasswordUser && s.RoleUser == "Admin").FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfos = "Sai thông tin đăng nhập hoặc bạn không có quyền vào trang này";
                return View("Admin");

            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = user.NameUser;
                Session["UserRole"] = "Admin";
                Session["PasswordUser"] = user.PasswordUser;
                return RedirectToAction("Index", "AdminUsers");
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}