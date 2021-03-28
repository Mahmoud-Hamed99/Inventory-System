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
using PagedList;

namespace Inventory_System.Controllers
{
    public class SafesController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: Safes
        int pageSize = 20;
        public ActionResult Index(int? Page, int? year, int? month1, int? month2)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            if (year != null && month1 != null && month2 != null)
            {
                StartDate = new DateTime(year.Value, month1.Value, 1);
                if (month2.Value > month1.Value)
                {
                    EndDate = new DateTime(year.Value, month2.Value, 1);
                    EndDate = EndDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    EndDate = StartDate.AddMonths(1);
                }
            }
            List<Safe> SafeList = new List<Safe>();
            double Balance = 0.0;
            int pageNumber = (Page ?? 1);

            if (year != null && month1 != null && month2 != null)
                SafeList = db.Safe.Where(a => a.DateCreated >= StartDate && a.DateCreated <= EndDate).ToList();
            else
                SafeList = db.Safe.ToList();

                foreach (var i in SafeList)
                {
                    if (i.TransactionType.Contains("ايداع"))
                        Balance += i.Deposit;
                    else if (i.TransactionType.Contains("سحب"))
                        Balance -= i.Withdraw;
                }
            ViewBag.Balance = Balance;
            
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
            return View();
        }

        // POST: Safes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SafeId,PermessionNumber,DateCreated,TransactionType,Deposit,Withdraw,Notes")] Safe safe)
        {
            if (ModelState.IsValid)
            {
                db.Safe.Add(safe);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

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
            return View(safe);
        }

        // POST: Safes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SafeId,PermessionNumber,DateCreated,TransactionType,Deposit,Withdraw,Notes")] Safe safe)
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
            return View(safe);
        }

        // POST: Safes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Safe safe = db.Safe.Find(id);
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
