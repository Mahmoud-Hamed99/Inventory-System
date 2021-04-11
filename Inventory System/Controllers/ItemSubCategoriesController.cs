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
using PagedList;

namespace Inventory_System.Controllers
{
    [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit")]
    public class ItemSubCategoriesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 20;
        // GET: ItemSubCategories
        public ActionResult Index(int? Page)
        {
            var itemSubCategories = db.ItemSubCategories.Include(i => i.ItemCategory);
            int pageNumber = (Page ?? 1);
            return View(itemSubCategories.OrderBy(a=>a.ItemCategoryId).ToPagedList(pageNumber,pageSize));
        }

        // GET: ItemSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemSubCategory);
        }
        [VerifyUser(Roles = "superadmin,warehouse")]
        // GET: ItemSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.ItemCategoryId = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            return View();
        }

        // POST: ItemSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouse")]
        public ActionResult Create([Bind(Include = "ItemSubCategoryId,ItemSubCategoryName,ItemCategoryId")] ItemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.ItemSubCategories.Add(itemSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemCategoryId = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName", itemSubCategory.ItemCategoryId);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemCategoryId = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName", itemSubCategory.ItemCategoryId);
            return View(itemSubCategory);
        }

        // POST: ItemSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemSubCategoryId,ItemSubCategoryName,ItemCategoryId")] ItemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemCategoryId = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName", itemSubCategory.ItemCategoryId);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemSubCategory);
        }

        // POST: ItemSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            db.ItemSubCategories.Remove(itemSubCategory);
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
