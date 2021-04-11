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
    [VerifyUser(Roles = "superadmin,warehouse,cost,warehouseaudit")]
    public class ItemReturnsController : Controller
    {
        private InventoryDB db = new InventoryDB();
        int pageSize = 20;

        // GET: ItemReturns
        public ActionResult Index(int? Page)
        {
            var itemReturns = db.ItemReturns.Include(i => i.Item).Include(a=>a.Item.ItemInputs).Include(i => i.Project).Include(i=> i.ItemInput.Vendor);
            int pageNumber = (Page ?? 1);
            return View(itemReturns.OrderBy(a=>a.DateCreated).ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        
        public ActionResult Index(int? year, int? month)
        {
            

            
            var itemReturns = db.ItemReturns.Include(i => i.Item).Include(a => a.Item.ItemInputs).Include(i => i.Project).Include(i => i.ItemInput.Vendor);
            if(year!=null)
            {
                itemReturns = itemReturns.Where(a => a.DateCreated.Year == year);
            }
            if (month != null)
            {
                itemReturns = itemReturns.Where(a => a.DateCreated.Month == month);
            }
            return View(itemReturns.OrderBy(a => a.DateCreated).ToPagedList(1, 1000000000));
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
            ViewBag.projectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            ViewBag.depId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            return View();
        }

        public JsonResult GetMembers(int id,int projectId)
        {
            //ViewBag.ItemId = new SelectList(db.ItemOutputs.Include(a => a.Item.ItemName).Where(a => a.TechnicalDepartmentId == depId).ToList(), "ItemId", "ItemName");

            db.Configuration.ProxyCreationEnabled = false;
            var itemOutputList = db.ItemOutputs.Include(a=>a.Item).Where(x => x.TechnicalDepartmentId == id
            && x.ItemOutputApproved == true && x.ProjectId == projectId).ToList();
            List<Item> itemsList = new List<Item>();
            
            foreach (var i in itemOutputList)
            {
                itemsList.Add(new Item()
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item.ItemName
                });
            }
            var ret = Json(itemsList, JsonRequestBehavior.AllowGet);
            return ret;
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

                var x = db.Items.Find(itemReturn.ItemId);
                x.ItemQuantityAdded += itemReturn.ItemQuantity;
                
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
