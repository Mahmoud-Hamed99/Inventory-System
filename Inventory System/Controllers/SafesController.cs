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
    [VerifyUser(Roles = "generalaccountant,safe")]
    public class SafesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: Safes
        int pageSize = 20;
        public ActionResult Index(int? Page, string startDate,string endDate)
        {


            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            List<Safe> SafeList = new List<Safe>();
            
            int pageNumber = (Page ?? 1);

            SafeList = helper.Classes.Helper.FilterByDate<Safe>(startDate, endDate,
                db.Safe.Include(a => a.SafeSubCategory).Include(a => a.SafeSubCategory.SafeCategory));
            var res = helper.Classes.Helper.DoCalculation(db.Safe, startDate, endDate);

            ViewBag.Statement = res;
            
            return View(SafeList.OrderBy(a=>a.SafeId).ToPagedList(pageNumber,pageSize));
        }

        // GET: Safes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safe safe = db.Safe.Find(id);
            if (safe == null)
            {
                return HttpNotFound();
            }
            return View(safe);
        }

        // GET: Safes/Create
        public ActionResult Create()
        {
            ViewBag.SafeSubCategoryId = new SelectList(db.safeSubCategories, "SafeSubCategoryId", "Name");
            return View();
        }

        // POST: Safes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SafeId,PermessionNumber,DateCreated,TransactionType,Deposit,Withdraw,Notes,SafeSubCategoryId")] Safe safe)
        {
            if (ModelState.IsValid)
            {
                db.Safe.Add(safe);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.SafeSubCategoryId = new SelectList(db.safeSubCategories, "SafeSubCategoryId", "Name");
            return View(safe);
        }

        // GET: Safes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safe safe = db.Safe.Find(id);
            if (safe == null)
            {
                return HttpNotFound();
            }
            ViewBag.SafeSubCategoryId = new SelectList(db.safeSubCategories, "SafeCategoryId", "Name", safe.SafeSubCategoryId);
            return View(safe);
        }

        // POST: Safes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SafeId,PermessionNumber,DateCreated,TransactionType,Deposit,Withdraw,Notes,SafeSubCategoryId")] Safe safe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(safe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(safe);
        }

        // GET: Safes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safe safe = db.Safe.Find(id);
            if (safe == null)
            {
                return HttpNotFound();
            }
            db.Safe.Remove(safe);
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
