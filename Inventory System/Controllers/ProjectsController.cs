﻿using System;
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
    [VerifyUser(Roles = "projectplanning")]
    public class ProjectsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        //int pageSize = 2;
        // GET: Projects
        public ActionResult Index()
        {
           // int pageNumber = (Page ?? 1);
            return View(
                db.Projects
                
                .Where(a=>a.ProjectFinished == false).ToList());
        }

        
        [HttpPost]
        ActionResult ReturnIndex()
        {

           // int pageNumber = (Page ?? 1);
            var projects = db.Projects.Where(a => a.ProjectFinished == false);
           
            return View("Index", projects.ToList());
        }
        [HttpPost]
        public ActionResult ProjectFinishedFun(int[] ProjectFinishedList )
        {
            if (ProjectFinishedList != null)
            {
                foreach (var v in ProjectFinishedList)
                {
                    var res = db.Projects.Find(v);
                        if (res != null)
                        {
                            res.ProjectFinished = true;
                            
                        }
                    Helper.AddLog(db, "Set project finished", v, "Projects", this);
                }
                db.SaveChanges();
            }
            return ReturnIndex();
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectName,ProjectCode,DateCreated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                Helper.AddLog(db, "Created Project", project.ProjectId, "Projects", this);
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            //ViewBag.itemOutputs = db.ItemOutputs.Include(a => a.ProjectId == id).ToList();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectName,ProjectCode,DateCreated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                Helper.AddLog(db, "Edited Project", project.ProjectId, "Projects", this);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            Helper.AddLog(db, "Deleted Project", project.ProjectId, "Projects", this);
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
