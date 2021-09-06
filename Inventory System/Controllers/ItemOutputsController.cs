using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
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

        int pageSize = 20;
        // GET: ItemOutputs
        [VerifyUser(Roles = "superadmin,warehouse,cost,warehouseaudit")]
        public ActionResult Index(int? page, int? TechnicalDepartmentId, int? ProjectId, int? docNumber, string startDate,string endDate)
        {
            

            User user;
            Helper.CheckUser(HttpContext, db, out user);
            ViewBag.MainRole = user.Roles;
            int pageNumber =1;
            if(page.HasValue)
            {
                pageNumber = page.Value;
            }
            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i=>i.Item.ItemInputs).Include(i => i.Project).Include(i => i.TechnicalDepartment);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");

            //---------------------------------------------------
            var res = itemOutputs.ToList();
            if (TechnicalDepartmentId != null)
                res = res.Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId).ToList();
            if (ProjectId != null)
                res = res.Where(a => a.ProjectId == ProjectId).ToList();
            if (startDate != null && endDate != null)
            {

                res = helper.Classes.Helper.FilterByDate<ItemOutput>(startDate, endDate,
                    res.AsQueryable());
            }
            double totalPrices = 0;
            foreach(var item in res.ToList())
            {
                totalPrices += (item.Item.ItemAvgPrice * item.ItemOutputQuantity);
            }
            ViewBag.totalPrices = totalPrices;
            if(docNumber.HasValue)
            {
                res = res.Where(a => a.DocCode == docNumber.Value).ToList();
            }
            return View(res.OrderByDescending(a => a.DocCode).ToPagedList(pageNumber, 20));
            //if (TechnicalDepartmentId != null && ProjectId != null) 
            //{
            //    var items = db.ItemOutputs.Include(i => i.Project)
            //          .Include(i => i.TechnicalDepartment)
            //          .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.ProjectId == ProjectId);

            //    ViewBag.ProjectIdv = ProjectId;
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
            //    return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            //}
            //else if (ProjectId == null && TechnicalDepartmentId != null)
            //{
            //    var items = db.ItemOutputs.Include(i => i.TechnicalDepartment)
            //        .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId);

            //    ViewBag.ProjectIdv = ProjectId;
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
            //    return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            //}
            //else if(ProjectId !=null && TechnicalDepartmentId ==null)
            //{
            //    var items = db.ItemOutputs.Include(i => i.Project)
            //       .Where(a => a.ProjectId == ProjectId);

            //    ViewBag.ProjectIdv = ProjectId;
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;

            //    return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            //}
            //else
            //    return View(db.ItemOutputs.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));

        }

        [VerifyUser(Roles = "superadmin,warehouse,cost,warehouseaudit")]
        public ActionResult warehouse(int? TechnicalDepartmentId, int? ProjectId)
        {
            
            return ReturnWarehouse(TechnicalDepartmentId,ProjectId);
        }
        [VerifyUser(Roles = "superadmin,warehouse,cost,warehouseaudit")]
        ActionResult ReturnWarehouse(int? TechnicalDepartmentId, int? ProjectId)
        {
            if (ProjectId != null)
            {
                ViewBag.prid = ProjectId;
            }
            var itemOutputs = db.ItemOutputs.Include(i => i.Item).Include(i => i.Project).Include(i => i.TechnicalDepartment).Where(a => a.ItemOutputApproved == false);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            if (TechnicalDepartmentId != null)
                itemOutputs = itemOutputs.Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId);
            if (ProjectId != null)
                itemOutputs = itemOutputs.Where(a => a.ProjectId == ProjectId);
            return View("warehouse",itemOutputs.OrderByDescending(a => a.DocCode).ToList());
        }

        [HttpPost]
        [VerifyUser(Roles = "superadmin,warehouse,cost,warehouseaudit")]
        public ActionResult Approve(int[] ItemApproved,double[] ItemQ, int[] ItemDoc)
        {
            if (ItemApproved != null)
            {
                double qToAdd = 0;
                int outputIdToAdd = 0;
                List<int> itemsOut = new List<int>();
                List<int> itemsOutNo = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.Required,
        new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    int inddd = 0;
                    foreach (var v in ItemApproved)
                    {
                        var res = db.ItemOutputs.Include(a => a.Item)
                            .Include(a => a.Item.ItemInputs)
                            .Include(a => a.Item.ItemOutputs)
                            .Include(a => a.Item.ItemReturns)
                            .Single(a => a.ItemOutputId == v);

                        var AvailableQnt = (res.Item.ItemInputs.Sum(a => a.ItemQuantity)
                            -
                            res.Item.ItemOutputs.Where(a => a.ItemOutputApproved).Sum(a => a.ItemOutputQuantity))
                            + res.Item.ItemReturns.Where(a=>a.projectId!=null).Sum(a => a.ItemQuantity)
                            - res.Item.ItemReturns.Where(a => a.projectId == null).Sum(a => a.ItemQuantity);
                        if (ItemQ[inddd] <= AvailableQnt)
                        {
                            if (res != null)
                            {
                                res.ItemOutputApproved = true;
                                res.ExchangeDate = DateTime.Now;
                                res.DocCode = ItemDoc[inddd];
                                db.Items.Find(res.ItemId).ItemQuantityWithdraw += ItemQ[inddd];//res.ItemOutputQuantity;
                                if(ItemQ[inddd]!=res.ItemOutputQuantity)
                                {
                                    var diff = res.ItemOutputQuantity - ItemQ[inddd];
                                    res.ItemOutputQuantity = ItemQ[inddd];
                                    db.ItemOutputs.Add(new ItemOutput()
                                    {
                                        ItemId = res.ItemId,
                                        ItemOutputQuantity = diff,
                                        ProjectId = res.ProjectId,
                                        TechnicalDepartmentId = res.TechnicalDepartmentId
                                    });
                                }
                            }
                            itemsOut.Add(v);
                        }
                        else
                        {
                            outputIdToAdd = v;
                            qToAdd = ItemQ[inddd] - AvailableQnt;
                            ViewBag.msg = " هذه الكميه غير متاحه";
                            break;
                        }
                        inddd++;
                    }
                    if (qToAdd == 0)
                    {
                        db.SaveChanges();
                    }

                    scope.Complete();
                }
                if(qToAdd!=0)
                {
                    if (db.DemandItems.Where(a => a.ItemOutputId == outputIdToAdd).ToList().Count() == 0)
                    {
                        db.DemandItems.Add(new DemandItem()
                        {
                            DemandItemPriority = DateTime.Now,
                            DemandItemQuantity = qToAdd,
                            ItemOutputId = outputIdToAdd
                        });
                        db.SaveChanges();
                        Helper.AddNotification(db,
                                "يوجد خامة غير متوفرة",
                                "يوجد خامة غير متوفرة",
                                db.Users.Where(a => a.Roles == "demandplanning").ToList());
                    }
                }

                foreach (var v in itemsOut)
                {
                    Helper.AddLog(db, "ItemOut Approved ", v, "ItemOutput", this);
                }
                foreach(var v in itemsOutNo)
                {
                    Helper.AddLog(db, "ItemOut NOT Approved No Quantity", v, "ItemOutput", this);
                }
            }
            
            return RedirectToAction("Index");
        }

        [VerifyUser(Roles = "projectplanning")]
        public ActionResult technicalList(int? Page ,int? TechnicalDepartmentId, int? ProjectId)
        {
            var itemOutputs = db.ItemOutputs
                .Include(a => a.Item.ItemInputs)
                .Include(i => i.Item)
                .Include(a => a.Item.ItemReturns)
                .Include(i=>i.Item.ItemOutputs)
                .Include(i => i.Project)
                .Include(i => i.TechnicalDepartment).Where(a => a.Project.ProjectFinished == false);
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            int pageNumber = (Page ?? 1);
            var res = db.ItemOutputs.
                Include(a => a.Project).
                Include(a => a.TechnicalDepartment)
                .Include(a => a.Item.ItemInputs)
                .Include(a=>a.Item.ItemReturns)
                .Include(a => a.Item.ItemOutputs)
                .Include(a => a.Item).ToList();
            if (TechnicalDepartmentId != null)
                res = res.Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId).ToList();
            if (ProjectId != null)
                res = res.Where(a => a.ProjectId == ProjectId).ToList();

            return View(res.OrderByDescending(a => a.DocCode).ToPagedList(1, 20));
            //if (TechnicalDepartmentId != null && ProjectId != null) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
            //{
            //    var items = db.ItemOutputs.Include(i => i.Project)
            //          .Include(i => i.TechnicalDepartment)
            //          .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.ProjectId == ProjectId && a.Project.ProjectFinished == false);
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
            //    ViewBag.ProjectIdv = ProjectId;
            //    return View(items.OrderBy(a=>a.DateCreated).ToPagedList(pageNumber,pageSize));
            //}
            //else if (ProjectId == null && TechnicalDepartmentId != null)
            //{
            //    var items = db.ItemOutputs.Include(i => i.TechnicalDepartment).Include(i => i.Project)
            //        .Where(a => a.TechnicalDepartmentId == TechnicalDepartmentId && a.Project.ProjectFinished == false);
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
            //    ViewBag.ProjectIdv = ProjectId; 
            //    return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            //}
            //else if (ProjectId != null && TechnicalDepartmentId == null)
            //{
            //    var items = db.ItemOutputs.Include(i => i.Project)
            //       .Where(a => a.ProjectId == ProjectId && a.Project.ProjectFinished == false);
            //    ViewBag.TechnicalDepartmentIdv = TechnicalDepartmentId;
            //    ViewBag.ProjectIdv = ProjectId;
            //    return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            //}

            //else
            //    return View(itemOutputs.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));

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

        [VerifyUser(Roles = "warehouse,projectplanning")]
        // GET: ItemOutputs/Create
        public ActionResult Create(int? prid)
        {
            
            ViewBag.ItemCategory = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            List<Item> itms = new List<Item>();
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            foreach(var v in db.Items
                    .Include(a => a.ItemOutputs)
                    .Include(a => a.ItemReturns)
                    .Include(a => a.ItemInputs).ToList())
                    
            {
                
                v.ItemReminder = v.ItemInputs.Sum(a => a.ItemQuantity) -
                            v.ItemOutputs.Where(a=>a.ItemOutputApproved).Sum(a => a.ItemOutputQuantity) -
                            v.ItemReturns.Where(a => a.projectId == null).Sum(a => a.ItemQuantity) +
                            v.ItemReturns.Where(a => a.projectId != null).Sum(a => a.ItemQuantity);
                itms.Add(new Item()
                {
                    ItemId = v.ItemId,
                    ItemUnit = v.ItemUnit,
                    ItemReminder = v.ItemReminder
                });
            }
            ViewBag.allItems = Newtonsoft.Json.JsonConvert.SerializeObject(itms);
            if (prid != null)
            {
                itms = new List<Item>();
                ViewBag.prid = prid;
                
                foreach(var v in db.Projects
                    .Include(a=>a.ItemOutputs)
                    .Include(a=>a.ItemReturns)
                    .Include(a=>a.ItemOutputs.Select(aa=>aa.Item.ItemInputs))
                    .Include(a=>a.ItemOutputs.Select(aa=>aa.Item))
                    .Single(a => a.ProjectId == prid.Value).ItemOutputs)
                {
                    if (itms.Where(a => a.ItemId == v.ItemId).Count() == 0)
                    {
                        v.Item.ItemReminder = v.Item.ItemInputs.Sum(a => a.ItemQuantity) -
                            v.Item.ItemOutputs.Sum(a => a.ItemOutputQuantity) -
                            v.Item.ItemReturns.Where(a => a.projectId == null).Sum(a => a.ItemQuantity) +
                            v.Item.ItemReturns.Where(a => a.projectId != null).Sum(a => a.ItemQuantity);
                        itms.Add(new Item()
                        {
                            ItemId = v.Item.ItemId,
                            ItemUnit = v.Item.ItemUnit,
                            ItemReminder = v.Item.ItemReminder
                        });
                    }
                }
                ViewBag.ItemId = new SelectList(itms, "ItemId", "ItemName");
                ViewBag.allItems = Newtonsoft.Json.JsonConvert.SerializeObject(itms);
            }
            
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            ViewBag.TechnicalDepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName");
            return View();
        }

        // POST: ItemOutputs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[VerifyUser(Roles = "warehouse,projectplanning")]
        //public ActionResult Create([Bind(Include = "ItemOutputId,ItemOutputQuantity,ItemId,ProjectId,TechnicalDepartmentId,ItemOutputApproved,DateCreated")] ItemOutput itemOutput)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        db.ItemOutputs.Add(itemOutput);
        //        Helper.AddLog(db, "Created ItemOut ", itemOutput.ItemId, "ItemOutput", this);
        //        DemandItem demandItem = new DemandItem();
        //        var selInputs = db.ItemInputs.Where(a => a.ItemId == itemOutput.ItemId);
        //        var selOutputs = db.ItemOutputs.Where(aa => aa.ItemOutputApproved && aa.ItemId == itemOutput.ItemId);
        //        var AvailableQntInStore = 
        //            (selInputs.Count()==0?0:selInputs.Sum(aa => aa.ItemQuantity)) - (selOutputs.Count()==0?0:selOutputs.Sum(aa => aa.ItemOutputQuantity));

        //        db.SaveChanges();

        //        double RequiredQnt = itemOutput.ItemOutputQuantity;



        //        if (RequiredQnt > AvailableQntInStore)
        //        {
        //            //check if item available in demand table 



        //                demandItem.ItemOutputId = itemOutput.ItemOutputId;

        //                demandItem.DemandItemQuantity = RequiredQnt - AvailableQntInStore;   

        //                db.DemandItems.Add(demandItem);
        //            Helper.AddNotification(db,
        //                        "يوجد خامة غير متوفرة",
        //                        "يوجد خامة غير متوفرة",
        //                        db.Users.Where(a => a.Roles == "demandplanning").ToList());
        //            Helper.AddLog(db, "ItemOutput Not sufficient, requested from demand", demandItem.DemandItemId, "DemandItem", this);
        //        }
        //        db.SaveChanges();
        //        if(((Inventory_System.Models.User)ViewBag.mainUser).Roles=="warehouse")
        //        {
        //            return RedirectToAction("warehouse", "itemoutputs");
        //        }
        //        return RedirectToAction("technicallist", "itemoutputs");
        //    }


        //    if (((Inventory_System.Models.User)ViewBag.mainUser).Roles == "warehouse")
        //    {
        //        return RedirectToAction("warehouse", "itemoutputs");
        //    }
        //    return RedirectToAction("technicallist", "itemoutputs");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "warehouse,projectplanning")]
        public ActionResult Create(int[] ItemId, int[] ProjectId, double[] ItemOutputQuantity, int[] TechnicalDepartmentId)
        {
            List<ItemOutput> itemOutputs = new List<ItemOutput>();
            for(int i =0;i<ItemId.Length;i++)
            {
                itemOutputs.Add(new ItemOutput()
                {
                    ItemId = ItemId[i],
                    ProjectId = ProjectId[i],
                    ItemOutputQuantity = ItemOutputQuantity[i]
                });
            }
            if (ModelState.IsValid)
            {
                foreach (var itemOutput in itemOutputs)
                {
                    db.ItemOutputs.Add(itemOutput);
                    Helper.AddLog(db, "Created ItemOut ", itemOutput.ItemId, "ItemOutput", this);
                    DemandItem demandItem = new DemandItem();
                    var selInputs = db.ItemInputs.Where(a => a.ItemId == itemOutput.ItemId);
                    var selOutputs = db.ItemOutputs.Where(aa => aa.ItemOutputApproved && aa.ItemId == itemOutput.ItemId);
                    var AvailableQntInStore =
                        (selInputs.Count() == 0 ? 0 : selInputs.Sum(aa => aa.ItemQuantity)) - (selOutputs.Count() == 0 ? 0 : selOutputs.Sum(aa => aa.ItemOutputQuantity));

                    db.SaveChanges();

                    double RequiredQnt = itemOutput.ItemOutputQuantity;



                    if (RequiredQnt > AvailableQntInStore)
                    {
                        //check if item available in demand table 



                        demandItem.ItemOutputId = itemOutput.ItemOutputId;

                        demandItem.DemandItemQuantity = RequiredQnt - AvailableQntInStore;

                        db.DemandItems.Add(demandItem);
                        Helper.AddNotification(db,
                                    "يوجد خامة غير متوفرة",
                                    "يوجد خامة غير متوفرة",
                                    db.Users.Where(a => a.Roles == "demandplanning").ToList());
                        Helper.AddLog(db, "ItemOutput Not sufficient, requested from demand", demandItem.DemandItemId, "DemandItem", this);
                    }
                    db.SaveChanges();

                }
                if (((Inventory_System.Models.User)ViewBag.mainUser).Roles == "warehouse")
                {
                    return RedirectToAction("warehouse", "itemoutputs");
                }
                return RedirectToAction("technicallist", "itemoutputs");

            }


            if (((Inventory_System.Models.User)ViewBag.mainUser).Roles == "warehouse")
            {
                return RedirectToAction("warehouse", "itemoutputs");
            }
            return RedirectToAction("technicallist", "itemoutputs");
        }
        [VerifyUser(Roles = "superadmin,warehouse,projectplanning,warehouseaudit")]
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
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", itemOutput.ProjectId);
            ViewBag.DepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName",itemOutput.TechnicalDepartmentId);

            return View(itemOutput);
        }

        // POST: ItemOutputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit,projectplanning")]
        public ActionResult Edit([Bind(Include = "ItemOutputId,ItemOutputQuantity,ItemId,ProjectId,DateCreated,TechnicalDepartmentId,DocCode", Exclude =("ExchangeDate"))] ItemOutput itemOutput)
        {
            if (ModelState.IsValid)
            {
                var original_data = db.ItemOutputs.AsNoTracking().Where(P => P.ItemOutputId == itemOutput.ItemOutputId).FirstOrDefault();
                if (original_data.ItemOutputApproved && itemOutput.ItemOutputApproved == false)
                    itemOutput.ExchangeDate = null;
                else if (!original_data.ItemOutputApproved && itemOutput.ItemOutputApproved)
                    itemOutput.ExchangeDate = DateTime.Now;
                db.Entry(itemOutput).State = EntityState.Modified;
                db.SaveChanges();
                Helper.AddLog(db, "Edited ItemOutput ", itemOutput.ItemId, "ItemOutput", this);
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemOutput.ItemId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", itemOutput.ProjectId);
            ViewBag.DepartmentId = new SelectList(db.TechnicalDepartments, "TechnicalDepartmentId", "TechnicalDepartmentName",itemOutput.TechnicalDepartmentId);

            return View(itemOutput);
        }

        // GET: ItemOutputs/Delete/5
        [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit,projectplanning")]
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
        [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit,projectplanning")]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemOutput itemOutput = db.ItemOutputs.Find(id);
            db.ItemOutputs.Remove(itemOutput);
            db.SaveChanges();
            Helper.AddLog(db, "Deleted ItemOutput ", id, "ItemOutput", this);
            return RedirectToAction("technicalList");
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
