using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
    public class ItemsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 20;
        // GET: Items
        [VerifyUser(Roles ="superadmin,warehouse,warehouseaudit,cost")]
        public ActionResult Index(int? year ,int? Page , int? category, int? subcategory,int? item, string startDate, string endDate)
        {
            
            double totalAVG = 0;
            double totalAdded = 0;
            double totalOut = 0;
            double totalRemainder = 0;

           

            ViewBag.categoryv = category;
            ViewBag.subcategoryv = subcategory;
            ViewBag.item = item;
            int pageNumber = (Page ?? 1);
            
            if (year.HasValue == false)
            {
                year = DateTime.Now.Year;
            }
            DateTime toCompare = new DateTime(year.Value, 1, 1);
            toCompare = toCompare.AddYears(1);
            if(endDate!=null)
            {
                toCompare = DateTime.ParseExact(endDate,
                    "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            DateTime fromCompare = toCompare.AddYears(-1);
            if (startDate != null)
            {
                fromCompare = DateTime.ParseExact(startDate,
                    "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            var baseitems = db.Items
               .Include(a => a.ItemSubCategory)
               .Include(a => a.ItemSubCategory.ItemCategory)
               .Include(a => a.ItemInputs)
               .Include(a => a.ItemOutputs)
               .Include(a=>a.ItemReturns);
            if(item.HasValue)
            {
                if(item.Value!=0)
                {
                    baseitems = baseitems.Where(a => a.ItemId == item.Value);
                }
                else if (subcategory.HasValue)
                {
                    if (subcategory.Value != 0)
                        baseitems = baseitems.Where(a => a.ItemSubCategoryId == subcategory.Value);
                    else if (category.HasValue)
                    {
                        baseitems = baseitems.Where(a => a.ItemSubCategory.ItemCategoryId == category.Value);

                    }
                }
                else if (category.HasValue)
                {
                    baseitems = baseitems.Where(a => a.ItemSubCategory.ItemCategoryId == category.Value);

                }
            }
            else if (subcategory.HasValue)
            {
                if(subcategory.Value != 0)
                    baseitems = baseitems.Where(a=>a.ItemSubCategoryId == subcategory.Value);
                else if (category.HasValue)
                {
                    baseitems = baseitems.Where(a => a.ItemSubCategory.ItemCategoryId == category.Value);

                }
            }
            else if (category.HasValue)
            {
                baseitems = baseitems.Where(a => a.ItemSubCategory.ItemCategoryId == category.Value);
                
            }
            //DateTime dt = DateTime.Now;
            var itms = db.ItemInputs.Where(aa => aa.DateCreated < fromCompare).ToList();
            var itreturns = db.ItemReturns
                    .Where(aa =>
                    aa.DateCreated <= toCompare &&
                    aa.DateCreated >= fromCompare
                    ).ToList();
            var itins = db.ItemInputs.
                    Where(aa => aa.DateCreated <= toCompare && aa.DateCreated >= fromCompare).ToList();
            var itouts = db.ItemOutputs
                    .Where(aa => aa.DateCreated <= toCompare && aa.DateCreated >= fromCompare && aa.ItemOutputApproved).ToList();
            var items = baseitems
                .OrderBy(a=>a.ItemId)
                .ToList()
                .Select(
                a =>
                {
                    
                    a.ItemQuantity = itms.Count()==0?0:itms.Where(aa=>aa.ItemId == a.ItemId).Sum(aa=>aa.ItemQuantity);
                    a.ItemReturns = itreturns.Where(aa=>aa.ItemId == a.ItemId).ToList();
                    a.ItemInputs = itins.Where(aa=>aa.ItemId == a.ItemId).ToList();
                    a.ItemOutputs = itouts.Where(aa => aa.ItemId == a.ItemId).ToList();
                    var a1 = (a.ItemInputs.Sum(aa => aa.ItemQuantity));
                    var a2 = (a.ItemOutputs.Where(aa => aa.ItemOutputApproved).Sum(aa => aa.ItemOutputQuantity));
                    var a3 = (a.ItemReturns.Where(aa => aa.projectId == null).Sum(aa => aa.ItemQuantity));
                    var a4 = (a.ItemReturns.Where(aa => aa.projectId != null).Sum(aa => aa.ItemQuantity));
                    a.ItemReminder = 
                    a1 - 
                    a2 - 
                    a3 + 
                    a4;

                    totalAVG += a.ItemAvgPrice * a.ItemQuantity;
                    totalAdded += (a.ItemInputs.Sum(aa => aa.ItemTotalCost) + (a.ItemReturns.Where(aa => aa.projectId != null).Sum(aa => aa.ItemQuantity) * a.ItemAvgPrice));
                    totalOut += ((a.ItemOutputs.Sum(aa => aa.ItemOutputQuantity) + a.ItemReturns.Where(aa => aa.projectId == null).Sum(aa => aa.ItemQuantity)) * a.ItemAvgPrice);
                    totalRemainder += (a.ItemReminder * a.ItemAvgPrice);

                    return a;
                });
            ViewBag.totalAVG = totalAVG;
            ViewBag.totalAdded = totalAdded;
            ViewBag.totalOut = totalOut;
            ViewBag.totalRemainder = totalRemainder;
            //var lll = items.ToList();
            //var mil = DateTime.Now.Subtract(dt).TotalMilliseconds;
            return View(items.OrderBy(a => a.ItemId).ToPagedList(pageNumber, pageSize));
            


        }

        //[HttpPost]
        //public ActionResult Index(int? category , int? subcategory)
        //{

        //    ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");

        //    if (subcategory !=0 && category !=0) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
        //    {
        //       var  items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
        //             .Include(i => i.ItemSubCategory)
        //             .Where(a => a.ItemSubCategory.ItemCategoryId == category && a.ItemSubCategoryId == subcategory);
        //        foreach (var v in items)
        //        {
        //            v.ItemReminder = v.ItemQuantity + v.ItemQuantityAdded - v.ItemQuantityWithdraw - v.ItemReturn;
        //        }
        //   //     db.Entry(items).State = EntityState.Modified;

        //        db.SaveChanges();
        //        return View(items.ToList());
        //    }
        //    else
        //    {

        //        var items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
        //            .Where(a => a.ItemSubCategory.ItemCategoryId == category);
        //        foreach (var v in items)
        //        {
        //            v.ItemReminder = v.ItemQuantity + v.ItemQuantityAdded - v.ItemQuantityWithdraw - v.ItemReturn;
        //        }
        // //       db.Entry(items).State = EntityState.Modified;

        //        db.SaveChanges();
        //        return View(items.ToList());
        //    }


        //}

        public JsonResult GetMembers(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ItemSubCategory> itemSubCategoriesList = db.ItemSubCategories.Where(x => x.ItemCategoryId == id).ToList();
            return Json(itemSubCategoriesList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSubCategories(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Item> items = db.Items.Where(x => x.ItemSubCategoryId == id).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

       

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [VerifyUser(Roles = "superadmin,warehouse")]
        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouse")]
        public ActionResult Create([Bind(Include = "ItemId,ItemName,ItemUnit,ItemQuantity,ItemAvgPrice,ItemSubCategoryId,DateCreated,BinCode")] Item item)
        {
            if (ModelState.IsValid)
            {
      
                db.Items.Add(item);
                db.SaveChanges();
                Helper.AddLog(db, "Created Item ", item.ItemId, "Items", this);
                //var newItemInput = new ItemInput()
                //{
                //    ItemId = item.ItemId,
                //    ItemPrice = 0,
                //    ItemQuantity = item.ItemQuantity,
                //    ItemTotalCost = 0,
                //    Notes = ""
                //};
                //db.ItemInputs.Add(newItemInput);
                //db.SaveChanges();
                return RedirectToAction("Create","ItemInputs");
            }

            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName", item.ItemSubCategoryId);
            return View(item);
        }

        [VerifyUser(Roles = "superadmin")]
        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName", item.ItemSubCategoryId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin")]
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,ItemUnit,ItemQuantity,ItemAvgPrice,ItemSubCategoryId,DateCreated,BinCode")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                Helper.AddLog(db, "Edited Item ", item.ItemId, "Items", this);
                return RedirectToAction("Index");
            }
            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName", item.ItemSubCategoryId);
            return View(item);
        }

        // GET: Items/Delete/5
        [VerifyUser(Roles = "superadmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            Helper.AddLog(db, "Deleted Item ", item.ItemId, "Items", this);
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
