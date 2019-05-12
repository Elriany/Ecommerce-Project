using MVCProject10._5._2019.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject10._5._2019.Controllers
{
    public class CustomerController : Controller
    {
        private myDbContext context;
        public CustomerController()
        {
            context = new myDbContext();
        }
        // GET: Category
        public ActionResult Index()
        {
            if(Session["Admin_ID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(context.customers);
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            return View(context.customers.Find(id));
        }

        // GET: Category/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Category/Create
        //[HttpPost]
        //public ActionResult Create(customer cust)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.customers.Add(cust);
        //        context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cust);
        //}

        // GET: Category/Edit/id
        public ActionResult Edit(int id)
        {
            return View(context.customers.Find(id));
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(customer cust)
        {
            var oldCust = context.customers.Find(cust.cust_id); 
            try
            {
                oldCust.cust_name = cust.cust_name;
                oldCust.cus_email = cust.cus_email;
                oldCust.cust_phone = cust.cust_phone;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(cust);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var CustiDelete = context.customers.Find(id);
            context.customers.Remove(CustiDelete);
            context.SaveChanges();
            return RedirectToAction("Index");

        }
        
    }
}