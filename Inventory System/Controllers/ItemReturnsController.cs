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
    public class ItemReturnsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: ItemReturns
        public ActionResult Index()
        {
            var itemReturns = db.ItemReturns.Include(i => i.Item).Include(i => i.Project);
            return View(itemReturns.ToList());
        }

        // GET: ItemReturns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemReturn itemReturn = db.ItemReturns.Find(id);
            if (itemReturn == null)
            {
                return HttpNotFound();
            }
            return View(itemReturn);
        }

        // GET: ItemReturns/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            ViewBag.projectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: ItemReturns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemReturnId,ItemId,ItemQuantity,projectId,DateCreated")] ItemReturn itemReturn)
        {
            if (ModelState.IsValid)
            {
                db.ItemReturns.Add(itemReturn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemReturn.ItemId);
            ViewBag.projectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemReturn.projectId);
            return View(itemReturn);
        }

        // GET: ItemReturns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemReturn itemReturn = db.ItemReturns.Find(id);
            if (itemReturn == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemReturn.ItemId);
            ViewBag.projectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemReturn.projectId);
            return View(itemReturn);
        }

        // POST: ItemReturns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemReturnId,ItemId,ItemQuantity,projectId,DateCreated")] ItemReturn itemReturn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemReturn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemReturn.ItemId);
            ViewBag.projectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemReturn.projectId);
            return View(itemReturn);
        }

        // GET: ItemReturns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemReturn itemReturn = db.ItemReturns.Find(id);
            if (itemReturn == null)
            {
                return HttpNotFound();
            }
            return View(itemReturn);
        }

        // POST: ItemReturns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemReturn itemReturn = db.ItemReturns.Find(id);
            db.ItemReturns.Remove(itemReturn);
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
