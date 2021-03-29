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
    public class SafeSubCategoriesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: SafeSubCategories
        public ActionResult Index()
        {
            var safeSubCategories = db.safeSubCategories.Include(s => s.SafeCategory);
            return View(safeSubCategories.ToList());
        }

        // GET: SafeSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeSubCategory safeSubCategory = db.safeSubCategories.Find(id);
            if (safeSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(safeSubCategory);
        }

        // GET: SafeSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.SafeCategoryId = new SelectList(db.SafeCategories, "SafeCategoryId", "Name");
            return View();
        }

        // POST: SafeSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SafeSubCategoryId,Name,SafeCategoryId")] SafeSubCategory safeSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.safeSubCategories.Add(safeSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SafeCategoryId = new SelectList(db.SafeCategories, "SafeCategoryId", "Name", safeSubCategory.SafeCategoryId);
            return View(safeSubCategory);
        }

        // GET: SafeSubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeSubCategory safeSubCategory = db.safeSubCategories.Find(id);
            if (safeSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.SafeCategoryId = new SelectList(db.SafeCategories, "SafeCategoryId", "Name", safeSubCategory.SafeCategoryId);
            return View(safeSubCategory);
        }

        // POST: SafeSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SafeSubCategoryId,Name,SafeCategoryId")] SafeSubCategory safeSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(safeSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SafeCategoryId = new SelectList(db.SafeCategories, "SafeCategoryId", "Name", safeSubCategory.SafeCategoryId);
            return View(safeSubCategory);
        }

        // GET: SafeSubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafeSubCategory safeSubCategory = db.safeSubCategories.Find(id);
            if (safeSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(safeSubCategory);
        }

        // POST: SafeSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SafeSubCategory safeSubCategory = db.safeSubCategories.Find(id);
            db.safeSubCategories.Remove(safeSubCategory);
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
