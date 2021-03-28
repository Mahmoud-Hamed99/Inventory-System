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
using PagedList;


namespace Inventory_System.Controllers
{
    public class DemandItemsController : Controller
    {
        int pageSize = 20;
        private InventoryDB db = new InventoryDB();

        // GET: DemandItems
        public ActionResult Index(int? page)
        {
            var demandItems = db.DemandItems.Include(d => d.Item).Where(a => a.DemandItemApproval == false);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a=>a.DemandItemId).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult DemandHistory(int? page)
        {
            var demandItems = db.DemandItems.Include(d => d.Item);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a => a.DemandItemId).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult PurchasingApproval(int? page)
        {
            var demandItems = db.DemandItems.Include(d => d.Item).Where(a => a.DemandItemApproval == true && a.PurchasingApproval==false);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a=>a.DemandItemId).ToPagedList(pageNumber,pageSize));
        }

        
        public ActionResult PurchasingHistory(int? page)
        {
            var demandItems = db.DemandItems.Include(d => d.Item).Where(a => a.DemandItemApproval == true);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a => a.DemandItemId).ToPagedList(pageNumber, pageSize));
        }

        // GET: DemandItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DemandItem demandItem = db.DemandItems.Find(id);
            if (demandItem == null)
            {
                return HttpNotFound();
            }
            return View(demandItem);
        }

        // GET: DemandItems/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            return View();
        }

        // POST: DemandItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DemandItemId,ItemId,DemandItemQuantity,DemandItemPriority,DemandItemApproval")] DemandItem demandItem)
        {
            if (ModelState.IsValid)
            {
                db.DemandItems.Add(demandItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
            return View(demandItem);
        }

        // GET: DemandItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (DateTime.Now.Hour >0 && DateTime.Now.Hour < 24)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DemandItem demandItem = db.DemandItems.Find(id);
                if (demandItem == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
                return View(demandItem);
            }

            else
            {
                ViewBag.msg2 = "لا يمكنك التعديل الان";
                return RedirectToAction("Index");
            }
       
    }

        // POST: DemandItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DemandItemId,ItemId,DemandItemQuantity,DemandItemPriority,DemandItemApproval")] DemandItem demandItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demandItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
            return View(demandItem);
        }

        ////////////////////////////////////////////////////////////////////////////////
        public ActionResult PurchasingEdit(int? id)
        {
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DemandItem demandItem = db.DemandItems.Find(id);
                if (demandItem == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
                return View(demandItem);
           

        }

        // POST: DemandItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchasingEdit([Bind(Include = "DemandItemId,ItemId,DemandItemQuantity,DemandItemApproval,PurchasingApproval")] DemandItem demandItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demandItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PurchasingApproval");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
            return View(demandItem);
        }
        //*********************************************************************************


        // GET: DemandItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DemandItem demandItem = db.DemandItems.Find(id);
            if (demandItem == null)
            {
                return HttpNotFound();
            }
            return View(demandItem);
        }

        // POST: DemandItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DemandItem demandItem = db.DemandItems.Find(id);
            db.DemandItems.Remove(demandItem);
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
