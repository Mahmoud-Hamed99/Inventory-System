using helper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Inventory_System.Models;

namespace Inventory_System.Controllers
{
    public class HomeController : Controller
    {
        //[VerifyUser(Roles ="superadmin")]
        // GET: Home
        public ActionResult Index()
        {
            //AddSafe();
            ////////AddItems();
            //Models.User mainUser = new Models.User()
            //{
            //    Fullname = "safe",
            //    Password = "123456",
            //    Roles = "safe",
            //    username = "safe"
            //};
            //db.Users.Add(mainUser);
            //db.SaveChanges();
            return View();
        }
        void AddItems()
        {
            if (Directory.Exists(@"C:\Users\mahmo\Downloads\data system") == false)
                return;
            InventoryDB db = new InventoryDB();
            foreach (DirectoryInfo di in new DirectoryInfo(@"C:\Users\mahmo\Downloads\data system").GetDirectories())
            {
                ItemCategory cat = new ItemCategory()
                {
                    DateCreated = DateTime.Now.AddDays(-3),
                    ItemCategoryName = di.Name
                };

                db.ItemCategories.Add(cat);
                db.SaveChanges();
                foreach (FileInfo fi in di.GetFiles())
                {
                    var sub = new ItemSubCategory()
                    {
                        ItemCategoryId = cat.ItemCategoryId,
                        ItemSubCategoryName = fi.Name.Substring(0,fi.Name.LastIndexOf("."))
                    };
                    db.ItemSubCategories.Add(sub);
                    db.SaveChanges();
                    string[] lines = System.IO.File.ReadAllLines(fi.FullName);
                    foreach(var v in lines)
                    {
                        if(string.IsNullOrEmpty(v) == false)
                        {
                            var line = v.Split(',');
                            if (string.IsNullOrEmpty(line[0]) == false)
                            {
                                try
                                {
                                    if (line.Length > 3)
                                    {
                                        db.Items.Add(new Item()
                                        {
                                            DateCreated = DateTime.Now.AddDays(-3),
                                            ItemAvgPrice = 0,
                                            ItemMinQuantity = 0,
                                            ItemName = line[0] + line[1],
                                            ItemQuantity = line.Length < 3 ? 0 : double.Parse(String.IsNullOrEmpty(line[3]) ? "0" : line[3]),
                                            ItemSubCategoryId = sub.ItemSubCategoryId,
                                            ItemUnit = line[2]
                                        });
                                    }
                                    else
                                    {
                                        db.Items.Add(new Item()
                                        {
                                            DateCreated = DateTime.Now.AddDays(-3),
                                            ItemAvgPrice = 0,
                                            ItemMinQuantity = 0,
                                            ItemName = line[0],
                                            ItemQuantity = line.Length < 3 ? 0 : double.Parse(String.IsNullOrEmpty(line[2]) ? "0" : line[2]),
                                            ItemSubCategoryId = sub.ItemSubCategoryId,
                                            ItemUnit = line[1]
                                        });
                                    }

                                    db.SaveChanges();
                                }
                                catch
                                {

                                }
                                
                            }
                        }
                    }
                    
                }
            }
        }

        void AddSafe()
        {
            if (System.IO.File.Exists(@"C:\Users\mahmo\Downloads\الخزينة\نوع المصروف والايراد وتوجية.csv") == false)
                return;
            InventoryDB db = new InventoryDB();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\mahmo\Downloads\الخزينة\نوع المصروف والايراد وتوجية.csv");
            foreach(var v in lines)
            {
                if (string.IsNullOrEmpty(v))
                    continue;
                string[] vals = v.Split(',');
                string cmp = vals[1];
                var cats = db.SafeCategories.Where(a => a.Name == cmp).ToList();
                if (cats.Count()==0)
                {
                    var cat = new SafeCategory()
                    {
                        DateCreated = DateTime.Now.AddDays(-3),
                        Name = vals[1]
                    };
                    db.SafeCategories.Add(cat);
                    db.SaveChanges();
                    
                }
                else
                {
                    db.safeSubCategories.Add(new SafeSubCategory()
                    {
                        DateCreated = DateTime.Now.AddDays(-3),
                        Name = vals[0],
                        SafeCategoryId = cats.First().SafeCategoryId
                    });
                    db.SaveChanges();

                }
            }
        }
    }
}