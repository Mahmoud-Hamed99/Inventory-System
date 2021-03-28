using helper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_System.Controllers
{
    public class HomeController : Controller
    {
        //[VerifyUser(Roles ="superadmin")]
        // GET: Home
        public ActionResult Index()
        {
            //InventoryDB db = new InventoryDB();
            //Models.User mainUser = new Models.User()
            //{
            //    Fullname = "cost",
            //    Password = "123456",
            //    Roles = "cost",
            //    username = "cost"
            //};
            //db.Users.Add(mainUser);
            //db.SaveChanges();
            return View();
        }
    }
}