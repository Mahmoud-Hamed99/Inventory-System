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
    public class BankAccountsController : Controller
    {
        private InventoryDB db = new InventoryDB();

        int pageSize = 20;

        public ActionResult Index(string BankName , int? year , int? month1,int? month2 ,string CurrentFilter , int? page)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            if(year!=null && month1!=null&& month2!=null)
            {
                StartDate = new DateTime(year.Value, month1.Value, 1);
                if(month2.Value>month1.Value)
                {
                    EndDate = new DateTime(year.Value, month2.Value, 1);
                    EndDate = EndDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    EndDate = StartDate.AddMonths(1);
                }
            }
            //var BankNameList = db.BankAccountants.GroupBy(a => a.BankName).ToList();
            List<string> banks = new List<string>();
            banks.Add("الاهلي");
            banks.Add("كريدي أجريكول");
            ViewBag.BankName = new SelectList(banks);


            ViewBag.CurrentFilter = BankName;
            int pageNumber = (page ?? 1);

            // with every search .. show detailed bank account in detected period.

            if (!String.IsNullOrEmpty(BankName) && year!=null && month1!=null && month2!=null)
            {
                List<BankAccount> items = db.BankAccountants.Where(a => a.BankName.Contains(BankName) && a.DateCreated >= StartDate && a.DateCreated <= EndDate).ToList();
                ViewBag.CurrentFilter = BankName;
                ViewBag.startDate = StartDate;
                ViewBag.endDate = EndDate;
                return Calc(items, BankName, StartDate, EndDate, CurrentFilter, page, pageNumber);
            }

            else if (String.IsNullOrEmpty(BankName) && year != null && month1 != null && month2 != null)
            {
                List<BankAccount> items = db.BankAccountants.Where(a => a.DateCreated >= StartDate && a.DateCreated <= EndDate).ToList();
                ViewBag.BankName = new SelectList(banks);
                return View(items.ToPagedList(pageNumber,pageSize));
            }

            else if (!String.IsNullOrEmpty(BankName))
            {
                List<BankAccount> items = db.BankAccountants.Where(a => a.BankName.Contains(BankName)).ToList();
                ViewBag.BankName = new SelectList(banks);
                return View(items.ToPagedList(pageNumber,pageSize));
            }

            else
            {
                   return View(db.BankAccountants.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
            }
           

        }

        public ActionResult Calc(List<BankAccount> items,string BankName, DateTime? StartDate, DateTime? EndDate ,string CurrentFilter , int? page ,int pageNumber)
        {
            if(StartDate != null && EndDate != null)
            { 
                List<BankAccount> BankAccountsList = db.BankAccountants.Where(
                a => a.CheckIsPaied == false && a.BankName.Contains(BankName) &&
                a.DateCreated >= StartDate && a.DateCreated <= EndDate).ToList();

                double deposits = 0.0;
                double withdraw = 0.0;
                double FirstBalance = 0.0;
                double FinalBalance = 0.0;
                double DepositChecks = 0.0;
                double WithdrawChecks = 0.0;

                if (items.Count() > 0)
                {
                    if (items.FirstOrDefault().TransitionType.Contains("ايداع")) // check transaction to cal balance before this transaction.
                        FirstBalance = items.FirstOrDefault().Balance - items.FirstOrDefault().Deposit;
                    else
                        FirstBalance = items.FirstOrDefault().Balance + items.FirstOrDefault().Withdraw;

                    foreach (var i in items)
                    {
                        if (i.CheckIsPaied == true) // get مقبوضات ومدفوعات  thats mean i received money in my hand.
                        {
                            deposits += i.Deposit;
                            withdraw += i.Withdraw;
                        }
                    }
                    ViewBag.AllDeposit = deposits;
                    ViewBag.AllWithdraw = withdraw;
                    ViewBag.FirstBalance = FirstBalance;
                    FinalBalance = FirstBalance + deposits - withdraw;
                    ViewBag.FinalBalance = FinalBalance;
                }

                foreach (var x in BankAccountsList)
                {
                    if (x.TransitionType.Contains("ايداع")) // شيكات صرفت لدي البنك ولم تصرف لدينا
                        DepositChecks += x.Deposit;
                    else if (x.TransitionType.Contains("سحب")) // شيكات صرفت لدينا ولم تصرف لدي البنك
                        WithdrawChecks += x.Withdraw;
                }
                    ViewBag.DepositChecks = DepositChecks;
                    ViewBag.WithdrawChecks = WithdrawChecks;
                    ViewBag.ActualBalance = FinalBalance + DepositChecks - WithdrawChecks;
                
           }
              return View(items.OrderBy(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
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
            List<string> banks = new List<string>();
            banks.Add("الاهلي");
            banks.Add("كريدي أجريكول");
            var sl = new SelectList(banks);
            foreach (var v in sl)
            {
                if (v.Value == "الاهلي")
                    v.Selected = true;
            }
            ViewBag.BankName = sl;
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,BankName,DateCreated,TransitionNumber,Statement,TransitionType,Deposit,Withdraw,Balance,CheckIsPaied")] BankAccount bankAccount)
        {
            List<string> banks = new List<string>();
            banks.Add("الاهلي");
            banks.Add("كريدي أجريكول");
            var sl = new SelectList(banks);
            foreach(var v in sl)
            {
                if(v.Value == "الاهلي")
                    v.Selected = true;
            }
            ViewBag.BankName = sl;
            if (ModelState.IsValid)
            {
                double allDeposit, allWithdraw = 0.0;
                // update balance in everey transaction operation.
                if (db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)).ToList().Count != 0)
                {
                     allDeposit = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)  
                    && a.DateCreated <bankAccount.DateCreated).ToList().Sum(a => a.Deposit);
           
                     allWithdraw = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName) 
                     && a.DateCreated <bankAccount.DateCreated).ToList().Sum(a => a.Withdraw);
               
                    if(bankAccount.Withdraw==0.0) // makind deposit
                        bankAccount.Balance = allDeposit - allWithdraw + bankAccount.Deposit ;
                   else // making withdraw
                        bankAccount.Balance = allDeposit - allWithdraw - bankAccount.Withdraw;

                    // if check is created by old date , we should re-calc banck acc transactions after this old date.  
                    List<BankAccount> updatedList = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName) 
                    && a.DateCreated> bankAccount.DateCreated).ToList(); 
                     
                       
                    foreach(var y in updatedList)
                    {
                        if (bankAccount.Deposit == 0) //making withdraw in this check with old date.
                            y.Balance -= bankAccount.Withdraw;
                        else 
                            y.Balance += bankAccount.Deposit;
                    }

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
            List<string> banks = new List<string>();
            banks.Add("الاهلي");
            banks.Add("كريدي أجريكول");
            var sl = new SelectList(banks);
            foreach (var v in sl)
            {
                if (v.Text == bankAccount.BankName)
                    v.Selected = true;
            }
            ViewBag.BankName = sl;
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
        public ActionResult Edit([Bind(Include = "BankAccountId,BankName,DateCreated,TransitionNumber,Statement,TransitionType,Deposit,Withdraw,Balance,CheckIsPaied")] BankAccount bankAccount,double oldDeposit, double oldWithdraw)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();


                var bankAccList = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)
                && a.DateCreated > bankAccount.DateCreated);

                if (bankAccount.Withdraw == 0.0) // make deposit in this edit operation
                {                    
                    
                    if(oldDeposit==0.0) // if old transaction was withdraw .
                    { 
                        bankAccount.Balance += oldWithdraw;
                        bankAccount.Balance += bankAccount.Deposit;
                    }
                    else // if old transaction was deposit
                    {
                        bankAccount.Balance -= oldDeposit;
                        bankAccount.Balance += bankAccount.Deposit;
                    }

                    foreach (var x in bankAccList) // update balance in all transaction which created after this check edit.
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
                else // make withdraw in this edit operation
                {
                    
                    if(oldDeposit !=0.0)   // if old transaction was deposit.
                    {
                        bankAccount.Balance -= oldDeposit;
                        bankAccount.Balance -= bankAccount.Withdraw;
                    }
                    else   // if old transaction was withdraw .
                    {
                        bankAccount.Balance += oldWithdraw;
                        bankAccount.Balance -= bankAccount.Withdraw;
                    }


                    foreach (var x in bankAccList) // update balance in all transaction which created after this check edit.
                    {
                        if (oldDeposit != 0.0)
                        {
                            x.Balance -= oldDeposit; // back balance before old deposit .
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
            List<string> banks = new List<string>();
            banks.Add("الاهلي");
            banks.Add("كريدي أجريكول");
            var sl = new SelectList(banks);
            foreach (var v in sl)
            {
                if (v.Text == bankAccount.BankName)
                    v.Selected = true;
            }
            ViewBag.BankName = sl;
            return View(bankAccount);
        }

        double deletedDeposit = 0.0;
        double deletedWithdraw = 0.0;
        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccountants.Find(id);
            deletedDeposit = bankAccount.Deposit;
            deletedWithdraw = bankAccount.Withdraw;
            ViewBag.deletedDeposit = deletedDeposit;
            ViewBag.deletedWithdraw = deletedWithdraw;

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
        
            var bankAccList = db.BankAccountants.Where(a => a.BankName.Contains(bankAccount.BankName)
               && a.DateCreated > bankAccount.DateCreated);

            if (bankAccount.Withdraw == 0.0) // this operation was deposit
            {
                foreach (var x in bankAccList) // update balance in all transaction which created after this check remove.
                {
                        x.Balance -= bankAccount.Deposit;
                }
            }

            else //  this operation was withdraw
            {
                foreach (var x in bankAccList) // update balance in all transaction which created after this check remove.
                {
                     x.Balance += bankAccount.Withdraw; // back balance before old withdraw .
                }
            }
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
