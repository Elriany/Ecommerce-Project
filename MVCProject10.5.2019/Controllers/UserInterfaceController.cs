using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject10._5._2019.Models;
namespace MVCProject10._5._2019.Controllers
{
    public class UserInterfaceController : Controller
    {
        private myDbContext context;
        public UserInterfaceController()
        {
            context = new myDbContext();
        }
        public ActionResult StartPoint()
        {
            return View(context.products);
        }
        public ActionResult Index()
        {
            if(Session["Customer_Id"] !=null)
            {
                return View(context.products);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(customer cust)
        {
            if (ModelState.IsValid)
            {
                context.customers.Add(cust);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(cust);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(customer cust)
        {
            var check = context.customers.Where(c => c.cus_email == cust.cus_email && c.cust_name == cust.cust_name).FirstOrDefault();
            if(check != null)
            {
                Session["Customer_Id"] = cust.cust_id;
                return RedirectToAction("Index");
            }
            else
            {
                @ViewBag.error = "Enter Username and Email Valid";
                return View("Login");
            }

        }
        public ActionResult AddInCart(int id)
        {            
            if(Session["Cart"] == null)
            {
                List<myCart> cart = new List<myCart>();
                cart.Add(new myCart(context.products.Find(id), 1));
                Session["Cart"] = cart;
            }
            else
            {
                List<myCart> cart = (List<myCart>) Session["Cart"];
                int numOfProduct = isFound(id);
                if(numOfProduct == -1)
                {
                    cart.Add(new myCart(context.products.Find(id), 1));
                }
                else
                {
                    cart[numOfProduct].quantity++;
                }
                Session["Cart"] = cart;
            }
            return View();
        }
        private int isFound(int id)
        {
            List<myCart> cart = (List<myCart>)Session["Cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].prod.prod_id == id)
                    return i;
            }
            return -1;
        }
        public ActionResult Details(int id)
        {
            return View(context.products.Find(id));
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("StartPoint");
        }
        [HttpGet]
        public ActionResult DeleteFromCart(int id=0)
        {
            int index = isFound(id);
            List<myCart> cart = (List<myCart>)Session["Cart"];
            cart.RemoveAt(id);
            Session["Cart"] = cart;
            return RedirectToAction("AddInCart");
        }
    }
}