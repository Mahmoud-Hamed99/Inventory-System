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
        public ActionResult Index(string startDate,string endDate)
        {
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            var safeSubCategories = db.safeSubCategories.Include(s => s.SafeCategory).Include(s=>s.Saves);
            List<CatsSums> catSumsList = new List<CatsSums>();
            foreach(var v in safeSubCategories)
            {
                catSumsList.Add(new CatsSums()
                {
                    SafeSubCategory = v,
                    FinanceStatement = new helper.Classes.FinanceStatement()
                });
                catSumsList.Last().FinanceStatement =
                    helper.Classes.Helper.DoCalculation(
                        v.Saves.AsQueryable(), startDate, endDate);
            }
            ViewBag.Statement = catSumsList;
            return View(safeSubCategories.ToList());
        }
        public class CatsSums
        {
            public SafeSubCategory SafeSubCategory { get; set; }
            public helper.Classes.FinanceStatement FinanceStatement { get; set; }
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
