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
    public class BankAccountsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        // GET: BankAccounts
        public ActionResult Index()
        {
            ViewBag.BankName = new SelectList(db.BankAccountants.GroupBy(a=>a.BankName)).Distinct().ToList();
            return View(db.BankAccountants.ToList());
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccountants.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,BankName,DateCreated,TransitionNumber,Statement,TransitionType,Deposit,Withdraw,Balance")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                // update balance in everey transaction operation.
                if (db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)).ToList().Count != 0)
                {
                    double allDeposit = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)).Sum(a => a.Deposit);
                    double allWithdraw = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)).Sum(a => a.Withdraw);
                   if(bankAccount.Withdraw==0.0)
                    bankAccount.Balance = allDeposit - allWithdraw + bankAccount.Deposit ;
                   else
                        bankAccount.Balance = allDeposit - allWithdraw - bankAccount.Withdraw;

                }
                else // in first time (no bank account exist)
                    bankAccount.Balance = bankAccount.Deposit;

                    db.BankAccountants.Add(bankAccount);
                    db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(bankAccount);
        }

        public double oldDeposit = 0.0;
        public double oldWithdraw = 0.0;
        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccountants.Find(id);
            oldDeposit = bankAccount.Deposit;
            oldWithdraw = bankAccount.Withdraw;
            ViewBag.oldDeposit = oldDeposit;
            ViewBag.oldWithdraw = oldWithdraw;  
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankAccountId,BankName,DateCreated,TransitionNumber,Statement,TransitionType,Deposit,Withdraw,Balance")] BankAccount bankAccount,double oldDeposit, double oldWithdraw)
        {
            if (ModelState.IsValid)
            {

   //             var old = oldDeposit;
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();

                if (bankAccount.Withdraw == 0.0) // make deposit
                {                    
                    var bankAccList = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName) && a.BankAccountId >bankAccount.BankAccountId);
                    if(oldDeposit==0.0)
                    { 
                        bankAccount.Balance += oldWithdraw;
                        bankAccount.Balance += bankAccount.Deposit;
                    }
                    else
                    {
                        bankAccount.Balance -= oldDeposit;
                        bankAccount.Balance += bankAccount.Deposit;
                    }
                    foreach (var x in bankAccList)
                    {
                        if (oldDeposit == 0.0)
                        {
                            x.Balance += oldWithdraw;
                            x.Balance += bankAccount.Deposit;
                        }
                        else
                        {
                            x.Balance -= oldDeposit;
                            x.Balance += bankAccount.Deposit;
                        }
                    }
                }
                else // make withdraw
                {
                    
                    var bankAccList = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)&& a.BankAccountId >bankAccount.BankAccountId);
 
                    if(oldDeposit!=0.0)
                    {
                        bankAccount.Balance -= oldDeposit;
                        bankAccount.Balance -= bankAccount.Withdraw;
                    }
                    else
                    {
                        bankAccount.Balance += oldWithdraw;
                        bankAccount.Balance -= bankAccount.Withdraw;
                    }


                    foreach (var x in bankAccList)
                    {
                        if (oldDeposit != 0.0)
                        {
                            x.Balance -= oldDeposit; // back balance before old withdraw .
                            x.Balance -= bankAccount.Withdraw;
                        }
                        else
                        {
                            x.Balance += oldWithdraw; // back balance before old withdraw .
                            x.Balance -= bankAccount.Withdraw;
                        }

                       
                    }
                }



                //db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccountants.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccountants.Find(id);
            db.BankAccountants.Remove(bankAccount);
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
