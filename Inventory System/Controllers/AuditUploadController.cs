using helper.Classes;
using Inventory_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_System.Controllers
{
    public class AuditUploadController : Controller
    {
        public class submitModel
        {
            [DataType(DataType.MultilineText)]
            public string DocNumber { get; set; }
            [DataType(DataType.MultilineText)]
            public string ItemName { get; set; }
            [DataType(DataType.MultilineText)]
            public string Quantities { get; set; }
        }

        public class submitModelOut
        {
            [DataType(DataType.MultilineText)]
            public string DocNumber { get; set; }
            [DataType(DataType.MultilineText)]
            public string ItemName { get; set; }
            [DataType(DataType.MultilineText)]
            public string Quantities { get; set; }
            [DataType(DataType.MultilineText)]
            public string ProjectCode { get; set; }
        }


        private InventoryDB db = new InventoryDB();

        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult IndexOut()
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            List<string[]> incorrectItems = new List<string[]>();
            ViewBag.incorrectItems = incorrectItems;
            return View();
        }
        [HttpPost]
        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult IndexOut(submitModelOut model)
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            string[] itemNames = model.ItemName.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] docNumbers = model.DocNumber.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] Quantities = model.Quantities.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] ProjectCodes = model.ProjectCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (itemNames.Count() != docNumbers.Count() || itemNames.Count() != Quantities.Count() || itemNames.Count() != ProjectCodes.Count())
            {
                ViewBag.Message = "عدد السجلات غير متطابق بين كل خانة";
                return View();
            }
            List<string[]> incorrectItems = new List<string[]>();
            for (int i = 0; i < itemNames.Count(); i++)
            {
                string _itemName = itemNames[i];
                string _projectCode = ProjectCodes[i];
                if (string.IsNullOrEmpty(_itemName))
                    continue;
                int cnt = db.Items.Where(a => a.ItemName == _itemName).Count();
                int pcnt = db.Projects.Where(a => a.ProjectCode == _projectCode).Count();
                if (cnt == 0)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "الإسم غير موجود" });
                }
                else if (cnt > 1)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "يوجد أكثر من خامة بنفس الاسم" });
                }
                else if (pcnt == 0)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "كود المشروع غير موجود" });
                }
                else if(pcnt > 1)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "يوجد أكثر من مشروع بنفس الكود" });
                }
                else
                {
                    double qnt = 0;
                    int docNumber = 0;
                    if (double.TryParse(Quantities[i], out qnt) == false)
                    {
                        incorrectItems.Add(new string[] { itemNames[i], "الكميه غير صحيحة" });
                    }
                    else if (int.TryParse(docNumbers[i], out docNumber) == false)
                    {
                        incorrectItems.Add(new string[] { itemNames[i], "رقم ملف غير صحيح" });
                    }

                }
            }
            if (incorrectItems.Count == 0)
            {
                for (int i = 0; i < itemNames.Count(); i++)
                {
                    string _itemName = itemNames[i];
                    string _projectCode = ProjectCodes[i];
                    if (string.IsNullOrEmpty(_itemName))
                        continue;
                    var item = db.Items.Single(a => a.ItemName == _itemName);
                    var pr = db.Projects.Single(a => a.ProjectCode == _projectCode);

                    double qnt = double.Parse(Quantities[i]);
                    int doc = int.Parse(docNumbers[i]);
                    ItemOutput io = new ItemOutput()
                    {
                        DocCode = doc,
                        ItemId = item.ItemId,
                        ProjectId = pr.ProjectId,
                        ItemOutputQuantity = qnt
                    };
                    
                    db.ItemOutputs.Add(io);
                }
                db.SaveChanges();
                ViewBag.Message = "تمت الاضافة";
            }
            ViewBag.incorrectItems = incorrectItems;
            return View();
        }
        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult ConfirmOut()
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            return View();
        }


        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult Index()
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            List<string[]> incorrectItems = new List<string[]>();
            ViewBag.incorrectItems = incorrectItems;
            return View();
        }
        [HttpPost]
        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult Index(submitModel model)
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            string[] itemNames = model.ItemName.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] docNumbers = model.DocNumber.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] Quantities = model.Quantities.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if(itemNames.Count()!=docNumbers.Count() || itemNames.Count()!=Quantities.Count())
            {
                ViewBag.Message = "عدد السجلات غير متطابق بين كل خانة";
                return View();
            }
            List<string[]> incorrectItems = new List<string[]>();
            for (int i = 0; i < itemNames.Count(); i++)
            {
                string _itemName = itemNames[i];
                if (string.IsNullOrEmpty(_itemName))
                    continue;
                int cnt = db.Items.Where(a => a.ItemName == _itemName).Count();
                if (cnt == 0)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "الإسم غير موجود" });
                }
                else if(cnt>1)
                {
                    incorrectItems.Add(new string[] { itemNames[i], "يوجد أكثر من خامة بنفس الاسم" });
                }
                else
                {
                    double qnt = 0;
                    int docNumber = 0;
                    if(double.TryParse(Quantities[i],out qnt) == false)
                    {
                        incorrectItems.Add(new string[] { itemNames[i], "الكميه غير صحيحة" });
                    }
                    else if(int.TryParse(docNumbers[i],out docNumber) == false)
                    {
                        incorrectItems.Add(new string[] { itemNames[i], "رقم ملف غير صحيح" });
                    }
                }
            }
            if(incorrectItems.Count == 0)
            {
                for (int i = 0; i < itemNames.Count(); i++)
                {
                    string _itemName = itemNames[i];
                    if (string.IsNullOrEmpty(_itemName))
                        continue;
                    var item = db.Items.Single(a => a.ItemName == _itemName);
                    double qnt = double.Parse(Quantities[i]);
                    int doc = int.Parse(docNumbers[i]);
                    ItemInput ii = new ItemInput()
                    {
                        DocCode = doc,
                        ItemId = item.ItemId,
                        ItemQuantity = qnt
                    };
                    db.ItemInputs.Add(ii);
                }
                db.SaveChanges();
                ViewBag.Message = "تمت الاضافة";
            }
            ViewBag.incorrectItems = incorrectItems;
            return View();
        }
        [VerifyUser(Roles = "warehouseaudit")]
        public ActionResult Confirm()
        {
            User user;
            Helper.CheckUser(HttpContext, db, out user);
            return View();
        }
    }
}