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
        public ActionResult Index()
        {

            ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            //ViewBag.subcategory = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName");
            //   var items = db.Items.Include(i => i.ItemSubCategory);
            // ViewBag.category = db.ItemCategories;
            var items = db.Items;
            return View(items.ToList());
        }

        [HttpPost]
        public ActionResult Index(int? category , int? subcategory)
        {
           
            ViewBag.category = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            //ViewBag.subcategory = new SelectList(db.ItemSubCategories, "ItemSubCategoryId", "ItemSubCategoryName");
            //ViewBag.item = new SelectList(db.Items, "ItemId", "ItemName");
            

            if (subcategory !=0 && category !=0) // this condition is wrong ... momkn ast8na 3no ... if i can set category drop down list any text after each search process.
            {
               var  items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
                     .Include(i => i.ItemSubCategory)
                     .Where(a => a.ItemSubCategory.ItemCategoryId == category && a.ItemSubCategoryId == subcategory);
                return View(items.ToList());
            }
            else
            {
                var items = db.Items.Include(i => i.ItemSubCategory.ItemCategory)
                    .Where(a => a.ItemSubCategory.ItemCategoryId == category);
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
