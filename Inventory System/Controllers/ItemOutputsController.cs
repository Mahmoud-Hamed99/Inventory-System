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
    public class ItemOutputsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 2;
        // GET: ItemOutputs
        [VerifyUser(Roles ="superadmin,warehouse")]
        public ActionResult Index(int? Page , int? TechnicalDepartmentId, int? ProjectId)
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            ViewBag.MainRole = user.Roles;
            int pageNumber = (Page ?? 1);

            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i => i.Project).Include(i => i.TechnicalDepartment);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            
            //---------------------------------------------------
            
            if (TechnicalDepartmentId != null && ProjectId != null) 
            {
                var items = db.ItemOutputs.Include(i => i.Project)
                      .Include(i => i.TechnicalDepartment)
                      .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.ProjectId == ProjectId);

                ViewBag.ProjectIdv = ProjectId;
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
                return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }
            else if (ProjectId == null && TechnicalDepartmentId != null)
            {
                var items = db.ItemOutputs.Include(i => i.TechnicalDepartment)
                    .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId);

                ViewBag.ProjectIdv = ProjectId;
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
                return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }
            else if(ProjectId !=null && TechnicalDepartmentId ==null)
            {
                var items = db.ItemOutputs.Include(i => i.Project)
                   .Where(a => a.ProjectId == ProjectId);

                ViewBag.ProjectIdv = ProjectId;
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;

                return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(db.ItemOutputs.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));

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

                    var AvailableQnt = db.Items.Find(res.ItemId).ItemReminder;
                    if (res.ItemOutputQuantity <= AvailableQnt)
                    {
                        if (res != null)
                        {
                            res.ItemOutputApproved = true;
                            db.Items.Find(res.ItemId).ItemQuantityWithdraw += res.ItemOutputQuantity;
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

        
        public ActionResult technicalList(int? Page ,int? TechnicalDepartmentId, int? ProjectId)
        {
            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i => i.Project).Include(i => i.TechnicalDepartment).Where(a => a.Project.ProjectFinished == false);
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            int pageNumber = (Page ?? 1);

            if (TechnicalDepartmentId != null && ProjectId != null) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
            {
                var items = db.ItemOutputs.Include(i => i.Project)
                      .Include(i => i.TechnicalDepartment)
                      .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.ProjectId == ProjectId && a.Project.ProjectFinished == false);
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
                ViewBag.ProjectIdv = ProjectId;
                return View(items.OrderBy(a=>a.DateCreated).ToPagedList(pageNumber,pageSize));
            }
            else if (ProjectId == null && TechnicalDepartmentId != null)
            {
                var items = db.ItemOutputs.Include(i => i.TechnicalDepartment).Include(i => i.Project)
                    .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.Project.ProjectFinished == false);
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
                ViewBag.ProjectIdv = ProjectId; 
                return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }
            else if (ProjectId != null && TechnicalDepartmentId == null)
            {
                var items = db.ItemOutputs.Include(i => i.Project)
                   .Where(a => a.ProjectId == ProjectId && a.Project.ProjectFinished == false);
                ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
                ViewBag.ProjectIdv = ProjectId;
                return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }

            else
                return View(itemOutputs.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));

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
           
            if (ModelState.IsValid)
            {
                db.ItemOutputs.Add(itemOutput);

                DemandItem demandItem = new DemandItem();

                var AvailableQntInStore = db.Items.Find(itemOutput.ItemId).ItemReminder;

                var ItemRequiredQntList = db.ItemOutputs.Where(a => a.ItemId == itemOutput.ItemId && a.ItemOutputApproved== false).ToList();

                double RequiredQnt =itemOutput.ItemOutputQuantity;

                for(int i=0; i<ItemRequiredQntList.Count; i++)
                {
                    RequiredQnt += ItemRequiredQntList[i].ItemOutputQuantity;
                }

                if (RequiredQnt > AvailableQntInStore)
                {
                    //check if item available in demand table 
                    var demanItemAvailable = db.DemandItems.Where(a => a.ItemId == itemOutput.ItemId && a.DemandItemApproval==false).ToList();

                    if (demanItemAvailable.Count != 0) // if available , add quantity only
                    {
                         // will calculate required qnt in every item output order to prevent mistakes in quantities.
                         db.DemandItems.FirstOrDefault(a => a.ItemId == itemOutput.ItemId && a.DemandItemApproval == false).DemandItemQuantity = RequiredQnt - AvailableQntInStore  ;
                    }
                    else // add qnt and item name .
                    {
                        demandItem.ItemId = itemOutput.ItemId;

                        demandItem.DemandItemQuantity = RequiredQnt - AvailableQntInStore;   

                        db.DemandItems.Add(demandItem);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

  
            return View(itemOutput);
        }
        [VerifyUser(Roles = "superadmin,warehouse")]
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
        [VerifyUser(Roles = "superadmin,warehouse")]
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
        [VerifyUser(Roles = "superadmin,warehouse")]
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
        [VerifyUser(Roles = "superadmin,warehouse")]
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
