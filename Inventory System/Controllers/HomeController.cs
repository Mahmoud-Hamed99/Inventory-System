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
            //    Fullname = "safe",
            //    Password = "123456",
            //    Roles = "safe",
            //    username = "safe"
            //};
            //db.Users.Add(mainUser);
            //db.SaveChanges();
            return View();
        }
    }
}