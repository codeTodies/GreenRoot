using DoAnNhom1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DoAnNhom1.Controllers
{
    public class SearchpageController : Controller
    {
        // GET: Searchpage
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult SearchP(string SortOrder, string currentFilter, string SearchString, int? page)
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
            ViewBag.CurrentFilters = SearchString;
            var courses = from c in db.Regions
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameRe.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameRe);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameRe);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchArea(string SortOrder, string currentFilters, string SearchStrings, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(SortOrder) ? "Name_desc" : "";
            if (SearchStrings != null)
            {
                page = 1;
            }
            else
            {
                SearchStrings = currentFilters;
            }
            ViewBag.CurrentFilter = SearchStrings;
            var courses = from c in db.Areas
                          select c;
            if (!string.IsNullOrEmpty(SearchStrings))
            {
                courses = courses.Where(c => c.NameArea.Contains(SearchStrings));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameArea);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameArea);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }
    }
}