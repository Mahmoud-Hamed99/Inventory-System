using Inventory_System;
using Inventory_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace helper.Classes
{
    public interface TableWithDate
    {
        
        DateTime DateCreated { get; set; }
    }
    public interface TableWithDepositWithdraw:TableWithDate
    {
        double Deposit { get; set; }

        double Withdraw { get; set; }
    }
    public class FinanceStatement {
        public double StartingBalance { get; set; }
        public double EndBalance { get; set; }
        public double CurrentBalance { get; set; }
        public double Deposits { get; set; }
        public double Withdraws { get; set; }
        public double Diff { get; set; }
    }
    public class Helper
    {
        static public void AddLog(InventoryDB db,string action,int rowId,string tableName, ControllerBase controller)
        {
            try
            {
                db.UserLogs.Add(new UserLog()
                {
                    Action = action,
                    DateCreated = DateTime.Now,
                    RowId = rowId,
                    TableName = tableName,
                    UserId = (controller.ViewBag.mainUser as User).Id,
                    Username = (controller.ViewBag.mainUser as User).username
                });
                db.SaveChanges();
            }
            catch
            {

            }
            
        }

        static public void AddNotification(InventoryDB db, string NotificationName, string NotificationText, List<User> users)
        {
            try
            {
                foreach (var v in users)
                {
                    db.Notifications.Add(new Notification()
                    {
                        DateCreated = DateTime.Now,
                        NotificationName = NotificationName,
                        NotificationText = NotificationText,
                        UserId = v.Id
                    });
                }
                db.SaveChanges();
            }
            catch
            {

            }
            
            
        }
        static public List<T> FilterByDate<T>(string fromDateString,string toDateString, 
            IQueryable<TableWithDate> data)
        {
            if(fromDateString!=null && toDateString!=null)
            {
                DateTime fromDT = DateTime.ParseExact(fromDateString,
                    "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime toDT = DateTime.ParseExact(toDateString,
                    "dd-MM-yyyy", CultureInfo.InvariantCulture);
                toDT = toDT.AddDays(1);
                return data.Where(a => a.DateCreated >= fromDT && a.DateCreated <= toDT).
                    ToList().ConvertAll(Convert<T>);
            }
            else
            {
                return data.ToList().ConvertAll(Convert<T>); ;
            }
            
        }
        
        static public FinanceStatement DoCalculation(IQueryable<TableWithDepositWithdraw> data, string fromDateString, string toDateString)
        {
            double firstBalance = 0.0;
            double lastBalance = 0.0;
            double currentBalance = 0.0;
            double deposits = 0.0;
            double withdraws = 0.0;
            try
            {
                currentBalance = data.Sum(a => a.Deposit) - data.Sum(a => a.Withdraw);
            }
            catch
            {
                currentBalance = 0;
            }
            
            if (fromDateString != null && toDateString != null)
            {
                DateTime fromDT = DateTime.ParseExact(fromDateString,
                    "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var dt = data.Where(a => a.DateCreated < fromDT);
                firstBalance = dt.Sum(a => a.Deposit) - dt.Sum(a => a.Withdraw);
                
            }
            var filteredData = 
                FilterByDate<TableWithDepositWithdraw>(fromDateString, toDateString, data);
            deposits = filteredData.Sum(a => a.Deposit);
            withdraws = filteredData.Sum(a => a.Withdraw);
            lastBalance = (firstBalance + deposits) - withdraws;


            return new FinanceStatement()
            {
                CurrentBalance = currentBalance,
                Deposits = deposits,
                EndBalance = lastBalance,
                StartingBalance = firstBalance,
                Withdraws = withdraws,
                Diff = deposits - withdraws
            };

        }
        static public T Convert<T>(object dt)
        {
            return (T)dt;
        }
        static public bool CheckUser(HttpContextBase context, InventoryDB dbcontext,out User user)
        {
            bool res = false;
            bool fndUsername = false;
            bool fndPassword = false;

            user = null;
            foreach (var v in context.Request.Cookies.Keys)
            {
                if (v.ToString() == "username")
                {
                    fndUsername = true;
                }
                else if (v.ToString() == "password")
                {
                    fndPassword = true;
                }
            }

            if (fndUsername == false || fndPassword == false)
            {
                
                return false;
            }
            string username = context.Request.Cookies["username"].Value;
            string password = context.Request.Cookies["password"].Value;
            var coll = dbcontext.Users.Where(a => a.username == username.ToLower() && a.Password == password).ToList();
            res = coll.Count() > 0;
            user = coll.First();
            return res;
        }
        static public bool CheckUser(HttpContextBase context,out User user)
        {
            InventoryDB dbcontext = new InventoryDB();
            return CheckUser(context, dbcontext,out user);
        }
        static public void Logout(HttpContextBase context)
        {
            string[] myCookies = context.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                context.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

        }
        static public bool Login(HttpContextBase context, InventoryDB dbcontext, string username, string password,Controller controller=null)
        {
            Logout(context);
            bool res = false;



            var resAll = dbcontext.Users.Where(a => a.username == username.ToLower() && a.Password == password).ToList();
            res = resAll.Count > 0;
            if (res)
            {
                if (controller != null)
                {
                    controller.ViewBag.mainUser = resAll.First();
                    var u = resAll.First();
                    controller.ViewBag.notifications = dbcontext.Notifications.Where(a => a.UserId == u.Id).ToList();
                }
                var c1 = new HttpCookie("username", username);
                c1.Expires = DateTime.Now.AddYears(1);
                context.Response.Cookies.Add(c1);
                var c2 = new HttpCookie("password", password);
                c2.Expires = DateTime.Now.AddYears(1);
                context.Response.Cookies.Add(c2);
                

            }
            return res;
        }
    }
    public class VerifyUserAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user;
            var found = Helper.CheckUser(filterContext.HttpContext, out user);
            filterContext.Controller.ViewBag.mainUser = user;
            if (user != null)
            {
                var db = new InventoryDB();
                filterContext.Controller.ViewBag.notifications = db.Notifications.Where(a => a.UserId == user.Id).ToList();
            }
            if (!found)
            {
                filterContext.Result = new RedirectResult("~/Login?targetUrl=" + filterContext.HttpContext.Request.Url.AbsolutePath);


            }
            else if(string.IsNullOrEmpty(Roles) == false)
            {
                var foundRole = false;
                foreach(var v in Roles.Split(','))
                {
                    foreach(var vv in user.Roles.Split(','))
                    {
                        if(vv == v)
                        {
                            foundRole = true;
                            break;
                        }
                    }
                    if (foundRole)
                        break;
                }
                if(!foundRole)
                {
                    filterContext.Result = new RedirectResult("~/Login?targetUrl=" + filterContext.HttpContext.Request.Url.AbsolutePath);
                }
            }

        }
    }
}