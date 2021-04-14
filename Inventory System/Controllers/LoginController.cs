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
            if (Helper.Login(HttpContext, db, username, password,this))
            {
                Helper.AddLog(db, "Logged in", 0, null, this);
                //if (String.IsNullOrEmpty(targetUrl))
                //{ 
                    switch (((Models.User)ViewBag.mainUser).Roles)
                    {
                        case "warehouse":
                            return RedirectToAction("Index", "Items");
                            //break;
                        case "warehouseaudit":
                            return RedirectToAction("Index", "ItemInputs", new { acc = true });
                            //break;
                        case "purchasing":
                            return RedirectToAction("PurchasingApproval", "DemandItems");
                            //break;
                        case "generalaccountant":
                            return RedirectToAction("Index", "BankAccounts");
                            //break;
                        case "demandplanning":
                            return RedirectToAction("Index", "DemandItems");
                            //break;
                        case "projectplanning":
                            return RedirectToAction("Index", "Projects");
                        case "cost":
                            return RedirectToAction("Index", "Items");
                        case "safe":
                             return RedirectToAction("Index", "Safes");
                        //break;
                        default:
                            return RedirectToAction("Index", "Items");
                            //break;
                    }
                //}
                //else
                //    return Redirect(targetUrl);
            }
            return View();
        }
        public ActionResult Logout(string username, string password)
        {
            Helper.AddLog(db, "Logged out", 0, null, this);
            Helper.Logout(HttpContext);

            return View("Index");
        }
        [VerifyUser]
        public ActionResult Prof()
        {
            return View();
        }
        [VerifyUser]
        [HttpPost]
        public ActionResult Prof(string password)
        {
            try
            {
                var u = ((Inventory_System.Models.User)ViewBag.mainUser);
                db.Users.Find(u.Id).Password = password;
                db.SaveChanges();
                Helper.Login(HttpContext, db, u.username, password, this);
                switch (u.Roles)
                {
                    case "warehouse":
                        return RedirectToAction("Index", "Items");
                    //break;
                    case "warehouseaudit":
                        return RedirectToAction("Index", "ItemInputs", new { acc = true });
                    //break;
                    case "purchasing":
                        return RedirectToAction("PurchasingApproval", "DemandItems");
                    //break;
                    case "generalaccountant":
                        return RedirectToAction("Index", "BankAccounts");
                    //break;
                    case "demandplanning":
                        return RedirectToAction("Index", "DemandItems");
                    //break;
                    case "projectplanning":
                        return RedirectToAction("Index", "Projects");
                    case "cost":
                        return RedirectToAction("Index", "Items");
                    case "safe":
                        return RedirectToAction("Index", "Safes");
                    //break;
                    default:
                        return RedirectToAction("Index", "Items");
                        //break;
                }
            }
            catch
            {

            }
            
            return View();
        }
    }
}