using Inventory_System.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using helper.Classes;

namespace Inventory_System.Controllers
{
    public class ItemInputsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 20;
        // GET: ItemInputs
        [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit,cost")]
        public ActionResult Index( int? page , bool acc = false)
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            ViewBag.MainRole = user.Roles;
            if (user.Roles == "warehouse")
            {
                acc = false;
            }
            else
            {
                acc = true;
            }
            ViewBag.IsAccountant = acc;
            var itemInputs = db.ItemInputs.Include(i => i.Item).Include(a => a.Vendor).ToList();
            int pageNumber = (page ?? 1);
            return View(itemInputs.OrderBy(a=>a.ItemInputId).ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        [VerifyUser(Roles = "superadmin,warehouse,warehouseaudit,cost")]
        public ActionResult Index(string startDate, string endDate)
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            ViewBag.MainRole = user.Roles;
            if (user.Roles == "warehouse")
            {
                ViewBag.IsAccountant = false;
            }
            else
            {
                ViewBag.IsAccountant = true;
            }

            var itemInputs = helper.Classes.Helper.FilterByDate<ItemInput>(startDate, endDate,
                    db.ItemInputs.OrderBy(a => a.ItemInputId).Include(i => i.Item).Include(a => a.Vendor));
            
            
            return View(itemInputs.ToPagedList(1,1000000000));
        }

        // GET: ItemInputs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemInput itemInput = db.ItemInputs.Find(id);
            if (itemInput == null)
            {
                return HttpNotFound();
            }
            return View(itemInput);
        }
        [VerifyUser(Roles = "superadmin,warehouse,cost")]
        // GET: ItemInputs/Create
        public ActionResult Create(string ItemCategory)
        {
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorName");
            ViewBag.ItemCategory = new SelectList(db.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            return View();
        }
        // POST: ItemInputs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouse,cost")]
        public ActionResult Create([Bind(Include = "ItemInputId,ItemId,ItemPrice,ItemQuantity,ItemTotalCost,VendorId,DateCreated")] ItemInput itemInput)
        {
            //  List<Item> itemList = db.Items.ToList();

            // int itemLen = db.Items.Count();
            if (ModelState.IsValid)
            {
                db.ItemInputs.Add(itemInput);
                db.SaveChanges();


                var x = db.Items.Find(itemInput.ItemId);
                if (x.ItemQuantity == 0)
                {
                    x.ItemQuantity = itemInput.ItemQuantity;
                }
                var itmWithPrice = db.ItemInputs.Where(a => a.ItemId == x.ItemId && a.ItemPrice > 0).ToList();
                if (itmWithPrice.Count() > 0)
                {
                    x.ItemAvgPrice = itmWithPrice.OrderByDescending(a => a.DateCreated).First().ItemPrice;
                }
                x.ItemQuantityAdded += itemInput.ItemQuantity;
                double itemQ = itemInput.ItemQuantity;
                foreach (var v in db.DemandItems.Where(a => a.DemandItemApproval && a.PurchasingApproval && a.DemandItemQuantity > 0))
                {
                    if (itemQ > 0)
                    {
                        if (v.DemandItemQuantity >= itemQ)
                        {
                            v.DemandItemQuantity -= itemQ;
                            itemQ = 0;


                        }
                        else
                        {
                            itemQ -= v.DemandItemQuantity;
                            v.DemandItemQuantity = 0;
                        }

                    }

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(itemInput);
        }



        public JsonResult GetMembers(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ItemSubCategory> itemSubCategoriesList = db.ItemSubCategories.Where(x => x.ItemCategoryId == id).ToList();
            return Json(itemSubCategoriesList, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetItems(int itemSubCategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Item> items = db.Items.Where(x => x.ItemSubCategoryId == itemSubCategoryId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVendorName(int itemId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ItemInput> itemInputs = db.ItemInputs.Where(x => x.ItemId == itemId).ToList();
            return Json(itemInputs, JsonRequestBehavior.AllowGet);
        }



        
        // GET: ItemInputs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemInput itemInput = db.ItemInputs.Find(id);
            if (itemInput == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemInput.ItemId);
            List<ItemInput> itemInputs = db.ItemInputs.Include(x => x.Item).Include(x => x.Vendor).ToList();
            ViewBag.Item = itemInputs.Where(x => x.ItemInputId == id).FirstOrDefault();
            return View(itemInput);
        }

        // POST: ItemInputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemInputId,ItemId,ItemPrice,ItemQuantity,ItemTotalCost,VendorId,DateCreated")] ItemInput itemInput)
        {
            if (ModelState.IsValid)
            {
                // ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemInput.ItemId);
                //List<ItemInput> itemInputs = db.ItemInputs.Include(x => x.Item).Include(x => x.Vendor).ToList();

                db.Entry(itemInput).State = EntityState.Modified;
                db.SaveChanges();
                var x = db.Items.Find(itemInput.ItemId);
                
                var itmWithPrice = db.ItemInputs.Where(a => a.ItemId == x.ItemId && a.ItemPrice > 0).ToList();
                if (itmWithPrice.Count() > 0)
                {
                    x.ItemAvgPrice = itmWithPrice.OrderByDescending(a => a.DateCreated).First().ItemPrice;
                }
                x.ItemQuantityAdded += itemInput.ItemQuantity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemInput);
        }


        public ActionResult WarhouseManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemInput itemInput = db.ItemInputs.Find(id);
            var itemsRes = db.ItemInputs.Where(a => a.ItemId == itemInput.ItemId && a.ItemPrice > 0).OrderByDescending(a => a.DateCreated).ToList();
            ViewBag.lastPrice = itemsRes.Count()==0?0:itemsRes.FirstOrDefault().ItemPrice;
            if (itemInput == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemInput.ItemId);
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorName", itemInput.VendorId);
            List<ItemInput> itemInputs = db.ItemInputs.Include(x => x.Item).Include(x => x.Vendor).ToList();
            ViewBag.Item = itemInputs.Where(x => x.ItemInputId == id).FirstOrDefault();
            return View(itemInput);
        }

        // POST: ItemInputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WarhouseManager([Bind(Include = "ItemInputId,ItemId,ItemPrice,ItemQuantity,ItemTotalCost,VendorId,ItemReturn,Notes,DateCreated")] ItemInput itemInput,int minimumAllowed)
        {
            if (ModelState.IsValid)
            {


                //if(itm.ItemInputs.Count == 1)
                //{
                //    itm.ItemQuantity = itemInput.ItemQuantity;
                //}
                itemInput.ItemTotalCost = itemInput.ItemQuantity * itemInput.ItemPrice;
                db.Entry(itemInput).State = EntityState.Modified;
                db.SaveChanges();
                
                var itemsRes = db.ItemInputs.Where(a => a.ItemId == itemInput.ItemId && a.ItemPrice > 0).OrderByDescending(a => a.DateCreated).ToList();
                ViewBag.lastPrice = itemsRes.Count() == 0 ? 0 : itemsRes.FirstOrDefault().ItemPrice;
                var itm = db.Items.Single(a => a.ItemId == itemInput.ItemId);
                itm.ItemMinQuantity = minimumAllowed;
                var x = db.Items.Find(itemInput.ItemId);

                var itmWithPrice = db.ItemInputs.Where(a => a.ItemId == x.ItemId && a.ItemPrice > 0).ToList();
                if (itmWithPrice.Count() > 0)
                {
                    x.ItemAvgPrice = itmWithPrice.OrderByDescending(a => a.DateCreated).First().ItemPrice;
                }
                x.ItemQuantityAdded += itemInput.ItemQuantity;
                db.SaveChanges();
                itm = db.Items.Include(a => a.ItemInputs).Single(a => a.ItemId == itemInput.ItemId);
                if (itm.ItemInputs.Count == 1)
                {
                    itm.ItemQuantity = itemInput.ItemQuantity;
                }
                db.SaveChanges();
                var xy = itemInput.Item.ItemMinQuantity;

                if(itemInput.ItemReturn != 0)
                { 
                    ItemReturn itemReturn = new ItemReturn();
                    itemReturn.ItemInput = itemInput;
                    itemReturn.ItemId = itemInput.ItemId;
                    itemReturn.ItemQuantity = itemInput.ItemReturn;
                    
                   
                    
                    x.ItemQuantityWithdraw += itemInput.ItemReturn;
                    
                    db.ItemReturns.Add(itemReturn);
                    db.SaveChanges();
                }
                return RedirectToAction("Index",new {acc=true });
            }
            return View(itemInput);
        }


        // GET: ItemInputs/Delete/5
        [VerifyUser(Roles = "superadmin,warehouseaudit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemInput itemInput = db.ItemInputs.Find(id);
            if (itemInput == null)
            {
                return HttpNotFound();
            }
            return View(itemInput);
        }

        // POST: ItemInputs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [VerifyUser(Roles = "superadmin,warehouseaudit")]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemInput itemInput = db.ItemInputs.Find(id);
            db.ItemInputs.Remove(itemInput);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult Return(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ItemInput itemInput = db.ItemInputs.Find(id);

        //    if (itemInput == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", itemInput.ItemId);
        //    ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorName", itemInput.VendorId);
        //    //List<ItemInput> itemInputs = db.ItemInputs.Include(x => x.Item).Include(x => x.Vendor).ToList();
        //    //ViewBag.Item = itemInputs.Where(x => x.ItemInputId == id).FirstOrDefault();
        //    return View(itemInput);
        //}


        //[HttpPost, ActionName("Return")]
        //[ValidateAntiForgeryToken]
        //public ActionResult ReturnConfirmed(int id)
        //{
        //    ItemInput itemInput = db.ItemInputs.Find(id);
            
        //    ItemReturn itemReturn = new ItemReturn();

        //    itemReturn.ItemId = itemInput.ItemId;
        //    itemReturn.ItemInput.VendorId = itemInput.VendorId;
        //    itemReturn.ItemQuantity = 5; // el qnt will be taken from user .

        //    db.ItemReturns.Add(itemReturn);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //} 

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
