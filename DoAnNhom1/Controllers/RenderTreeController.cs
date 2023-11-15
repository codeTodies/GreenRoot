using DoAnNhom1.Models;
using PagedList;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class RenderTreeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RenderTree

        public ActionResult Index(string SortOrder, string currentFilter, string SearchString, int? page,int?id ,int? min = null, int? max = null)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(SortOrder) ? "Name_desc" : "";
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            var courses = from c in db.Trees
                          where c.IDArea==id
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameTree.Contains(SearchString));
            }
            if (min.HasValue && max.HasValue)
            {
                courses = courses.Where(s => s.Price >= min && s.Price <= max);
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameTree);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameTree);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Indexs(string SortOrder, string currentFilter, string SearchString, int? page, int? min = null, int? max = null)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(SortOrder) ? "Name_desc" : "";
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            var courses = from c in db.Trees
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameTree.Contains(SearchString));
            }
            if (min.HasValue && max.HasValue)
            {
                courses = courses.Where(s => s.Price >= min && s.Price <= max);
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameTree);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameTree);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));

        }
    }
}