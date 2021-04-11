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
    public class DemandItemsController : Controller
    {
        int pageSize = 20;
        private InventoryDB db = new InventoryDB();

        [VerifyUser(Roles = "demandplanning")]
        // GET: DemandItems
        public ActionResult Index(int? page)
        {
            var demandItems = db.DemandItems
                .Include(d => d.ItemOutput)
                .Include(a=>a.ItemOutput.Item)
                .Include(a=>a.ItemOutput.Project)
                .Where(a => a.DemandItemApproval == false);
            int pageNumber = (page ?? 1);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            return View(demandItems.OrderBy(a=>a.DemandItemPriority).ToPagedList(1, 1000000000));
        }
        [HttpPost]
        public ActionResult Index(int ProjectId)
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            var demandItems = db.DemandItems
                .Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project)
                .Where(a => a.DemandItemApproval == false && a.ItemOutput.ProjectId == ProjectId);

            return View(demandItems.OrderBy(a => a.DemandItemPriority).ToPagedList(1, 1000000000));
        }
        [VerifyUser(Roles = "demandplanning")]
        public ActionResult DemandHistory(int? page)
        {

            var demandItems = db.DemandItems.Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a => a.DemandItemId).ToPagedList(pageNumber, pageSize));
        }
        [VerifyUser(Roles = "purchasing")]
        public ActionResult PurchasingApproval(int? page)
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            var demandItems = db.DemandItems.Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project)
                .Where(a => a.DemandItemApproval == true && a.PurchasingApproval==false);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a=>a.DemandItemId).ToPagedList(1, 1000000000));
        }
        [HttpPost]
        public ActionResult PurchasingApproval(int? page, int ProjectId)
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            var demandItems = db.DemandItems.Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project)
                .Where(a => a.DemandItemApproval == true && a.PurchasingApproval == false && a.ItemOutput.ProjectId == ProjectId);
            int pageNumber = (page ?? 1);
            return View(demandItems.OrderBy(a => a.DemandItemId).ToPagedList(1, 1000000000));
        }

        [VerifyUser(Roles = "purchasing")]
        public ActionResult PurchasingHistory(int? page)
        {
            var demandItems = db.DemandItems.Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project)
                .Where(a => a.DemandItemApproval == true);
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

        //// POST: DemandItems/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DemandItemId,ItemId,DemandItemQuantity,DemandItemPriority,DemandItemApproval")] DemandItem demandItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DemandItems.Add(demandItem);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemOutputId);
        //    return View(demandItem);
        //}

        // GET: DemandItems/Edit/5
        [VerifyUser(Roles = "demandplanning")]
        public ActionResult Edit(int? id)
        {
            if (DateTime.Now.Hour >0 && DateTime.Now.Hour < 24)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DemandItem demandItem = db.DemandItems.Include(d => d.ItemOutput)
                .Include(a => a.ItemOutput.Item)
                .Include(a => a.ItemOutput.Project).Single(a=>a.DemandItemId == id);
                if (demandItem == null)
                {
                    return HttpNotFound();
                }
                
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
        public ActionResult Edit([Bind(Include = "DemandItemId,ItemOutputId,DemandItemQuantity,DemandItemPriority,DemandItemApproval")] DemandItem demandItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demandItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(demandItem);
        }

        ////////////////////////////////////////////////////////////////////////////////
        [VerifyUser(Roles = "purchasing")]
        public ActionResult PurchasingEdit(int? id)
        {
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DemandItem demandItem = db.DemandItems.Include(a=>a.ItemOutput)
                .Include(a=>a.ItemOutput.Item)
                .Include(a=>a.ItemOutput.Project)
                .Single(a=>a.DemandItemId == id);
                if (demandItem == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
                return View(demandItem);
           

        }

        // POST: DemandItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "purchasing")]
        public ActionResult PurchasingEdit([Bind(Include = "DemandItemId,ItemOutputId,DemandItemQuantity,DemandItemPriority,DemandItemApproval,PurchasingApproval")] DemandItem demandItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demandItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PurchasingApproval");
            }
            //ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", demandItem.ItemId);
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
