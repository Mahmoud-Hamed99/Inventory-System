﻿using System;
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
    public class ItemCategoriesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 20;
        // GET: ItemCategories
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(db.ItemCategories.OrderBy(a=>a.DateCreated).ToPagedList(pageNumber,pageSize));
        }

        // GET: ItemCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory itemCategory = db.ItemCategories.Find(id);
            if (itemCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemCategory);
        }
        [VerifyUser(Roles = "superadmin,warehouse")]
        // GET: ItemCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouse")]
        public ActionResult Create([Bind(Include = "ItemCategoryId,ItemCategoryName,DateCreated")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                db.ItemCategories.Add(itemCategory);
                db.SaveChanges();
                Helper.AddLog(db, "Created Item Category", itemCategory.ItemCategoryId, "ItemCategory", this);
                return RedirectToAction("Index");
            }

            return View(itemCategory);
        }

        // GET: ItemCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory itemCategory = db.ItemCategories.Find(id);
            if (itemCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemCategory);
        }

        // POST: ItemCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemCategoryId,ItemCategoryName,DateCreated")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemCategory).State = EntityState.Modified;
                db.SaveChanges();
                Helper.AddLog(db, "Edited Item Category", itemCategory.ItemCategoryId, "ItemCategory", this);
                return RedirectToAction("Index");
            }
            return View(itemCategory);
        }

        // GET: ItemCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory itemCategory = db.ItemCategories.Find(id);
            if (itemCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemCategory);
        }

        // POST: ItemCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemCategory itemCategory = db.ItemCategories.Find(id);
            db.ItemCategories.Remove(itemCategory);
            db.SaveChanges();
            Helper.AddLog(db, "Deleted Item Category", id, "ItemCategory", this);
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
