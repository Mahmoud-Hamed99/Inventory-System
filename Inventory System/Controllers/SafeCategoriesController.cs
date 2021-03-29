using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventory_System;
using Inventory_System.Models;

namespace Inventory_System.Controllers
{
    public class SafeCategoriesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: SafeCategories
        public ActionResult Index()
        {
            return View(db.SafeCategories.ToList());
        }

        // GET: SafeCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeCategory safeCategory = db.SafeCategories.Find(id);
            if (safeCategory == null)
            {
                return HttpNotFound();
            }
            return View(safeCategory);
        }

        // GET: SafeCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SafeCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SafeCategoryId,Name")] SafeCategory safeCategory)
        {
            if (ModelState.IsValid)
            {
                db.SafeCategories.Add(safeCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(safeCategory);
        }

        // GET: SafeCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeCategory safeCategory = db.SafeCategories.Find(id);
            if (safeCategory == null)
            {
                return HttpNotFound();
            }
            return View(safeCategory);
        }

        // POST: SafeCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SafeCategoryId,Name")] SafeCategory safeCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(safeCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(safeCategory);
        }

        // GET: SafeCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeCategory safeCategory = db.SafeCategories.Find(id);
            if (safeCategory == null)
            {
                return HttpNotFound();
            }
            return View(safeCategory);
        }

        // POST: SafeCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SafeCategory safeCategory = db.SafeCategories.Find(id);
            db.SafeCategories.Remove(safeCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
