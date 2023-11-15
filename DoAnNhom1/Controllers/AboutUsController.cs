using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnNhom1.Filters;
using DoAnNhom1.Models;
using PagedList;

namespace DoAnNhom1.Controllers
{
    [CustomAuthorize(new UserRole[] { UserRole.Admin })]
    public class AboutUsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AboutUs
        public ActionResult Index(string SortOrder, string currentFilter, string SearchString, int? page)
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
            var courses = from c in db.AboutUs
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameUs.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameUs);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameUs);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));

        }
            // GET: AboutUs/Create
            public ActionResult Create()
        {
            AboutUs about = new AboutUs();
            return View(about);
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AboutUs aboutUs)
        {
            try
            {
                if (aboutUs.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(aboutUs.UploadImage.FileName);
                    string extend = Path.GetExtension(aboutUs.UploadImage.FileName);
                    fileName = fileName + extend;
                    aboutUs.ImgUs = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (aboutUs.UploadImage.ContentLength <= 6000000)
                        {
                            db.AboutUs.Add(aboutUs);
                            if (db.SaveChanges() > 0)
                            {
                                aboutUs.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                ViewBag.nofi = "Thêm nhà sáng lập thành công";
                                ModelState.Clear();
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("UploadImage","File Size must be Equal or less than 6mb");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "File không đúng định dạng");
                    }
                }
                else
                {
                    ModelState.AddModelError("UploadImage", "Mời chọn file");
                }
            }
            catch
            {
                return View();
            }
            return View(aboutUs);
        }

        // GET: AboutUs/Edit/5
        public ActionResult Edit(int? id)
        {
            var about = db.AboutUs.Where(s => s.ID == id).FirstOrDefault();
            Session["imgPath"] = about.ImgUs;
            if (about == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy đối tượng
            }

            return View(about);
        }

        // POST: AboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(AboutUs about)
        {
            if (ModelState.IsValid)
            {

                if (about.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(about.UploadImage.FileName);
                    string extend = Path.GetExtension(about.UploadImage.FileName);
                    fileName = fileName + extend;
                    about.ImgUs = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (about.UploadImage.ContentLength <= 6000000)
                        {
                            db.Entry(about).State = EntityState.Modified;

                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                about.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                TempData["nofi"] = "Sửa thành công";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.nofi = "File Size must be Equal or less than 6mb";
                        }
                    }
                    else
                    {
                        ViewBag.nofi = "Invalid File Type";
                    }
                }
                else
                {
                    about.ImgUs = Session["imgPath"].ToString();
                    db.Entry(about).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["nofi"] = "Update Success";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        // GET: AboutUs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutUs aboutUs = db.AboutUs.Find(id);
            if (aboutUs == null)
            {
                return HttpNotFound();
            }
            return View(aboutUs);
        }

        // POST: AboutUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var about = db.AboutUs.Where(s => s.ID == id).FirstOrDefault();
            if (about == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(about.ImgUs);
            db.Entry(about).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(curImg))
                {
                    System.IO.File.Delete(curImg);
                }
                TempData["nofi"] = "Xóa thành công";
                return RedirectToAction("Index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
