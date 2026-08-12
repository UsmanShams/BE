using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace be.Controllers
{
    public class frontController : Controller
    {
        BentContext db = new BentContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult signin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult signin(string email,string pass)
        {
            var res = db.Users.Any(x=>x.UName==email && x.Pass==pass);
            if (res==true)
            {
                var res1 = db.Users.Where(x => x.UName == email && x.Pass == pass).First();
                HttpContext.Session.SetString("name",res1.UName);
                HttpContext.Session.SetString("role", Convert.ToString(res1.URole));
                Followup f = new Followup();
                f.CName = Convert.ToString(res1.UId);
                var x = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                f.FuDate = DateTime.Now.Date;
                f.FuDescription = DateTime.Now.ToShortTimeString();
                f.FuEntered = x;
                db.Followups.Add(f);
                db.SaveChanges();
                return RedirectToAction("dash1", "front");
                
            }
            else
            {
                TempData["mess"] = "Incorrect Email Or Password";
                return View();
            }
        }
        public IActionResult signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("signin");
        }
        public IActionResult user()
        {
            var x = db.Users.ToList();
            return View(x);
        }
        public IActionResult product()
        {
            var x = db.Products.ToList();
            return View(x);
        }
        public IActionResult customer(int id)
        {
            if(id == 1)
            {
                var x1 = db.Customers.Where(x=>x.CStatus == "Active").OrderBy(x => x.CName).ToList();
                TempData["type"] = "Active Customers";
                return View(x1);
            }
            else if(id == 2)
            {
                var x2 = db.Customers.Where(x => x.CStatus == "In Active").OrderBy(x => x.CName).ToList();
                TempData["type"] = "Non Active Customers";
                return View(x2);
            }
            var x = db.Customers.OrderBy(x=>x.CName).ToList();
            TempData["type"] = "All Customers";
            return View(x);
        }
        public IActionResult vendor()
        {
            var x = db.Venders.ToList();
            return View(x);
        }
        public IActionResult bank_statement()
        {
            var x = db.Bankstatements.ToList();
            return View(x);
        }
        public IActionResult bd_ledger(int id, BankDetail bd)
        {
            var x = db.BankDetail.Where(x => x.BdName == id).OrderBy(x=>x.date).ToList();
            var x11 = db.Bankstatements.Where(x => x.BsId == id).First();
            ViewBag.name = x11.Bankname;
            ViewBag.id = x11.BsId;
            var bal = 0;
            foreach(var item in x)
            {
                item.BdBalance = bal + item.BdIn - item.BdOut;
                bal = Convert.ToInt32(item.BdBalance);
                db.SaveChanges();
            }
            x11.Balance = bal;
            db.SaveChanges();
            return View(x);
        }
        public IActionResult followup()
        {
            var x = db.Followups.OrderByDescending(x => x.FuId).ToList();
            return View(x);
        }
        public IActionResult po()
        {
            var xx = db.PoNos.OrderByDescending(x=>x.PonoId);
            return View(xx);
        }
        public IActionResult grn()
        {
            var xx = db.Grns.OrderByDescending(x=>x.GrnId);
            return View(xx);
        }
		public IActionResult stock()
		{
			var xx = db.Stocks.ToList();
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item1 in xx)
            {
                var bal = 0;
                var xc = db.Productledgers.Where(x => x.Pid == item1.PId).ToList();
                foreach(var item in xc)
                {
                    item.PlBalance = bal + item.PlIn - item.PlOut;
                    bal = Convert.ToInt32(item.PlBalance);
                    db.SaveChanges();
                }
                item1.SQty = bal;
                db.SaveChanges();
            }
            foreach (var item in xx)
            {
                var r = db.Products.Where(x => x.PName == item.PName).First();
                t_ltr = Convert.ToInt32(t_ltr + (item.PPack * item.SQty));
                tqt = tqt + Convert.ToInt32(item.SQty);
            }
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            return View(xx);
		}
        public IActionResult pl(int id)
        {
            var xx = db.Productledgers.Where(x=>x.Pid == id).OrderBy(x=>x.PlDate).ToList();
            var r = db.Products.Where(x => x.PId == id).First();
            ViewBag.name = r.PName;
            ViewBag.iid = r.PId;
            var bal = 0;
            foreach(var item in xx)
            {
                item.PlBalance = bal + item.PlIn - item.PlOut;
                bal = Convert.ToInt32(item.PlBalance);
                db.SaveChanges();
            }
            return View(xx);
        }
        public IActionResult pro1(int id)
        {
            var x = db.Products.ToList();
            return View(x);
        }
        public IActionResult sl(int id)
        {
            var x = db.Productledgers.ToList();
            return View(x);
        }
        public IActionResult vl(int id)
        {
            var x = db.Venders.ToList();
            return View(x);
        }
        public IActionResult vl1(int id)
        {
            if (id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            var x1 = db.Venders.Where(x=>x.VId==id).First();
            var xx = db.Vendorledger.Where(x=>x.VName== x1.VName).OrderBy(x=>x.date).ToList();
            ViewBag.name = x1.VName;
            ViewBag.iid = x1.VId;
            var bal = 0;
            foreach (var item in xx)
            {
                item.VlBalance = bal + item.VlIn - item.VlOut;
                bal = Convert.ToInt32(item.VlBalance);
                db.SaveChanges();
            }
            return View(xx);
        }
        public IActionResult cl_search(int id)
        {
            return View();
        }
        public IActionResult cl(int id,int cus)
        {
            if(cus != 0)
            {
                var x = db.Customers.Where(x=>x.CId == cus).OrderBy(x => x.CName).ToList();
                return View(x);
            }
            else
            {
                var x = db.Customers.OrderBy(x => x.CName).ToList();
                return View(x);
            }
        }
        public IActionResult cl1(int id)
        {
            if(id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            var x1 = db.Customers.Where(x => x.CId == id).First();
            ViewBag.id = x1.CName;
            ViewBag.iid = x1.CId;
            var bal = 0;
            var xx = db.Customerledger.Where(x => x.CName == Convert.ToString(x1.CId)).OrderBy(x=>x.date).ToList();
            foreach (var item in xx)
            {
                item.ClBalance = bal + item.ClIn - item.ClOut;
                bal = Convert.ToInt32(item.ClBalance);
                db.SaveChanges();
            }
            return View(xx);
        }
        public IActionResult ps(int? pid, int? date,int? month,int? year,int? id)
        {
            if (month != 0 && year != null && date != 0 && pid !=0)
            {
                var xx = db.Primarysales.Where(x => x.month == Convert.ToString(month) && x.PName == Convert.ToString(pid) && x.day == date
                && x.year == year).OrderBy(x=>x.date).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in xx)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(xx);
            }
            else if (month == 0 && year is null && date == 0 && pid == 0)
            {
                var xx = db.Primarysales.OrderBy(x => x.date).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in xx)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(xx);
            }
            else if ((month != 0 && year is null && date == 0 && pid == 0) || (month != 0 && year is null && date != 0 && pid == 0))
            {
                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ps", "add");
            }
            else if (month == 0 && year is null && date != 0 && pid == 0)
            {
                TempData["month"] = "Plz Enter Month";
                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ps", "add");
            }
            else if (month == 0 && year is null && date == 0 && pid != null)
            {
                var xx = db.Primarysales.Where(x=>x.PName == Convert.ToString(pid) ).OrderBy(x => x.date).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in xx)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(xx);
            }
            else if (month != 0 && year != null && date != 0)
            {
                var xx = db.Primarysales.Where(x => x.month == Convert.ToString(month) && x.day == date 
                && x.year == year).OrderBy(x => x.date).ToList();
                var t_pr = 0;
                var t_ltr = 0;
                var tqt = 0;
                foreach (var item in xx)
                {
                    var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                    t_pr = t_pr + Convert.ToInt32(item.Total);
                    t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
                    tqt = tqt + Convert.ToInt32(item.PsQty);
                }
                ViewBag.tpr = t_pr;
                ViewBag.tltr = t_ltr;
                ViewBag.tqt = tqt;
                return View(xx);
            }
            else if (month == 0 && year != null && date != 0)
            {
                TempData["month"] = "Plz Enter Month";
                return RedirectToAction("search_ps","add");
            }
            else if (month != 0 && year != null)
			{
				var xx = db.Primarysales.Where(x => x.month == Convert.ToString(month) && x.year == year).OrderBy(x => x.date).ToList();
				var t_pr = 0;
				var t_ltr = 0;
				var tqt = 0;
				foreach (var item in xx)
				{
					var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
					t_pr = t_pr + Convert.ToInt32(item.Total);
					t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
					tqt = tqt + Convert.ToInt32(item.PsQty);
				}
				ViewBag.tpr = t_pr;
				ViewBag.tltr = t_ltr;
				ViewBag.tqt = tqt;
				return View(xx);
			}
			else if (month == 0 && year != null)
            {

				var xx = db.Primarysales.Where(x => x.year == year).OrderBy(x => x.date).ToList();
				var t_pr = 0;
				var t_ltr = 0;
				var tqt = 0;
				foreach (var item in xx)
				{
					var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
					t_pr = t_pr + Convert.ToInt32(item.Total);
					t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
					tqt = tqt + Convert.ToInt32(item.PsQty);
				}
				ViewBag.tpr = t_pr;
				ViewBag.tltr = t_ltr;
				ViewBag.tqt = tqt;
				return View(xx);
			}
            else if ((month == 0 && year is null) || id is null)
            {
				var xx = db.Primarysales.OrderBy(x => x.date).ToList();
				var t_pr = 0;
				var t_ltr = 0;
				var tqt = 0;
				foreach (var item in xx)
				{
					var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
					t_pr = t_pr + Convert.ToInt32(item.Total);
					t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
					tqt = tqt + Convert.ToInt32(item.PsQty);
				}
				ViewBag.tpr = t_pr;
				ViewBag.tltr = t_ltr;
				ViewBag.tqt = tqt;
				return View(xx);
			}
            else if(id != null)
            {
                var d = db.Venders.Where(x => x.VId == id).First();
                var xx = db.Primarysales.Where(x=>x.VName==d.VName).OrderBy(x => x.date).ToList();
				var t_pr = 0;
				var t_ltr = 0;
				var tqt = 0;
				foreach (var item in xx)
				{
					var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
					t_pr = t_pr + Convert.ToInt32(item.Total);
					t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
					tqt = tqt + Convert.ToInt32(item.PsQty);
				}
				ViewBag.tpr = t_pr;
				ViewBag.tltr = t_ltr;
				ViewBag.tqt = tqt;
				return View(xx);
			}
            else
            {
				var d = db.Venders.Where(x => x.VId == id).First();
				var xx = db.Primarysales.Where(x => x.VName == d.VName).OrderBy(x => x.date).ToList();
				var t_pr = 0;
				var t_ltr = 0;
				var tqt = 0;
				foreach (var item in xx)
				{
					var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
					t_pr = t_pr + Convert.ToInt32(item.Total);
					t_ltr = Convert.ToInt32(t_ltr + (item.PsPack * item.PsQty));
					tqt = tqt + Convert.ToInt32(item.PsQty);
				}
				ViewBag.tpr = t_pr;
				ViewBag.tltr = t_ltr;
				ViewBag.tqt = tqt;
				return View(xx);
			}

        }
            //primary end
        public IActionResult orno()
        {
            var xx = db.OrderNos.OrderByDescending(x => x.OrdernoId);
            return View(xx);
        }
        public IActionResult das()
        {
            var xx = db.Das.Where(x=>x.DasType == "Drum" || x.DasType =="p").OrderBy(x=>x.DasDate).ToList();
            var bal = 0;
            foreach (var item in xx)
            {
                item.DasBalance = bal + item.DasCredit - item.DasDeit;
                bal = Convert.ToInt32(item.DasBalance);
                db.SaveChanges();
            }
            return View(xx);
        }
        public IActionResult das_pay()
        {
            var xx = db.Das.Where(x => x.DasType == "Pay").ToList();
            return View(xx);
        }
        public IActionResult ss_pending()
        {
            var xx = db.Secondarysales.Where(x => x.Status == 0).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).ToList();
            var t_pr = 0;
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item in xx)
            {
                t_pr = t_pr + Convert.ToInt32(item.TPrice);
                t_ltr = Convert.ToInt32(t_ltr + (Convert.ToInt32(item.Pack) * item.SsQty));
                tqt = tqt + Convert.ToInt32(item.SsQty);
            }
            ViewBag.tpr = t_pr;
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            return View(xx);
        }
        public IActionResult ss(int? pid,int? cid,int? month,int? date,int? year,string type)
        {
            if (pid == null && cid == null && date == null && month == null && year == null && type == null)
            {
                var xx = db.Secondarysales.Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x=>x.date).ToList();
                return View(xx);
            }
            else if (pid == null && cid == null && date == null && month == null && year == null && type == "0")
            {
                var xx = db.Secondarysales.Where(x=> x.Status != 0).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == null && cid == null && date == null && month == null && year == null && type == "Drum")
            {
                var xx = db.Secondarysales.Where(x=>(x.Type == "Drum" || x.Type == "Druml")).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == null && cid == null && date == null && month == null && year == null && type == "Pail")
            {
                var xx = db.Secondarysales.Where(x => x.Type == "Pail" ).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid != 0 && date != 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x=>x.CId==cid && x.PId == pid && x.Day == date && x.Month == month && x.Year ==year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == 0 && cid != 0 && date == 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.CId == cid && x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid == 0 && date == 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.PId == pid && x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid != 0 && date != 0 && month != 0 && year == null)
            {

                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ss", "add");
            }
            else if (pid == 0 && cid != 0 && date != 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.CId == cid && x.Day == date && x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid == 0 && date != 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.PId == pid && x.Day == date && x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == 0 && cid == 0 && date != 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.Day == date && x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid != 0 && date == 0 && month == 0 && year == null)
            {
                var xx = db.Secondarysales.Where(x => x.CId == cid && x.PId == pid).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid != 0 && cid == 0 && date == 0 && month == 0 && year == null)
            {
                var xx = db.Secondarysales.Where(x=> x.PId == pid ).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == 0 && cid != 0 && date == 0 && month == 0 && year == null)
            {
                var xx = db.Secondarysales.Where(x => x.CId == cid ).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else if (pid == 0 && cid == 0 && date != 0 && month == 0 && year == null)
            {
                TempData["month"] = "Plz Enter Month";
                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ss", "add");
            }
            else if (pid == 0 && cid == 0 && date != 0 && month != 0 && year == null)
            {
                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ss", "add");
            }
            else if (pid == 0 && cid == 0 && date == 0 && month != 0 && year == null)
            {
                TempData["year"] = "Plz Enter Year";
                return RedirectToAction("search_ss", "add");
            }
            else if (pid == 0 && cid == 0 && date != 0 && month == 0 && year != null)
            {
                TempData["month"] = "Plz Enter Month";
                return RedirectToAction("search_ss", "add");
            }
            else if (pid == 0 && cid == 0 && date == 0 && month != 0 && year != null)
            {
                var xx = db.Secondarysales.Where(x => x.Month == month && x.Year == year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
            else
            {
                var xx = db.Secondarysales.Where(x => (x.Type == "Drum" && x.Status != 0) || x.Type == "Druml").Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).OrderBy(x => x.date).ToList();
                return View(xx);
            }
        }
        public IActionResult pending(int? pid,int cid)
        {
            if (pid == 0 && cid == 0)
            {
                var xx = db.Order.Where(x => x.type == "Drum").OrderBy(x => x.OrUnique).ToList();
                return View(xx);
            }
            else if(pid >0 && cid == 0)
            {
                var xx = db.Order.Where(x => x.PId == pid && x.type == "Drum").OrderBy(x => x.OrUnique).ToList();
                return View(xx);
            }
            else if(pid > 0 && cid > 0)
            {
                var xx = db.Order.Where(x => x.PId == pid && x.CId == cid && x.type == "Drum").OrderBy(x => x.OrUnique).ToList();
                return View(xx);
            }
            else if (pid == 0 && cid > 0)
            {
                var xx = db.Order.Where(x => x.CId == cid && x.type == "Drum").OrderBy(x => x.OrUnique).ToList();
                return View(xx);
            }

            var xx1 = db.Order.Where(x => x.type == "Drum").OrderBy(x => x.OrUnique).ToList();
            return View(xx1);
        }
        public IActionResult loose()
        {
            var x = db.Products.ToList();
            return View(x);
        }
        public IActionResult looseledger(int id)
        {
            if(id == null || id == 0)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            TempData["id"] = id;
            var x1 = db.looseledger.Any(x => x.Description == Convert.ToString(id));
            if(x1 is true)
            {
                var x = db.looseledger.Where(x => x.Description == Convert.ToString(id))
                    .OrderBy(o => o.year)
                    .ThenBy(o => o.month)
                    .ThenBy(o => o.day)
                    .ToList();
                var x2 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).First();
                var pro = db.Products.Where(x=>x.PId == Convert.ToInt32(x2.Description)).First();
                ViewBag.name = pro.PName;
                ViewBag.id = id;
                var bal = 0;
                foreach(var item in x)
                {
                    item.lBalance = bal + item.lIn - item.lOut;
                    bal = Convert.ToInt32(item.lBalance);
                    db.SaveChanges();
                }
                return View(x);
            }
            else
            {
                var x = db.looseledger.Where(x => x.Description == Convert.ToString(id))
                    .OrderBy(o => o.year)
                    .ThenBy(o => o.month)
                    .ThenBy(o => o.day).ToList();
                ViewBag.id = id; 
                var bal = 0;
                foreach (var item in x)
                {
                    item.lBalance = bal + item.lIn - item.lOut;
                    bal = Convert.ToInt32(item.lBalance);
                    db.SaveChanges();
                }
                return View(x);
            }
        }
        public IActionResult payment()
        {
            var x = db.Pay.OrderByDescending(x=>x.PaId).ToList();
            return View(x);
        }
        public IActionResult cheque(int id)
        {
            if(id == 1)
            {
                var x = db.cheque.Where(x => x.Type == "pet").OrderByDescending(x => x.Ch_Id).ToList();
                return View(x);
            }
            else
            {
                var x = db.cheque.Where(x => x.Type == "cus").OrderByDescending(x => x.Ch_Id).ToList();
                return View(x);
            }
        }
        public IActionResult mcr(int cus)
        {
            if (cus > 0)
            {
                var x1 = db.Customers.Where(x => x.CId == cus).OrderBy(x => x.CName).ToList();
                var bal = 0;
                foreach (var item in x1)
                {
                    var r1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId));
                    if (r1 is true)
                    {
                        var xx = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId)).OrderBy(x => x.date).ToList();
                        foreach (var item1 in xx)
                        {
                            item1.ClBalance = bal + item1.ClIn - item1.ClOut;
                            bal = Convert.ToInt32(item1.ClBalance);
                            db.SaveChanges();
                        }
                    }
                }
                ViewBag.bal = bal;
                return View(x1);
            }
            else
            {
                var x1 = db.Customers.Where(x => x.CName != "Bilal Associate").OrderBy(x => x.CName).ToList();
                var bal = 0;
                foreach (var item in x1)
                {
                    var r1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId));
                    if (r1 is true)
                    {
                        var xx = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId)).OrderBy(x => x.date).ToList();
                        foreach (var item1 in xx)
                        {
                            item1.ClBalance = bal + item1.ClIn - item1.ClOut;
                            bal = Convert.ToInt32(item1.ClBalance);
                            db.SaveChanges();
                        }
                    }
                }
                ViewBag.bal = bal;
                return View(x1);
            }
            return View();
        }
        public IActionResult mcr_search()
        {
            return View();
        }
        public IActionResult dash()
        {
            return View();
        }
        public IActionResult dash1()
        {
            var xx = db.Primarysales.Where(x=>x.month == Convert.ToString(DateTime.Now.Month) && x.year == DateTime.Now.Year).ToList();
            var t_pr = 0;
            var pr_ltr = 0;
            foreach (var item in xx)
            {
                var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PName)).First();
                pr_ltr = pr_ltr + Convert.ToInt32(item.PsQty * item.PsPack);
                t_pr = t_pr + Convert.ToInt32(item.Total);
            }
            ViewBag.tpr11 = t_pr;
            ViewBag.pr_ltr = pr_ltr;

            var xx1 = db.Secondarysales.Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).Include(x => x.PIdNavigation).Include(x => x.CIdNavigation).ToList();
            var t_pr1 = 0;
            var ss_ltr = 0;
            var pr_price = 0;
            foreach (var item in xx1)
            {
                ss_ltr = ss_ltr + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                t_pr1 = t_pr1 + (Convert.ToInt32(item.TPrice * item.SsQty));
                pr_price = pr_price + (Convert.ToInt32(item.Pr_Price * item.SsQty));
            }
            var profit = t_pr1-pr_price;
            ViewBag.tpr1 = t_pr1;
            ViewBag.ss_ltr = ss_ltr;
            ViewBag.profit = profit;

            var x1 = db.Customers.Where(x => x.CName != "Bilal Associate").OrderBy(x => x.CName).ToList();
            var bal = 0;
            foreach (var item in x1)
            {
                var r1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId));
                if (r1 is true)
                {
                    var xxf = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId)).OrderBy(x => x.date).ToList();
                    foreach (var item1 in xxf)
                    {
                        item1.ClBalance = bal + item1.ClIn - item1.ClOut;
                        bal = Convert.ToInt32(item1.ClBalance);
                        db.SaveChanges();
                    }
                }
            }

            var x2 = db.Bankstatements.ToList();
            var bank = 0;
            foreach (var item in x2)
            {
                bank = bank + Convert.ToInt32(item.Balance);
            }


            var xxm = db.Stocks.ToList();
            var t_ltr = 0;
            var tqt1 = 0;
   
            foreach (var item in xxm)
            {
                var r = db.Products.Where(x => x.PName == item.PName).First();
                t_ltr = Convert.ToInt32(t_ltr + (item.PPack * item.SQty));
                tqt1 = tqt1 + Convert.ToInt32(item.SQty);
            }
            ViewBag.tltr = t_ltr;
            ViewBag.tqt1 = tqt1;


            ViewBag.bal = bal;
            ViewBag.bank = bank;
            ViewBag.total = bal + bank;

            return View(xx);
        }
        public IActionResult s1()
        {
            var x1 = db.Customers.Where(x => x.CName != "Bilal Associate").OrderBy(x => x.CName).ToList();
            var bal = 0;
            foreach (var item in x1)
            {
                var r1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId));
                if (r1 is true)
                {
                    var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId)).OrderBy(x => x.ClId).Last();
                    bal = bal + Convert.ToInt32(r.ClBalance);
                }
            }

            var x2 = db.Bankstatements.ToList();
            var bank = 0;
            foreach (var item in x2)
            {
                bank = bank + Convert.ToInt32(item.Balance);
            }

            var xx = db.Stocks.ToList();
            var tqt = 0;
            foreach (var item in xx)
            {
                tqt = tqt + Convert.ToInt32(item.SQty);
            }
            ViewBag.tqt = tqt;

            ViewBag.bal = bal;
            ViewBag.bank = bank;
            ViewBag.total = bal + bank;
            return View();
        }
        public IActionResult cus_sum()
        {
            var res1 = db.Customers.OrderBy(x=>x.CName).Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            ViewBag.CId = res1;
            return View();
        }
        public IActionResult cus_sum1(int cus,int year)
        {
            var c = db.Secondarysales.Any(x => x.CId == cus && x.Year == year);
            var x = db.Customers.Where(x => x.CId == cus).First();
            var cus1 = x.CName;
            if (c is true)
            {
                var cc = db.Secondarysales.Where(x => x.CId == cus && x.Year == year).ToList();
                int jan = 0;
                int feb = 0;
                int mar = 0;
                int apr = 0;
                int may = 0;
                int jun = 0;
                int jul = 0;
                int aug = 0;
                int sep = 0;
                int oct = 0;
                int nov = 0;
                int dec = 0;

                int janl = 0;
                int febl = 0;
                int marl = 0;
                int aprl = 0;
                int mayl = 0;
                int junl = 0;
                int jull = 0;
                int augl = 0;
                int sepl = 0;
                int octl = 0;
                int novl = 0;
                int decl = 0;
                foreach (var item in cc)
                {
                    if(item.Month == 1)
                    {
                        jan = jan + Convert.ToInt32(item.TPrice * item.SsQty);
                        janl = janl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 2)
                    {
                        feb = feb + Convert.ToInt32(item.TPrice * item.SsQty);
                        febl = febl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 3)
                    {
                        mar = mar + Convert.ToInt32(item.TPrice * item.SsQty);
                        marl = marl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 4)
                    {
                        apr = apr + Convert.ToInt32(item.TPrice * item.SsQty);
                        aprl = aprl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 5)
                    {
                        may = may + Convert.ToInt32(item.TPrice * item.SsQty);
                        mayl = mayl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 6)
                    {
                        jun = jun + Convert.ToInt32(item.TPrice * item.SsQty);
                        junl = junl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 7)
                    {
                        jul = jul + Convert.ToInt32(item.TPrice * item.SsQty);
                        jull = jull + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 8)
                    {
                        aug = aug + Convert.ToInt32(item.TPrice * item.SsQty);
                        augl = augl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 9)
                    {
                        sep = sep + Convert.ToInt32(item.TPrice * item.SsQty);
                        sepl = sepl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 10)
                    {
                        oct = oct + Convert.ToInt32(item.TPrice * item.SsQty);
                        octl = octl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 11)
                    {
                        nov = nov + Convert.ToInt32(item.TPrice * item.SsQty);
                        novl = novl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                    else if (item.Month == 12)
                    {
                        dec = dec + Convert.ToInt32(item.TPrice * item.SsQty);
                        decl = decl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                    }
                }
                ViewBag.jan = jan;
                ViewBag.feb = feb;
                ViewBag.mar = mar;
                ViewBag.apr = apr;
                ViewBag.may = may;
                ViewBag.jun = jun;
                ViewBag.jul = jul;
                ViewBag.aug = aug;
                ViewBag.sep = sep;
                ViewBag.oct = oct;
                ViewBag.nov = nov;
                ViewBag.dec = dec;
                ViewBag.total = jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + dec;


                ViewBag.janl = janl;
                ViewBag.febl = febl;
                ViewBag.marl = marl;
                ViewBag.aprl = aprl;
                ViewBag.mayl = mayl;
                ViewBag.junl = junl;
                ViewBag.jull = jull;
                ViewBag.augl = augl;
                ViewBag.sepl = sepl;
                ViewBag.octl = octl;
                ViewBag.novl = novl;
                ViewBag.decl = decl;
                ViewBag.totall = janl + febl + marl + aprl + mayl + junl + jull + augl + sepl + octl + novl + decl;
            }
            ViewBag.name = cus1;
            return View();
        }
        public IActionResult ann_sum()
        {
            return View();
        }
        public IActionResult ann_sum1(int year)
        {
            ViewBag.year = year;
            var c = db.Secondarysales.Any(x => x.Year == year);
            int ssjan = 0;
            int ssfeb = 0;
            int ssmar = 0;
            int ssapr = 0;
            int ssmay = 0;
            int ssjun = 0;
            int ssjul = 0;
            int ssaug = 0;
            int sssep = 0;
            int ssoct = 0;
            int ssnov = 0;
            int ssdec = 0;

            int prjan = 0;
            int prfeb = 0;
            int prmar = 0;
            int prapr = 0;
            int prmay = 0;
            int prjun = 0;
            int prjul = 0;
            int praug = 0;
            int prsep = 0;
            int proct = 0;
            int prnov = 0;
            int prdec = 0;


            if (c is true)
            {
                var cc = db.Secondarysales.Where(x =>x.Year == year).ToList();
                

                int ssjanl = 0;
                int ssfebl = 0;
                int ssmarl = 0;
                int ssaprl = 0;
                int ssmayl = 0;
                int ssjunl = 0;
                int ssjull = 0;
                int ssaugl = 0;
                int sssepl = 0;
                int ssoctl = 0;
                int ssnovl = 0;
                int ssdecl = 0;
                foreach (var item in cc)
                {
                    if (item.Month == 1)
                    {
                        ssjan = ssjan + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssjanl = ssjanl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack)) ;
                        prjan = prjan + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 2)
                    {
                        ssfeb = ssfeb + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssfebl = ssfebl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prfeb = prfeb + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 3)
                    {
                        ssmar = ssmar + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssmarl = ssmarl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prmar = prmar +Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 4)
                    {
                        ssapr = ssapr + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssaprl = ssaprl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prapr = prapr + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 5)
                    {
                        ssmay = ssmay + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssmayl = ssmayl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prmay = prmay + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 6)
                    {
                        ssjun = ssjun + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssjunl = ssjunl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prjun = prjun + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 7)
                    {
                        ssjul = ssjul + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssjull = ssjull + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prjul = prjul + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 8)
                    {
                        ssaug = ssaug + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssaugl = ssaugl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        praug = praug + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 9)
                    {
                        sssep = sssep + Convert.ToInt32(item.TPrice * item.SsQty);
                        sssepl = sssepl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prsep = prsep + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 10)
                    {
                        ssoct = ssoct + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssoctl = ssoctl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        proct = proct + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 11)
                    {
                        ssnov = ssnov + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssnovl = ssnovl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prnov = prnov + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                    else if (item.Month == 12)
                    {
                        ssdec = ssdec + Convert.ToInt32(item.TPrice * item.SsQty);
                        ssdecl = ssdecl + Convert.ToInt32(item.SsQty * Convert.ToInt32(item.Pack));
                        prdec = prdec + Convert.ToInt32(item.Pr_Price * item.SsQty);
                    }
                }

                prjan = ssjan - prjan;
                prfeb = ssfeb - prfeb;
                prmar = ssmar - prmar;
                prapr = ssapr - prapr;
                prmay = ssmay - prmay;
                prjun = ssjun - prjun;
                prjul = ssjul - prjul;
                praug = ssaug - praug;
                prsep = sssep - prsep;
                proct = ssoct - proct;
                prnov = ssnov - prnov;
                prdec = ssdec - prdec;

                ViewBag.prjan = prjan;
                ViewBag.prfeb = prfeb;
                ViewBag.prmar = prmar;
                ViewBag.prapr = prapr;
                ViewBag.prmay = prmay;
                ViewBag.prjun = prjun;
                ViewBag.prjul = prjul;
                ViewBag.praug = praug;
                ViewBag.prsep = prsep;
                ViewBag.proct = proct;
                ViewBag.prnov = prnov;
                ViewBag.prdec = prdec;
                ViewBag.prtotal = prjan + prfeb + prmar + prapr + prmay + prjun + prjul + praug + prsep + proct + prnov + prdec;



                ViewBag.sjan = ssjan;
                ViewBag.sfeb = ssfeb;
                ViewBag.smar = ssmar;
                ViewBag.sapr = ssapr;
                ViewBag.smay = ssmay;
                ViewBag.sjun = ssjun;
                ViewBag.sjul = ssjul;
                ViewBag.saug = ssaug;
                ViewBag.ssep = sssep;
                ViewBag.soct = ssoct;
                ViewBag.snov = ssnov;
                ViewBag.sdec = ssdec;
                ViewBag.stotal = ssjan + ssfeb + ssmar + ssapr + ssmay + ssjun + ssjul + ssaug + sssep + ssoct + ssnov + ssdec;


                ViewBag.sjanl = ssjanl;
                ViewBag.sfebl = ssfebl;
                ViewBag.smarl = ssmarl;
                ViewBag.saprl = ssaprl;
                ViewBag.smayl = ssmayl;
                ViewBag.sjunl = ssjunl;
                ViewBag.sjull = ssjull;
                ViewBag.saugl = ssaugl;
                ViewBag.ssepl = sssepl;
                ViewBag.soctl = ssoctl;
                ViewBag.snovl = ssnovl;
                ViewBag.sdecl = ssdecl;
                ViewBag.stotall = ssjanl + ssfebl + ssmarl + ssaprl + ssmayl + ssjunl + ssjull + ssaugl + sssepl + ssoctl + ssnovl + ssdecl;

            }


            var c1 = db.Primarysales.Any(x => x.year == year);
            if (c1 is true)
            {
                var cc1 = db.Primarysales.Where(x => x.year == year).ToList();
                int pjan = 0;
                int pfeb = 0;
                int pmar = 0;
                int papr = 0;
                int pmay = 0;
                int pjun = 0;
                int pjul = 0;
                int paug = 0;
                int psep = 0;
                int poct = 0;
                int pnov = 0;
                int pdec = 0;

                int pjanl = 0;
                int pfebl = 0;
                int pmarl = 0;
                int paprl = 0;
                int pmayl = 0;
                int pjunl = 0;
                int pjull = 0;
                int paugl = 0;
                int psepl = 0;
                int poctl = 0;
                int pnovl = 0;
                int pdecl = 0;
                foreach (var item in cc1)
                {
                    if (item.month == "1")
                    {
                        pjan = pjan + Convert.ToInt32(item.Total);
                        pjanl = pjanl + Convert.ToInt32(item.PsQty*item.PsPack);
                    }
                    else if (item.month == "2")
                    {
                        pfeb = pfeb + Convert.ToInt32(item.Total);
                        pfebl = pfebl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "3")
                    {
                        pmar = pmar + Convert.ToInt32(item.Total);
                        pmarl = pmarl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "4")
                    {
                        papr = papr + Convert.ToInt32(item.Total);
                        paprl = paprl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "5")
                    {
                        pmay = pmay + Convert.ToInt32(item.Total);
                        pmayl = pmayl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "6")
                    {
                        pjun = pjun + Convert.ToInt32(item.Total);
                        pjunl = pjunl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "7")
                    {
                        pjul = pjul + Convert.ToInt32(item.Total);
                        pjull = pjull + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "8")
                    {
                        paug = paug + Convert.ToInt32(item.Total);
                        paugl = paugl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "9")
                    {
                        psep = psep + Convert.ToInt32(item.Total);
                        psepl = psepl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "10")
                    {
                        poct = poct + Convert.ToInt32(item.Total);
                        poctl = poctl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "11")
                    {
                        pnov = pnov + Convert.ToInt32(item.Total);
                        pnovl = pnovl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                    else if (item.month == "12")
                    {
                        pdec = pdec + Convert.ToInt32(item.Total);
                        pdecl = pdecl + Convert.ToInt32(item.PsQty * item.PsPack);
                    }
                }
                ViewBag.pjan = pjan;
                ViewBag.pfeb = pfeb;
                ViewBag.pmar = pmar;
                ViewBag.papr = papr;
                ViewBag.pmay = pmay;
                ViewBag.pjun = pjun;
                ViewBag.pjul = pjul;
                ViewBag.paug = paug;
                ViewBag.psep = psep;
                ViewBag.poct = poct;
                ViewBag.pnov = pnov;
                ViewBag.pdec = pdec;
                ViewBag.ptotal = pjan + pfeb + pmar + papr + pmay + pjun + pjul + paug + psep + poct + pnov + pdec;


                ViewBag.pjanl = pjanl;
                ViewBag.pfebl = pfebl;
                ViewBag.pmarl = pmarl;
                ViewBag.paprl = paprl;
                ViewBag.pmayl = pmayl;
                ViewBag.pjunl = pjunl;
                ViewBag.pjull = pjull;
                ViewBag.paugl = paugl;
                ViewBag.psepl = psepl;
                ViewBag.poctl = poctl;
                ViewBag.pnovl = pnovl;
                ViewBag.pdecl = pdecl;
                ViewBag.ptotall = pjanl + pfebl + pmarl + paprl + pmayl + pjunl + pjull + paugl + psepl + poctl + pnovl + pdecl;
            }


            var x1 = db.Customers.Where(x => x.CName != "Bilal Associate").OrderBy(x => x.CName).ToList();
            var bal = 0;
            int cjan = 0;
            int cfeb = 0;
            int cmar = 0;
            int capr = 0;
            int cmay = 0;
            int cjun = 0;
            int cjul = 0;
            int caug = 0;
            int csep = 0;
            int coct = 0;
            int cnov = 0;
            int cdec = 0;
            foreach (var item in x1)
            {
                var r1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.year == year);
                if (r1 is true)
                {
                    var j1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 1 && x.year == year);
                    if(j1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 1 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 1)
                        {
                            foreach(var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn-item1.ClOut);
                                cjan = cjan + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var f1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 2 && x.year == year);
                    if (f1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 2 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 2)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cfeb = cfeb + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var m1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 3 && x.year == year);
                    if (m1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 3 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 3)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cmar = cmar + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var a1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 4 && x.year == year);
                    if (a1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 4 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 4)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                capr = capr + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var m2 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 5 && x.year == year);
                    if (m2 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 5 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 5)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cmay = cmay + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var j2 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 6 && x.year == year);
                    if (j2 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 6 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 6)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cjun = cjun + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var j3 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 7 && x.year == year);
                    if (j3 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 7 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 7)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cjul = cjul + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var a2 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 8 && x.year == year);
                    if (a2 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 8 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 8)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                caug = caug + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var s1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 9 && x.year == year);
                    if (s1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 9 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 9)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                csep = csep + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var o1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 10 && x.year == year);
                    if (o1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 10 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 9)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                coct = coct + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var n1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 11 && x.year == year);
                    if (n1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 11 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 11)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cnov = cnov + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                    var d1 = db.Customerledger.Any(x => x.CName == Convert.ToString(item.CId) && x.month <= 12 && x.year == year);
                    if (d1 is true)
                    {
                        var r = db.Customerledger.Where(x => x.CName == Convert.ToString(item.CId) && x.month <= 12 && x.year == year).OrderBy(x => x.date).ToList();
                        if (DateTime.Now.Month >= 12)
                        {
                            foreach (var item1 in r)
                            {
                                bal = bal + Convert.ToInt32(item1.ClIn - item1.ClOut);
                                cdec = cdec + Convert.ToInt32(item1.ClIn - item1.ClOut);
                            }
                        }
                    }

                }
            }
            ViewBag.bal = bal;
            ViewBag.cjan = cjan;
            ViewBag.cfeb = cfeb;
            ViewBag.cmar = cmar;
            ViewBag.capr = capr;
            ViewBag.cmay = cmay;
            ViewBag.cjun = cjun;
            ViewBag.cjul = cjul;
            ViewBag.caug = caug;
            ViewBag.csep = csep;
            ViewBag.coct = coct;
            ViewBag.cnov = cnov;
            ViewBag.cdec = cdec;
            ViewBag.ctotal = cjan + cfeb + cmar + capr + cmay + cjun + cjul + caug + csep + coct + cnov + cdec;


            var x11 = db.Products.ToList();
            int stjan = 0;
            int stfeb = 0;
            int stmar = 0;
            int stapr = 0;
            int stmay = 0;
            int stjun = 0;
            int stjul = 0;
            int staug = 0;
            int stsep = 0;
            int stoct = 0;
            int stnov = 0;
            int stdec = 0;

            int stjanp = 0;
            int stfebp = 0;
            int stmarp = 0;
            int staprp = 0;
            int stmayp = 0;
            int stjunp = 0;
            int stjulp = 0;
            int staugp = 0;
            int stsepp = 0;
            int stoctp = 0;
            int stnovp = 0;
            int stdecp = 0;

            foreach (var item in x11)
            {
                var prro = db.Products.Where(x => x.PId == item.PId).First();
                var r1 = db.Productledgers.Any(x => x.Pid == item.PId && x.year == year);
                if (r1 is true)
                {
                    var j1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 1 && x.year == year);
                    if (j1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 1 && x.year == year).OrderBy(x=>x.PlId).Last();
                        if (DateTime.Now.Month >= 1)
                        {
                            stjan = stjan + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var f1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 2 && x.year == year);
                    if (f1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 2 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 2)
                        {
                            stfeb = stfeb + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }
                    var m1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 3 && x.year == year);
                    if (m1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 3 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 3)
                        {
                            stmar = stmar + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var a1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 4 && x.year == year);
                    if (a1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 4 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 4)
                        {
                            stapr = stapr + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var m2 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 5 && x.year == year);
                    if (m2 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 5 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 5)
                        {
                            stmay = stmay + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var j2 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 6 && x.year == year);
                    if (j2 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 6 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 6)
                        {
                            stjun = stjun + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var j3 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 7 && x.year == year);
                    if (j3 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 7 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 7)
                        {
                            stjul = stjul + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var a2 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 8 && x.year == year);
                    if (a2 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 8 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 8)
                        {
                            staug = staug + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var s1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 9 && x.year == year);
                    if (s1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 9 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 9)
                        {
                            stsep = stsep + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var o1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 10 && x.year == year);
                    if (o1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 10 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 10)
                        {
                            stoct = stoct + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var n1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 11 && x.year == year);
                    if (n1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 11 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 11)
                        {
                            stnov = stnov + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                    var d1 = db.Productledgers.Any(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 12 && x.year == year);
                    if (d1 is true)
                    {
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && Convert.ToInt32(x.month) <= 12 && x.year == year).OrderBy(x => x.PlId).Last();
                        if (DateTime.Now.Month >= 12)
                        {
                            stdec = stdec + Convert.ToInt32(r.PlBalance * prro.PPack);
                        }
                    }

                }
            }

            foreach (var item in x11)
            {
                var prro = db.Products.Where(x => x.PId == item.PId).First();
                var r1 = db.Productledgers.Any(x => x.Pid == item.PId && x.year == year);
                if (r1 is true)
                {
                    var j1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "1" && x.year == year);
                    if (j1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "1" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "1" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 1 && x.Year == year && x.Status != "del1");
                        
                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 1 && x.Year == year && x.Status != "del1").ToList();
                            foreach(var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stjanp = stjanp + items.Price;
                                }
                            }
                        }
                    }

                    var f1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "2" && x.year == year);
                    if (f1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "2" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "2" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 2 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 2 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stfebp = stfebp + items.Price;
                                }
                            }
                        }
                    }
                    var m1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "3" && x.year == year);
                    if (m1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "3" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "3" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 3 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 3 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stmarp = stmarp + items.Price;
                                }
                            }
                        }
                    }

                    var a1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "4" && x.year == year);
                    if (a1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "4" && x.year == year).OrderBy(x => x.PlId).Last();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 4 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 4 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    staprp = staprp + items.Price;
                                }
                            }
                        }
                    }

                    var m2 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "5" && x.year == year);
                    if (m2 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "5" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "5" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 5 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 5 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stmayp = stmayp + items.Price;
                                }
                            }
                        }
                    }

                    var j2 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "6" && x.year == year);
                    if (j2 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "6" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "6" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 6 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 6 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stjunp = stjunp + items.Price;
                                }
                            }
                        }
                    }

                    var j3 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "7" && x.year == year);
                    if (j3 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "7" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "7" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 7 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 7 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stjulp = stjulp + items.Price;
                                }
                            }
                        }
                    }

                    var a2 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "8" && x.year == year);
                    if (a2 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "8" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "8" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 8 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 8 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    staugp = staugp + items.Price;
                                }
                            }
                        }
                    }

                    var s1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "9" && x.year == year);
                    if (s1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "9" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "9" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 9 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 9 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stsepp = stsepp + items.Price;
                                }
                            }
                        }
                    }

                    var o1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "10" && x.year == year);
                    if (o1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "10" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "10" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 10 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 10 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stoctp = stoctp + items.Price;
                                }
                            }
                        }
                    }

                    var n1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "11" && x.year == year);
                    if (n1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "11" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "11" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 11 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 11 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stnovp = stnovp + items.Price;
                                }
                            }
                        }
                    }

                    var d1 = db.Productledgers.Any(x => x.Pid == item.PId && x.month == "12" && x.year == year);
                    if (d1 is true)
                    {
                        var tot = 0;
                        var r = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "12" && x.year == year).OrderBy(x => x.PlId).Last();
                        var rj = db.Productledgers.Where(x => x.Pid == item.PId && x.month == "12" && x.year == year).ToList();
                        var sto = db.StockPrice.Any(x => x.PId == item.PId && x.Month == 12 && x.Year == year && x.Status != "del1");

                        if (sto is true)
                        {
                            var sto1 = db.StockPrice.Where(x => x.PId == item.PId && x.Month == 12 && x.Year == year && x.Status != "del1").ToList();
                            foreach (var xx in sto1)
                            {
                                tot = tot + 1;
                            }
                            var qty = r.PlBalance;
                            var diff = tot - qty;
                            var chk = 0;
                            foreach (var items in sto1)
                            {
                                chk = chk + 1;
                                if (chk > diff)
                                {
                                    stdecp = stdecp + items.Price;
                                }
                            }
                        }
                    }

                }
            }

            ViewBag.stjan = stjan;
            ViewBag.stfeb = stfeb;
            ViewBag.stmar = stmar;
            ViewBag.stapr = stapr;
            ViewBag.stmay = stmay;
            ViewBag.stjun = stjun;
            ViewBag.stjul = stjul;
            ViewBag.staug = staug;
            ViewBag.stsep = stsep;
            ViewBag.stoct = stoct;
            ViewBag.stnov = stnov;
            ViewBag.stdec = stdec;
            ViewBag.sttotal = stjan + stfeb + stmar + stapr + stmay + stjun + stjul + staug + stsep + stoct + stnov + stdec;


            ViewBag.stjanp = stjanp;
            ViewBag.stfebp = stfebp;
            ViewBag.stmarp = stmarp;
            ViewBag.staprp = staprp;
            ViewBag.stmayp = stmayp;
            ViewBag.stjunp = stjunp;
            ViewBag.stjulp = stjulp;
            ViewBag.staugp = staugp;
            ViewBag.stsepp = stsepp;
            ViewBag.stoctp = stoctp;
            ViewBag.stnovp = stnovp;
            ViewBag.stdecp = stdecp;
            ViewBag.sttotalp = stjanp + stfebp + stmarp + staprp + stmayp + stjunp + stjulp + staugp + stsepp + stoctp + stnovp + stdecp;

            var x12 = db.Bankstatements.ToList();
            int bjan = 0;
            int bfeb = 0;
            int bmar = 0;
            int bapr = 0;
            int bmay = 0;
            int bjun = 0;
            int bjul = 0;
            int baug = 0;
            int bsep = 0;
            int boct = 0;
            int bnov = 0;
            int bdec = 0;
            foreach (var item in x12)
            {
                var r1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.year == year);
                if (r1 is true)
                {
                    var j1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 1 && x.year == year);
                    if (j1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 1 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 1)
                        {
                            bjan = bjan + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var f1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 2 && x.year == year);
                    if (f1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 2 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 2)
                        {
                            bfeb = bfeb + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var m1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 3 && x.year == year);
                    if (m1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 3 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 3)
                        {
                            bmar = bmar + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var a1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 4 && x.year == year);
                    if (a1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 4 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 4)
                        {
                            bapr = bapr + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var m2 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 5 && x.year == year);
                    if (m2 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 5 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 5)
                        {
                            bmay = bmay + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var j2 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 6 && x.year == year);
                    if (j2 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 6 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 6)
                        {
                            bjun = bjun + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var j3 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 7 && x.year == year);
                    if (j3 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 7 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 7)
                        {
                            bjul = bjul + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var a2 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 8 && x.year == year);
                    if (a2 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 8 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 8)
                        {
                            baug = baug + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var s1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 9 && x.year == year);
                    if (s1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 9 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 9)
                        {
                            bsep = bsep + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var o1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 10 && x.year == year);
                    if (o1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 10 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 10)
                        {
                            boct = boct + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var n1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 11 && x.year == year);
                    if (n1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 11 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 11)
                        {
                            bnov = bnov + Convert.ToInt32(r.BdBalance);
                        }
                    }

                    var d1 = db.BankDetail.Any(x => x.BdName == item.BsId && x.month <= 12 && x.year == year);
                    if (d1 is true)
                    {
                        var r = db.BankDetail.Where(x => x.BdName == item.BsId && x.month <= 12 && x.year == year).OrderBy(x => x.BdId).Last();
                        if (DateTime.Now.Month >= 12)
                        {
                            bdec = bdec + Convert.ToInt32(r.BdBalance);
                        }
                    }


                }
            }
            ViewBag.bjan = bjan;
            ViewBag.bfeb = bfeb;
            ViewBag.bmar = bmar;
            ViewBag.bapr = bapr;
            ViewBag.bmay = bmay;
            ViewBag.bjun = bjun;
            ViewBag.bjul = bjul;
            ViewBag.baug = baug;
            ViewBag.bsep = bsep;
            ViewBag.boct = boct;
            ViewBag.bnov = bnov;
            ViewBag.bdec = bdec;
            ViewBag.btotal = bjan + bfeb + bmar + bapr + bmay + bjun + bjul + baug + bsep + boct + bnov + bdec;

            return View();
        }
        public IActionResult del_challan()
        {
            var x = db.del_no.OrderByDescending(x=>x.DId).ToList();
            return View(x);
        }
    }
}















