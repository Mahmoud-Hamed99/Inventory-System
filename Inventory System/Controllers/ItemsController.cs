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
    public class ItemsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: Items
        public ActionResult Index(int? year)
        {
            if(year.HasValue == false)
            {
                year = DateTime.Now.Year;
            }
            DateTime toCompare = new DateTime(year.Value, 1, 1);
            ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
         
            var items = db.Items.Include(a=>a.ItemSubCategory).Include(a=>a.ItemSubCategory.ItemCategory).ToList();

            foreach(var v in items)
            {
                var inputsSum = db.ItemInputs.Where(a => a.DateCreated < toCompare &&
                a.ItemId == v.ItemId).ToList().Sum(a => a.ItemQuantity);
                inputsSum += db.ItemReturns.Where(a => a.DateCreated < toCompare &&
                a.ItemId == v.ItemId).ToList().Sum(a => a.ItemQuantity);

                var outputsSum = db.ItemOutputs.Where(a => a.DateCreated < toCompare &&
                a.ItemId == v.ItemId).ToList().Sum(a => a.ItemOutputQuantity);
                var remainder = inputsSum - outputsSum;
                if(remainder > 0)
                {
                    v.ItemQuantity = remainder;
                }
                v.ItemReminder = v.ItemQuantityAdded - v.ItemQuantityWithdraw + v.ItemReturn;
            }
            db.SaveChanges();
            return View(items.ToList());
        }

        [HttpPost]
        public ActionResult Index(int? category , int? subcategory)
        {
           
            ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            
            if (subcategory !=0 && category !=0) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
            {
               var  items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
                     .Include(i => i.ItemSubCategory)
                     .Where(a => a.ItemSubCategory.ItemCategoryId == category && a.ItemSubCategoryId == subcategory);
                foreach (var v in items)
                {
                    v.ItemReminder = v.ItemQuantity + v.ItemQuantityAdded - v.ItemQuantityWithdraw - v.ItemReturn;
                }
           //     db.Entry(items).State = EntityState.Modified;

                db.SaveChanges();
                return View(items.ToList());
            }
            else
            {

                var items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
                    .Where(a => a.ItemSubCategory.ItemCategoryId == category);
                foreach (var v in items)
                {
                    v.ItemReminder = v.ItemQuantity + v.ItemQuantityAdded - v.ItemQuantityWithdraw - v.ItemReturn;
                }
         //       db.Entry(items).State = EntityState.Modified;

                db.SaveChanges();
                return View(items.ToList());
            }

            
        }

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
        public ActionResult Create([Bind(Include = "ItemId,ItemName,ItemUnit,ItemQuantity,ItemAvgPrice,ItemSubCategoryId,DateCreated")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                var newItemInput = new ItemInput()
                {
                    ItemId = item.ItemId,
                    ItemPrice = 0,
                    ItemQuantity = item.ItemQuantity,
                    ItemTotalCost = 0,
                    Notes = ""
                };
                db.ItemInputs.Add(newItemInput);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName", item.ItemSubCategoryId);
            return View(item);
        }

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
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,ItemUnit,ItemQuantity,ItemAvgPrice,ItemSubCategoryId,DateCreated")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemSubCategoryId = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName", item.ItemSubCategoryId);
            return View(item);
        }

        // GET: Items/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
