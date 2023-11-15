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
    public class InfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Infoes
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
            var courses = from c in db.Infos
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.Title.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.Title);
                    break;
                default:
                    courses = courses.OrderBy(c => c.Title);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }



        // GET: Infoes/Create
        public ActionResult Create()
        {
            Info info = new Info();
            return View(info);
        }

        // POST: Infoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Info info)
        {
            try
            {
                if (info.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(info.UploadImage.FileName);
                    string extend = Path.GetExtension(info.UploadImage.FileName);
                    fileName = fileName + extend;
                    info.ImgInfo = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (info.UploadImage.ContentLength <= 6000000)
                        {
                            db.Infos.Add(info);
                            if (db.SaveChanges() > 0)
                            {
                                info.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                ViewBag.nofi = "Thêm vùng thành công";
                                ModelState.Clear();
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
            return View(info);
        }

        // GET: Infoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Info info = db.Infos.Find(id);
            Session["imgPath"] = info.ImgInfo;
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        // POST: Infoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Info info)
        {
            if (ModelState.IsValid)
            {

                if (info.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(info.UploadImage.FileName);
                    string extend = Path.GetExtension(info.UploadImage.FileName);
                    fileName = fileName + extend;
                    info.ImgInfo = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (info.UploadImage.ContentLength <= 6000000)
                        {
                            db.Entry(info).State = EntityState.Modified;

                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                info.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                TempData["nofi"] = "Cập nhật thành công";
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
                    info.ImgInfo = Session["imgPath"].ToString();
                    db.Entry(info).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["nofi"] = "Cập nhật thành công";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        // GET: Infoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Info info = db.Infos.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        // POST: Infoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var info = db.Infos.Where(s => s.ID== id).FirstOrDefault();
            if (info == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(info.ImgInfo);
            db.Entry(info).State = EntityState.Deleted;
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
