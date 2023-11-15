using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<OrderDetail> orderD = db.OrderDetail.ToList();

            int? totalQuantity = orderD.Sum(od => od.Quantity);

            int finalTotalQuantity = totalQuantity.HasValue ? totalQuantity.Value : 0;

            ViewBag.TotalQuantity = finalTotalQuantity;

            return View();
        }
    }
}