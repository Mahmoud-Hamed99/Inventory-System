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
            
            AddOpenning();
            ////////////InventoryDB db = new InventoryDB();
            ////////////foreach (var v in db.Items)
            ////////////{
            ////////////    v.ItemAvgPrice = v.ItemQuantity;
            ////////////    v.ItemQuantity = 0;
            ////////////}
            ////////////db.SaveChanges();
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
                        ItemSubCategoryName = fi.Name.Substring(0, fi.Name.LastIndexOf("."))
                    };
                    db.ItemSubCategories.Add(sub);
                    db.SaveChanges();
                    string[] lines = System.IO.File.ReadAllLines(fi.FullName);
                    foreach (var v in lines)
                    {
                        if (string.IsNullOrEmpty(v) == false)
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
                                            ItemQuantity = 0,
                                            ItemMinQuantity = 0,
                                            ItemName = line[0] + line[1],
                                            ItemAvgPrice = line.Length < 3 ? 0 : double.Parse(String.IsNullOrEmpty(line[3]) ? "0" : line[3]),
                                            ItemSubCategoryId = sub.ItemSubCategoryId,
                                            ItemUnit = line[2]
                                        });
                                    }
                                    else
                                    {
                                        db.Items.Add(new Item()
                                        {
                                            DateCreated = DateTime.Now.AddDays(-3),
                                            ItemQuantity = 0,
                                            ItemMinQuantity = 0,
                                            ItemName = line[0],
                                            ItemAvgPrice = line.Length < 3 ? 0 : double.Parse(String.IsNullOrEmpty(line[2]) ? "0" : line[2]),
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
            foreach (var v in lines)
            {
                if (string.IsNullOrEmpty(v))
                    continue;
                string[] vals = v.Split(',');
                string cmp = vals[1];
                var cats = db.SafeCategories.Where(a => a.Name == cmp).ToList();
                if (cats.Count() == 0)
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

        void AddOpenning()
        {

            if (System.IO.File.Exists(Server.MapPath(@"رصيد8-5-2021.csv"/*@"C:\Users\mahmo\Downloads\رصيد 20-4-2021 (1).csv"*/)) == false)
                return;
            InventoryDB db = new InventoryDB();
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"رصيد8-5-2021.csv"/*@"C:\Users\mahmo\Downloads\رصيد 20-4-2021 (1).csv"*/));
            foreach (var v in lines)
            {
                if (string.IsNullOrEmpty(v))
                    continue;
                string[] vals = v.Split(',');
                string nm = vals[0];
                string cmp = vals[1];
                if (vals.Count()>2)
                {
                    for(int i = 0;i<vals.Count()-1;i++)
                    {
                        nm += vals[i];
                    }
                    cmp = vals[vals.Count() - 1];
                }
                
                
                if (double.Parse(cmp) > 0)
                {
                    foreach (var vv in db.Items.Where(a => a.ItemName == nm).ToList())
                    {

                        //db.ItemInputs.Add(new ItemInput()
                        //{
                        //    ItemId = vv.ItemId,
                        //    ItemPrice = vv.ItemAvgPrice,
                        //    ItemQuantity = double.Parse(cmp),
                        //    ItemTotalCost = double.Parse(cmp) * vv.ItemAvgPrice,

                        //});
                        addItem(new ItemInput()
                        {
                            ItemId = vv.ItemId,
                            ItemPrice = vv.ItemAvgPrice,
                            ItemQuantity = double.Parse(cmp),
                            ItemTotalCost = double.Parse(cmp) * vv.ItemAvgPrice,

                        }, db);
                    }
                }
                

            }
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath(@"رصيد8-5-2021.csv"));
        }
        void addItem(ItemInput itemInput,InventoryDB db)
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
            foreach (var v in db.DemandItems.Where(a => a.DemandItemApproval && a.PurchasingApproval && a.PurchasedItemQuantity > 0))
            {
                if (itemQ > 0)
                {
                    if (v.PurchasedItemQuantity >= itemQ)
                    {
                        v.PurchasedItemQuantity -= itemQ;
                        itemQ = 0;

                    }
                    else
                    {
                        itemQ -= v.PurchasedItemQuantity;
                        v.PurchasedItemQuantity = 0;
                    }
                    if (v.PurchasedItemQuantity == 0)
                    {
                        Helper.AddNotification(db,
                            "الخامة متوفرة",
                            "الخامة " + x.ItemName + " اصبحت متوفرة",
                            db.Users.Where(a => a.Roles == "projectplanning").ToList());
                    }
                }

            }
            db.SaveChanges();
        }
    }
}