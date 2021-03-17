using Inventory_System;
using Inventory_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace helper.Classes
{
    public class Helper
    {
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
        static public bool Login(HttpContextBase context, InventoryDB dbcontext, string username, string password)
        {
            Logout(context);
            bool res = false;



            res = dbcontext.Users.Where(a => a.username == username.ToLower() && a.Password == password).Count() > 0;
            if (res)
            {
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