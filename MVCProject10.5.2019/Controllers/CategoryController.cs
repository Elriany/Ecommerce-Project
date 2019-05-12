using MVCProject10._5._2019.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject10._5._2019.Controllers
{
    public class CategoryController : Controller
    {
        private myDbContext context;
        public CategoryController()
        {
            context = new myDbContext();
        }
        // GET: Category
        public ActionResult Index()
        {
            if (Session["Admin_ID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(context.categories);
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var cat = context.categories.Find(id);
            ViewBag.adminName = context.admin_tbl.Where(x => x.id == cat.admin_ID).Select(x => x.username).First();
            return PartialView(cat);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(category cat,HttpPostedFileBase img)
        {
            string path;
            if (ModelState.IsValid)
            {
                path = Path.Combine(Server.MapPath("~/Content/Uploads/Category"),Path.GetFileName(img.FileName));
                img.SaveAs(path);
                path = "~/Content/Uploads/Category/"+ Path.GetFileName(img.FileName);
                cat.cat_image = path;
                cat.admin_ID = 1; //ToDo
                cat.status = 1;
                context.categories.Add(cat);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        // GET: Category/Edit/id
        public ActionResult Edit(int id)
        {
            return View(context.categories.Find(id));
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(category cat,HttpPostedFileBase imgfile)
        {
            try
            {
                // TODO: Add update logic here
                var oldCategory = context.categories.Find(cat.cat_id);
                var oldimg = context.categories.Where(c => c.cat_id == cat.cat_id).Select(c=>c.cat_image).First();
                if (imgfile == null)
                {
                    oldCategory.cat_id = cat.cat_id;
                    oldCategory.cat_image = oldimg;
                    oldCategory.cat_name = cat.cat_name;
                    oldCategory.status = cat.status;
                    oldCategory.admin_ID = 1; //ToDo
                    context.SaveChanges();
                }
                else
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Uploads/Category"), Path.GetFileName(imgfile.FileName));
                    imgfile.SaveAs(path);
                    path = "~/Content/Uploads/Category/" + Path.GetFileName(imgfile.FileName);
                    oldCategory.cat_id = cat.cat_id;
                    oldCategory.cat_image = path;
                    oldCategory.cat_name = cat.cat_name;
                    oldCategory.status = cat.status;
                    oldCategory.admin_ID = 1; //ToDo
                    context.SaveChanges();
                    DeleteimgFromFolder(oldimg);//Delete Old Image
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(cat);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var img = context.categories.Where(c => c.cat_id == id).Select(c => c.cat_image).First(); //from project only
            var CatiDelete = context.categories.Find(id);
            context.categories.Remove(CatiDelete);
            context.SaveChanges();
            DeleteimgFromFolder(img);
            return RedirectToAction("Index");
            
        }

        private bool DeleteimgFromFolder(string path)
        {
            string fpath = Server.MapPath(path); // from Location In Hard from project
            if (!System.IO.File.Exists(fpath))
            {
                return false;
            }
            try
            {
                System.IO.File.Delete(fpath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

    }
}
