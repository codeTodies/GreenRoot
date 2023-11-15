using DoAnNhom1.Models;
using DoAnNhom1.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var aboutUsData = db.AboutUs.ToList();
            var regionsData = db.Regions.ToList();

            var combinedViewModel = new CombineViewModel
            {
                AboutUsData = aboutUsData,
                RegionsData = regionsData
            };

            return View(combinedViewModel);
        }
        public ActionResult StatisticTree()
        {
            List<OrderDetail> orderD = db.OrderDetail.ToList();
            List<Tree> proList = db.Trees.ToList();
            var query = from od in orderD
                        join p in proList on od.IDProduct equals p.TreeID into tbl
                        group od by new
                        {
                            idPro = od.IDProduct,
                            nameArea=od.Tree.Area.NameArea,
                            namePro = od.Tree.NameTree,
                            imagePro = od.Tree.ImageTree,
                            price = od.Tree.Price
                        } into gr
                        orderby gr.Sum(s => s.Quantity) descending
                        select new QuantityTree
                        {
                            TreeID = gr.Key.idPro,
                            NameTree = gr.Key.namePro,
                            NameArea = gr.Key.nameArea,
                            ImageTree = gr.Key.imagePro,
                            Price = gr.Key.price,
                            Sum_Quantity = gr.Sum(s => s.Quantity)
                        };
            return View(query.ToList());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Donate()
        {
            return View();
        }
    }
}