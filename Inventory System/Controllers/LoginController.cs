using helper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_System.Controllers
{
    public class LoginController : Controller
    {
        private InventoryDB db = new InventoryDB();
        // GET: Login
        public ActionResult Index(string targetUrl)
        {
            ViewBag.targetUrl = targetUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password, string targetUrl)
        {
            ViewBag.targetUrl = targetUrl;
            if (Helper.Login(HttpContext, db, username, password))
            {
                if (String.IsNullOrEmpty(targetUrl))
                    return RedirectToAction("Index", "Home");
                else
                    return Redirect(targetUrl);
            }
            return View();
        }
        public ActionResult Logout(string username, string password)
        {
            Helper.Logout(HttpContext);

            return View("Index");
        }
    }
}