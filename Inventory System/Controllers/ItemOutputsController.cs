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
    public class ItemOutputsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: ItemOutputs
        public ActionResult Index()
        {
            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i => i.Project).Include(i=>i.TechnicalDepartment);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");

            return View(itemOutputs.ToList());
        }

        public ActionResult warehouse()
        {
            return ReturnWarehouse();
        }

        ActionResult ReturnWarehouse()
        {
            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i => i.Project).Include(i => i.TechnicalDepartment).Where(a => a.ItemOutputApproved == false);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");

            return View("warehouse",itemOutputs.ToList());
        }
        [HttpPost]
        public ActionResult Approve(int[] ItemApproved)
        {
            if (ItemApproved != null)
            {
                foreach (var v in ItemApproved)
                {

                    var res = db.ItemOutputs.Find(v);
                    var AvailableQnt = db.Items.Find(v).ItemQuantity;
                    if (res.ItemOutputQuantity <= AvailableQnt)
                    {
                        if (res != null)
                        {
                            res.ItemOutputApproved = true;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.msg = " هذه الكميه غير متاحه";
                    }
                }
            }
            return ReturnWarehouse();
        }
        [HttpPost]
        public ActionResult Index(int? TechnicalDepartmentId, int? ProjectId)
        {
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");

            if (TechnicalDepartmentId != null && ProjectId != null) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
            {
                var items = db.ItemOutputs.Include(i => i.Project)
                      .Include(i => i.TechnicalDepartment)
                      .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.ProjectId == ProjectId);
                return View(items.ToList());
            }
            else
            {
                var items = db.ItemOutputs.Include(i => i.TechnicalDepartment)
                    .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId);
                return View(items.ToList());
            }

        }


        // GET: ItemOutputs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemOutput itemOutput = db.ItemOutputs.Find(id);
            if (itemOutput == null)
            {
                return HttpNotFound();
            }
            return View(itemOutput);
        }

        // GET: ItemOutputs/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            return View();
        }

        // POST: ItemOutputs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemOutputId,ItemOutputQuantity,ItemId,ProjectId,TechnicalDepartmentId,ItemOutputApproved,DateCreated")] ItemOutput itemOutput)
        {
            //ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemOutput.ItemId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemOutput.ProjectId);
            //ViewBag.DepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName", itemOutput.TechnicalDepartmentId);

            if (ModelState.IsValid)
            {
                db.ItemOutputs.Add(itemOutput);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

  
            return View(itemOutput);
        }

        // GET: ItemOutputs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemOutput itemOutput = db.ItemOutputs.Find(id);
            if (itemOutput == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemOutput.ItemId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemOutput.ProjectId);
            ViewBag.DepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName",itemOutput.TechnicalDepartmentId);

            return View(itemOutput);
        }

        // POST: ItemOutputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemOutputId,ItemOutputQuantity,ItemId,ProjectId,DateCreated,TechnicalDepartmentId")] ItemOutput itemOutput)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemOutput).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemOutput.ItemId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", itemOutput.ProjectId);
            ViewBag.DepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName",itemOutput.TechnicalDepartmentId);

            return View(itemOutput);
        }

        // GET: ItemOutputs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemOutput itemOutput = db.ItemOutputs.Include(a=>a.Item).Include(a=>a.Project).Include(a=>a.TechnicalDepartment).Single(a=>a.ItemOutputId==id);
            if (itemOutput == null)
            {
                return HttpNotFound();
            }
            return View(itemOutput);
        }

        // POST: ItemOutputs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemOutput itemOutput = db.ItemOutputs.Find(id);
            db.ItemOutputs.Remove(itemOutput);
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
