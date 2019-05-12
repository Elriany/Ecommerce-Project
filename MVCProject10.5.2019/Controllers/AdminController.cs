using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject10._5._2019.Models;

namespace MVCProject10._5._2019.Controllers
{
    public class AdminController : Controller
    {
        private myDbContext context;
        public AdminController()
        {
            context = new myDbContext();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["Admin_ID"] != null)
            {
                return RedirectToAction("index","Customer");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(admin_tbl admin)
        {
            var Auth = context.admin_tbl.Where(ad => ad.username == admin.username && ad.password == admin.password).FirstOrDefault();
            if (Auth ==null)
            {
                @ViewBag.error = "Please Enter Correct Username And Password";
                return View("Login");
            }
            else
            {
                Session["Admin_ID"] = Auth.id;
                return RedirectToAction("Index","Customer");
            }
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(admin_tbl admin)
        {
            context.admin_tbl.Add(admin);
            context.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Index()
        {
            if(Session["Admin_ID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View(context.admin_tbl);
            }
        }
    }
}