using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using helper.Classes;
using Inventory_System;
using Inventory_System.Models;

namespace Inventory_System.Controllers
{
    public class UserLogsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: UserLogs
        [VerifyUser(Roles ="superadmin")]
        public ActionResult Index()
        {
            return View(db.UserLogs.Take(1000).ToList());
        }

        // GET: UserLogs/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserLog userLog = db.UserLogs.Find(id);
        //    if (userLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userLog);
        //}

        //// GET: UserLogs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserLogs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,DateCreated,Action,UserId,Username,TableName,RowId")] UserLog userLog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UserLogs.Add(userLog);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(userLog);
        //}

        //// GET: UserLogs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserLog userLog = db.UserLogs.Find(id);
        //    if (userLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userLog);
        //}

        //// POST: UserLogs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,DateCreated,Action,UserId,Username,TableName,RowId")] UserLog userLog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(userLog).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userLog);
        //}

        //// GET: UserLogs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserLog userLog = db.UserLogs.Find(id);
        //    if (userLog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userLog);
        //}

        //// POST: UserLogs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserLog userLog = db.UserLogs.Find(id);
        //    db.UserLogs.Remove(userLog);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
