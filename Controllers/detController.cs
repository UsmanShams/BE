using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace be.Controllers
{
    public class detController : Controller
    {
        BentContext db = new BentContext();
        public IActionResult po(int id)
        {
            if(id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            var ccc = db.Pos.Any(x=>x.PoUnique == id);
            if (ccc is true)
            {
                var x = db.Pos.Where(x => x.PoUnique == id).Include(x => x.PIdNavigation).Include(x => x.VIdNavigation).ToList();
                var xx = db.Pos.Where(x => x.PoUnique == id);
                var item = db.Pos.Where(x => x.PoUnique == id).First();
                var r12 = db.PoNos.Where(x => x.PonoId == id).First();
                var r1 = db.Venders.Where(x => x.VId == Convert.ToInt32(r12.Vendor)).First();
                int x1 = 0;
                var x2 = 0;
                var gst = 1.18;
                foreach (var item1 in xx)
                {
                    x2 = Convert.ToInt32(item1.VId);
                }
                ViewBag.vendor = r1.VName; 
                if (id == 0 || id == null)
                {
                    ViewBag.id = TempData["id"];
                }
                else
                {
                    ViewBag.id = id;
                }
                ViewBag.date = item.day + "-" + item.month + "-" + item.year + " " + item.time;
                return View(x);
            }
            else
            {
                if(id ==0 || id == null)
                {
                    ViewBag.id = TempData["id"];
                }
                else
                {
                    ViewBag.id = id;
                }
                TempData["mess"] = "No Products In Purchase Order";
            }
            return View();
        }
        public IActionResult po_recipt(int id)
        {
			var x = db.Pos.Where(x => x.PoUnique == id).Include(x => x.PIdNavigation).Include(x => x.VIdNavigation).ToList();
			var xx = db.Pos.Where(x => x.PoUnique == id);
			int x1 = 0;
			foreach (var item in x)
			{
                var f = ((item.PIdNavigation.PSp/item.PIdNavigation.PPack) *0.18)+(item.PIdNavigation.PPr / item.PIdNavigation.PPack);
				int r = Convert.ToInt32(item.PoQty * item.PIdNavigation.PPack*f);
				x1 = x1 + r;
                var pro = db.Venders.Where(x => x.VId == item.VId).First();
                ViewBag.ntn = pro.VNtn;
                var a = item.day+"-"+item.month+"-"+item.year +" mg" + item.time;
				ViewBag.date = a;
                ViewBag.vendor = item.VIdNavigation.VName;
			}
			ViewBag.total = x1;
            ViewBag.id = id;
			return View(x);
        }
        public IActionResult order(int id)
        {
            if(id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            var ccc = db.Order.Where(x => x.OrUnique == id).ToList();
            var t_pr = 0;
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item in ccc)
            {
                var cus = db.Customers.Where(x => x.CId == item.CId).First();
                ViewBag.cname = cus.CName;
                var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PId)).First();
                t_pr = t_pr + Convert.ToInt32(item.OPrice*item.Qty);
                t_ltr = Convert.ToInt32(t_ltr + (r.PPack * item.Qty));
                tqt = tqt + Convert.ToInt32(item.Qty);
            }
            ViewBag.tpr = t_pr;
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            ViewBag.id = id;
            TempData["id"] = id;
            return View(ccc);
        }
        public IActionResult del_challan(int id)
        {
            var ccc = db.delivery.Where(x => x.del_no == id).ToList();
            ViewBag.id = id;
            return View(ccc);
        }
        public IActionResult del_recipt(int id)
        {
            var x = db.delivery.Where(x => x.del_no == id).ToList();
            int x1 = 0;
            var cus = "";
            var dc = 0;
            var add = "";
            foreach (var item in x)
            {
                x1 = x1 + Convert.ToInt32(item.qty);
                var del = db.del_no.Where(x => x.DId == item.del_no).First();
                var cus1 = db.Customers.Where(x => x.CId == del.CId).First();
                cus = cus1.CName;
                add = cus1.CAddress;
                dc = Convert.ToInt32(item.del_no);
            }
            ViewBag.total = x1;
            ViewBag.cus = cus;
            ViewBag.add = add;
            ViewBag.dc = dc;
			return View(x);
        }
        public IActionResult invoice(int id)
        {
            var ccc = db.Order.Where(x => x.OrUnique == id).ToList();
            var c1 = db.Order.Where(x => x.OrUnique == id).First();
            var t_pr = 0;
            var t_ltr = 0;
            var tqt = 0;
            foreach (var item in ccc)
            {
                var r = db.Products.Where(x => x.PId == Convert.ToInt32(item.PId)).First();
                t_pr = t_pr + Convert.ToInt32(item.OPrice * item.Qty );
                t_ltr = Convert.ToInt32(t_ltr + (r.PPack * item.Qty));
                tqt = tqt + Convert.ToInt32(item.Qty);
            }
            var cus = db.Customers.Where(x => x.CId == c1.CId).First();
            var inv_no = db.Invoices.Where(x => x.SsOrderno == id).First();
            ViewBag.invoice = inv_no.InId;
            ViewBag.date = inv_no.InDate1;
            ViewBag.name = cus.CName;
            ViewBag.name = cus.CAddress;
            ViewBag.tpr = t_pr;
            ViewBag.tltr = t_ltr;
            ViewBag.tqt = tqt;
            ViewBag.id = id;
            return View(ccc);
        }
    }
}
