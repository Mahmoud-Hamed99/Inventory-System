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
    public class TechnicalDepartmentsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int PageSize = 20; 
        // GET: TechnicalDepartments
        public ActionResult Index(int? Page)
        {
            int pageNumber = (Page ?? 1);
            return View(db.TechnicalDepartments.OrderBy(a=>a.TechnicalDepartmentId).ToPagedList(pageNumber,PageSize));
        }

        // GET: TechnicalDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalDepartment technicalDepartment = db.TechnicalDepartments.Find(id);
            if (technicalDepartment == null)
            {
                return HttpNotFound();
            }
            return View(technicalDepartment);
        }

        // GET: TechnicalDepartments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TechnicalDepartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TechnicalDepartmentId,TechnicalDepartmentName")] TechnicalDepartment technicalDepartment)
        {
            if (ModelState.IsValid)
            {
                db.TechnicalDepartments.Add(technicalDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(technicalDepartment);
        }

        // GET: TechnicalDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalDepartment technicalDepartment = db.TechnicalDepartments.Find(id);
            if (technicalDepartment == null)
            {
                return HttpNotFound();
            }
            return View(technicalDepartment);
        }

        // POST: TechnicalDepartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TechnicalDepartmentId,TechnicalDepartmentName")] TechnicalDepartment technicalDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(technicalDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(technicalDepartment);
        }

        // GET: TechnicalDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalDepartment technicalDepartment = db.TechnicalDepartments.Find(id);
            if (technicalDepartment == null)
            {
                return HttpNotFound();
            }
            return View(technicalDepartment);
        }

        // POST: TechnicalDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TechnicalDepartment technicalDepartment = db.TechnicalDepartments.Find(id);
            db.TechnicalDepartments.Remove(technicalDepartment);
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
