using MVCProject10._5._2019.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject10._5._2019.Content
{
    public class ProductController : Controller
    {
        private myDbContext context;
        public ProductController()
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
                return View(context.products);
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var prod = context.products.Find(id);
            ViewBag.catName = context.categories.Where(x => x.cat_id == prod.prod_id).Select(x => x.cat_name).First();
            return PartialView(prod);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ViewBag.category = context.categories.ToList();
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(product prod, HttpPostedFileBase img)
        {
            string path;
            if (ModelState.IsValid)
            {
                path = Path.Combine(Server.MapPath("~/Content/Uploads/Products"), Path.GetFileName(img.FileName));
                img.SaveAs(path);
                path = "~/Content/Uploads/Products/" + Path.GetFileName(img.FileName);
                prod.prod_image = path;
                context.products.Add(prod);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prod);
        }

        // GET: Category/Edit/id
        public ActionResult Edit(int id)
        {
            ViewBag.cats = new SelectList(context.categories, "cat_id", "cat_name");
            return View(context.products.Find(id));
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(product prod, HttpPostedFileBase imgfile)
        {
            try
            {
                // TODO: Add update logic here
                var oldProduct = context.products.Find(prod.prod_id);
                var oldimg = context.products.Where(c => c.prod_id == prod.prod_id).Select(c => c.prod_image).First();
                if (imgfile == null)
                {
                    oldProduct.prod_id = prod.prod_id;
                    oldProduct.prod_image = oldimg;
                    oldProduct.prod_name = prod.prod_name;
                    oldProduct.stock = prod.stock;
                    oldProduct.price = prod.price;
                    oldProduct.Cat_ID = prod.Cat_ID;
                    oldProduct.prod_description = prod.prod_description;
                    context.SaveChanges();
                }
                else
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Uploads/Products"), Path.GetFileName(imgfile.FileName));
                    imgfile.SaveAs(path);
                    path = "~/Content/Uploads/Products/" + Path.GetFileName(imgfile.FileName);
                    oldProduct.prod_id = prod.prod_id;
                    oldProduct.prod_image = path;
                    oldProduct.prod_name = prod.prod_name;
                    oldProduct.stock = prod.stock;
                    oldProduct.price = prod.price;
                    oldProduct.Cat_ID = prod.Cat_ID;
                    oldProduct.prod_description = prod.prod_description;
                    context.SaveChanges();
                    DeleteimgFromFolder(oldimg);//Delete Old Image
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(prod);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var img = context.products.Where(c => c.prod_id == id).Select(c => c.prod_image).First(); //from project only
            var ProdiDelete = context.products.Find(id);
            context.products.Remove(ProdiDelete);
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