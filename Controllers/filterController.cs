using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Data.Common;
using System.Security.Cryptography;

namespace be.Controllers
{
    public class filterController : Controller
    {
        BentContext db = new BentContext();
        public IActionResult ss()
        {
            return View();
        }
        public IActionResult ss1(DateTime date1, DateTime date2)
        {
            var cus = db.Secondarysales.Where(x => (x.date >= date1 && x.date <= date2) && ((x.Type == "Drum" && x.Status != 0) || x.Type == "Druml")).ToList();
            var t_pr = 0;
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item in cus)
            {
                var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PId)).First();
                t_pr = t_pr + Convert.ToInt32(item.TPrice);
                t_ltr = Convert.ToInt32(t_ltr + (Convert.ToInt32(item.Pack) * item.SsQty));
                tqt = tqt + Convert.ToInt32(item.SsQty);
            }
            ViewBag.tpr = t_pr;
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            ViewBag.d1 = date1.Day + "-" + date1.Month + "-" + date1.Year;
            ViewBag.d2 = date2.Day + "-" + date2.Month + "-" + date2.Year;
            return View(cus);
        }
        public IActionResult ps()
        {
            return View();
        }
        public IActionResult ps1(DateTime date1, DateTime date2)
        {
            var cus = db.Primarysales.Where(x => (x.date >= date1 && x.date <= date2)).ToList();
            var t_pr = 0;
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item in cus)
            {
                var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                t_pr = t_pr + Convert.ToInt32(item.Total);
                t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                tqt = tqt + Convert.ToInt32(item.PsQty);
            }
            ViewBag.tpr = t_pr;
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            ViewBag.d1 = date1.Day + "-" + date1.Month + "-" + date1.Year;
            ViewBag.d2 = date2.Day + "-" + date2.Month + "-" + date2.Year;
            return View(cus);
        }
        public IActionResult das()
        {
            return View();
        }
        public IActionResult das1(DateTime date1, DateTime date2)
        {
            var cus = db.Das.Where(x => (x.DasDate >= date1 && x.DasDate <= date2) && x.DasType == "Pay").ToList();
            ViewBag.d1 = date1.Day + "-" +date1.Month+"-"+date1.Year;
            ViewBag.d2 = date2.Day + "-" + date2.Month + "-" + date2.Year;
            return View(cus);
        }
        public IActionResult ag_pl(int id)
        {
            var cus = db.Products.Where(x => x.PId == id).First();
            ViewBag.id = id;
            ViewBag.name = cus.PName;
            return View();
        }
        public IActionResult ag_pl1(int id, DateTime date1, DateTime date2)
        {
            var cus = db.Productledgers.Where(x => x.Pid == id && (x.PlDate >= date1 && x.PlDate <= date2)).ToList();
            var cus1 = db.Products.Where(x => x.PId == id).First();
            ViewBag.id = cus1.PName;
            ViewBag.iid = cus1.PId;
            return View(cus);
        }
        public IActionResult ag_cl(int id)
        {
            var cus = db.Customers.Where(x => x.CId == id).First();
            ViewBag.id = id;
            ViewBag.name = cus.CName;
            return View();
        }
        public IActionResult ag_cl1(int id,DateTime date1,DateTime date2)
        {
            var cus = db.Customerledger.Where(x => x.CName == Convert.ToString(id) && (x.date>=date1 && x.date<=date2)).ToList();
            var cus1 = db.Customers.Where(x => x.CId == id).First();
            ViewBag.id = cus1.CName;
            ViewBag.iid = cus1.CId;
            return View(cus);
        }
        public IActionResult ag_vl(int id)
        {
            var cus = db.Venders.Where(x => x.VId == id).First();
            ViewBag.id = id;
            ViewBag.name = cus.VName;
            return View();
        }
        public IActionResult ag_vl1(int id, DateTime date1, DateTime date2)
        {
            var cus1 = db.Venders.Where(x => x.VId == id).First();
            var cus = db.Vendorledger.Where(x => x.VName == cus1.VName && (x.date>=date1 && x.date<=date2)).ToList();
            ViewBag.id = cus1.VName;
            ViewBag.iid = cus1.VId;
            return View(cus);
        }
        public IActionResult ag_ss()
        {
            return View();
        }
        public IActionResult ag_ss1(DateTime date1, DateTime date2,int cus,int pro,string pack)
        {
            var d1 = date1.Day;
            var m1 = date1.Month;
            var y1 = date1.Year;

            var d2 = date2.Day;
            var m2 = date2.Month;
            var y2 = date2.Year;

            if ((d1 > d2 && m1 == m2 && y1 == y2) || (m1 > m2 && y1 == y2) || (y1 > y2))
            {
                TempData["data"] = "Enter 'To Date' greater than 'From Date'";
                return RedirectToAction("ag_ss");
            }
            else if ((cus == 0 || cus == null) && (pro == 0 || pro == null))
            {
                if(pack == "0")
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2))).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
                else
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.Type == pack).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
            }
            else if ((cus > 0) && (pro == 0 || pro == null))
            {
                if(pack == "0")
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.CId == cus).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
                else
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.CId == cus && x.Type == pack).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
            }
            else if ((cus == 0 || cus == null) && (pro >0))
            {
                if(pack == "0")
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.PId == pro).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
                else
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.PId == pro && x.Type == pack).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
            }
            else if ((cus >0) && (pro > 0))
            {
                if(pack == "0")
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.CId == cus && x.PId == pro).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
                else
                {
                    var ss = db.Secondarysales.Where(x => ((x.date >= date1) && (x.date <= date2)) && x.CId == cus && x.PId == pro && x.Type == pack).OrderBy(x => x.date).ToList();
                    return View(ss);
                }
            }
            return View();
        }

        public IActionResult ag_ps()
        {
            return View();
        }
        public IActionResult ag_ps2(DateTime date1, DateTime date2, int cus)
        {
            var d1 = date1.Day;
            var m1 = date1.Month;
            var y1 = date1.Year;

            var d2 = date2.Day;
            var m2 = date2.Month;
            var y2 = date2.Year;

            if ((d1 > d2 && m1 == m2 && y1 == y2) || (m1 > m2 && y1 == y2) || (y1 > y2))
            {
                TempData["data"] = "Enter 'To Date' greater than 'From Date'";
                return RedirectToAction("ag_ps");
            }
            else if (cus == 0 || cus == null)
            {
                var ss = db.Primarysales.Where(x => x.date>=date1 && x.date<=date2).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in ss)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(ss);
            }
            else
            {
                var ss = db.Primarysales.Where(x => (x.date >= date1 && x.date <= date2) && x.PName == Convert.ToString(cus)).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in ss)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(ss);
            }
            return View();
        }
        public IActionResult cl(int id)
        {
            var x = db.Customers.Where(x => x.CId == id).First();
            ViewBag.name = x.CName;
            ViewBag.id = id;
            return View();
        }
        public IActionResult cl1(int id,DateTime date, DateTime date1)
        {

            var x1 = db.Customerledger.Where(x => x.CName == Convert.ToString(id) && (x.date>=date && x.date <= date1)).ToList();
            var cus = db.Customers.Where(x => x.CId == id).First();
            ViewBag.cus = cus.CName;
            return View(x1);
        }
        public IActionResult vl(int id)
        {
            var x = db.Venders.Where(x => x.VId == id).First();
            ViewBag.name = x.VName;
            ViewBag.id = id;
            return View();
        }
        public IActionResult vl1(int id, DateTime date, DateTime date1)
        {
            var x = db.Venders.Where(x => x.VId == id).First();
            var x1 = db.Vendorledger.Where(x => x.VName == Convert.ToString(x.VName) && (x.date >= date && x.date <= date1)).ToList();
            var cus = db.Venders.Where(x => x.VId == id).First();
            ViewBag.cus = cus.VName;
            return View(x1);
        }
        public IActionResult pl(int id)
        {
            ViewBag.id = id;
            return View();
        }
        public IActionResult pl1(int id, DateTime date, DateTime date1)
        {
            var xv = db.Products.Where(x => x.PId == id).First();
            var x1 = db.Productledgers.Where(x => x.Pid == xv.PId && (x.PlDate >= date && x.PlDate<= date1)).ToList();
            ViewBag.cus = xv.PName;
            return View(x1);
        }
        public IActionResult ag_das()
        {
            return View();
        }
        public IActionResult ag_das1(DateTime date, DateTime date1)
        {
            var x1 = db.Das.Where(x => (x.DasDate >= date && x.DasDate <= date1)).ToList();
            return View(x1);
        }
    }
}
