using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace be.Controllers
{
    public class addController : Controller
    {
        BentContext db = new BentContext();
        public IActionResult user()
        {
            return View();
        }
        [HttpPost]
        public IActionResult user(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("user","front");
        }
        public IActionResult product()
        {
            return View();
        }
        [HttpPost]
        public IActionResult product(Product p)
        {
            p.PType = "Oil";
			db.Products.Add(p);
            db.SaveChanges();
            var pro = db.Products.ToList();
            var cus = db.Customers.ToList();
            foreach (var item in pro)
            {
                foreach(var item1 in cus)
                {
                    var x = db.cus_pro.Any(x => x.cid == item1.CId && x.pid == item.PId);
                    if (x is false)
                    {
                        cus_pro cp = new cus_pro();
                        cp.cid = item1.CId;
                        cp.pid = item.PId;
                        cp.baseprice = 0;
                        db.cus_pro.Add(cp);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("product", "front");
        }
        public IActionResult customer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult customer(Customer c)
        {
            db.Customers.Add(c);
            db.SaveChanges();
            var cus = db.Customers.OrderBy(x => x.CId).Last();
            var pro = db.Products.ToList();
            foreach (var item in pro)
            {
                var x = db.cus_pro.Any(x => x.cid == cus.CId && x.pid == item.PId);
                if (x is false)
                {
                    cus_pro cp = new cus_pro();
                    cp.cid = cus.CId;
                    cp.pid = item.PId;
                    cp.baseprice = 0;
                    db.cus_pro.Add(cp);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("customer", "front");
        }
        public IActionResult vendor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult vendor(Vender v)
        {
            db.Venders.Add(v);
            db.SaveChanges();
            return RedirectToAction("vendor", "front");
        }
        public IActionResult bank_statement()
        {
            return View();
        }
        [HttpPost]
        public IActionResult bank_statement(Bankstatement b)
        {
            db.Bankstatements.Add(b);
            db.SaveChanges();
            var x2 = db.Bankstatements.OrderBy(x => x.BsId).Last();
            BankDetail bd1 = new BankDetail();
            bd1.BdName = x2.BsId;
            bd1.BdSender = "Closing";
            bd1.BdIn = x2.Balance;
            bd1.BdOut = 0;
            bd1.BdBalance = x2.Balance;
            bd1.day = DateTime.Now.Day;
            bd1.date = DateTime.Now.Date;
            bd1.month = DateTime.Now.Month;
            bd1.year = DateTime.Now.Year;
            bd1.time = DateTime.Now.ToShortTimeString();
            db.BankDetail.Add(bd1);
            db.SaveChanges();
            return RedirectToAction("bank_statement", "front");
        }
        public IActionResult followup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult followup(Followup f)
        {
            db.Followups.Add(f);
            db.SaveChanges();
            return RedirectToAction("followup", "front");
        }
        public IActionResult po1()
        {
            var res1 = db.Venders.Select(x => new SelectListItem { Text = x.VName, Value = Convert.ToString(x.VId) });
            ViewBag.VId = res1;
            //PoNo pp = new PoNo();
            //pp.PonoStatus = "no";
            //db.PoNos.Add(pp);
            //db.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult po1(PoNo p)
        {
            p.PonoStatus = "no";
            db.PoNos.Add(p);
            db.SaveChanges();
            return RedirectToAction("po");
        }
        public IActionResult po()
        {
            var r = db.PoNos.OrderBy(x => x.PonoId).Last();
            var x = db.Pos.Where(x=>x.PoUnique==r.PonoId).ToList();
            var res = db.Products.Select(x=> new SelectListItem { Text = x.PName+" , "+ x.PPack+ "L ", Value = Convert.ToString(x.PId) });
            ViewBag.PId = res;
            return View();
        }
        [HttpPost]
        public IActionResult po(Po p,int id)
        {
            if (p.PoQty <= 0)
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("po");
            }
            var res = db.PoNos.OrderBy(x => x.PonoId).Last();
            var x21 = db.Pos.Any(x=>x.PoUnique == res.PonoId && x.PId == p.PId);
            if(x21 == true)
            {
				var x211 = db.Pos.Where(x => x.PoUnique == res.PonoId && x.PId == p.PId).First();
                x211.PoQty = x211.PoQty + p.PoQty;
                db.SaveChanges();

                var rj = db.Products.Where(x=>x.PId==x211.PId).First();
                var xe = db.Pos.Where(x => x.PoUnique == res.PonoId && x.PId == p.PId).First();
				var disc = Convert.ToInt32(rj.Trade_disc) * rj.PPack;
				xe.PoPrice = rj.PPr-disc; 
			}
            else
            { 
				p.PoUnique = res.PonoId;
				var x = db.Venders.Where(x => x.VId == Convert.ToInt32(res.Vendor)).First();
				p.VId = x.VId;
				p.PoDate = DateTime.Now;
                p.day = DateTime.Now.Day;
                p.month = DateTime.Now.Month;
                p.year = DateTime.Now.Year;
                p.time = DateTime.Now.ToShortTimeString();
				if (p.PoPrice is null)
				{
					var rj = db.Products.Where(x => x.PId == p.PId).First();

                    var r = Convert.ToDouble(rj.PPr * 1.00 / rj.PPack * 1.00);
                    var pprint = Convert.ToInt64(r * 100);
                    var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);
                    var ppr1 = pprfloat - Convert.ToDouble(rj.Trade_disc);

                    p.baseprltr = Convert.ToString(ppr1);
                    var discount = Convert.ToInt32(rj.Trade_disc) * rj.PPack;
                    p.PoPrice = rj.PPr-discount;
               
				}
				var c = db.Pos.Where(x => x.PoUnique == res.PonoId).Count();
				if (c == 0)
				{
					p.Count = 1;
				}
				else if (c > 0)
				{
					var cc = db.Pos.Where(x => x.PoUnique == res.PonoId).Count();
					p.Count = cc + 1;
				}
                db.Pos.Add(p);
            }
            db.SaveChanges();
            if (id == 1)
            {
                return RedirectToAction("po");
            }
            else
            {
                return RedirectToAction("po", "front");
            }
            
        }
        public IActionResult grn(int id)
        {
            if(id == 0 || id == null)
            {
                ViewBag.id = TempData["data"];
            }
            else
            {
                ViewBag.id = id;
            }
            return View();
        }
        [HttpPost]
        public IActionResult grn(Grn g,int id)
        {
            if (Convert.ToInt32(g.GrnDc) <= 0)
            {
                TempData["data"] = Convert.ToInt32(g.GrnDc);
                TempData["mess"] = "Enter Correct Grn";
                return RedirectToAction("grn");
            }
            var xx = db.Pos.Where(x => x.PoUnique == id).First();
            var x = db.PoNos.Where(x => x.PonoId == id).First();
            var chk3 = db.Grns.Any(x=>x.GrnDc ==  g.GrnDc);
            if(chk3 is true)
            {
                TempData["mess"] = "Grn Number Already Existed In Database";
                return RedirectToAction("grn");
            }
            else
            {
                x.PonoStatus = "yes";
                g.PoId = xx.PoId;
                g.Pono = id;
                g.GrnDate = DateTime.Now;
                g.day = DateTime.Now.Day;
                g.month = DateTime.Now.Month;
                g.year = DateTime.Now.Year;
                g.time = DateTime.Now.ToShortTimeString();
                db.Grns.Add(g);
                db.SaveChanges();
            }
            var credit = 0;
            var pr = 0;
            var c = db.Pos.Where(x => x.PoUnique == id).Count();
            for(int i = 1; i <= c; i++)
            {
                var ch = db.Pos.Where(x => x.PoUnique == id && x.Count == i).First();
                var pro = db.Products.Where(x => x.PId == ch.PId).First();
                var chk = db.Stocks.Any(x => x.PId == pro.PId);
                pr = pr + Convert.ToInt32(ch.PoPrice);
                credit = credit + Convert.ToInt32(ch.PoQty * ch.PIdNavigation.PPack);

                StockPrice sp = new StockPrice();
                for(int j = 1; j <= ch.PoQty; j++)
                {
                    var cv = db.StockPrice.Any(x=>x.Id==1);
                    if(cv is true)
                    {
                        var cv1 = db.StockPrice.OrderBy(x => x.Id).Last();
                        sp.Id = cv1.Id + 1;
                    }
                    else
                    {
                        sp.Id = 1;
                    }
                    sp.PId = Convert.ToInt32(ch.PId);
                    sp.Pack = Convert.ToInt32(pro.PPack);
                    sp.Qty = 1;
                    var cx = Convert.ToDouble(ch.baseprltr) / 1;
                    sp.Price = Convert.ToInt32(cx * pro.PPack);
                    sp.Pono = id;
                    sp.Date = DateTime.Now.Day;
                    sp.Month = DateTime.Now.Month;
                    sp.Year = DateTime.Now.Year;
                    sp.Status = "active";
                    db.StockPrice.Add(sp);
                    db.SaveChanges();
                }

                if (chk is true)
                {
                    var chk1 = db.Stocks.Where(x => x.PId == pro.PId).First();
                    chk1.SQty = chk1.SQty + ch.PoQty;
                    db.SaveChanges();
                }
                else
                {
                    Stock s = new Stock();
                    s.PName = pro.PName;
                    s.PPack = pro.PPack;
                    s.SQty = ch.PoQty;
                    s.PId = pro.PId;
                    db.Stocks.Add(s);
                    db.SaveChanges();
                }
                Productledger p = new Productledger();
                var x2 = db.Venders.Where(x => x.VId == ch.VId).First();
                p.CName = x2.VName;
                p.pono = id;
                p.Type = "primary";
                p.PlDate = DateTime.Now;
                p.Pid = ch.PId;
                p.PlIn = ch.PoQty;
                p.PlOut = 0;
                p.month = Convert.ToString(DateTime.Now.Month);
                p.day = DateTime.Now.Day;
                p.year = DateTime.Now.Year;
                p.time = DateTime.Now.ToShortTimeString();
                var cc1 = db.Productledgers.Any(x=>x.Pid ==  ch.PId);
                if(cc1 is true)
                {
                    var cc2 = db.Productledgers.Where(x => x.Pid == ch.PId).OrderBy(x=>x.PlId).Last();
                    p.PlBalance = cc2.PlBalance + ch.PoQty;
                    db.Productledgers.Add(p);
                    db.SaveChanges();
                }
                else
                {
                    p.PlBalance = ch.PoQty;
                    db.Productledgers.Add(p);
                    db.SaveChanges();
                }

                Primarysale ps = new Primarysale();
                var b = db.Grns.OrderBy(x => x.GrnId).Last();
				var r1 = Convert.ToDouble(pro.PPr * 1.00 / pro.PPack * 1.00);
				var pprint1 = Convert.ToInt32(r1 * 100);
				var pprfloat1 = Convert.ToDouble(pprint1 * 1.00 / 100.00);
				ps.VName = x2.VName;
                ps.PName = Convert.ToString(ch.PId);
                ps.DcNo = b.GrnDc;
                ps.PsPrice = pro.PPr;
                ps.PsQty = ch.PoQty;
                ps.PsPack = pro.PPack;
                ps.PsDate = b.GrnDate;
                ps.date = DateTime.Now.Date;
                ps.day = DateTime.Now.Day;
                ps.year = DateTime.Now.Year;
                ps.month = Convert.ToString(DateTime.Now.Month);
                ps.time = DateTime.Now.ToShortTimeString();
                ps.Price = ch.baseprltr;
                ps.Total = Convert.ToString(ch.PoPrice*ch.PoQty);
                ps.OrderID = xx.PoUnique;
                db.Primarysales.Add(ps);



                var ven = db.Vendorledger.Any(x => x.VName == x2.VName);
                if (ven is true)
                {
                    var ven1 = db.Vendorledger.Where(x => x.VName == x2.VName).OrderBy(x => x.VlId).Last();
                    Vendorledger vl = new Vendorledger();
                    vl.VName = x2.VName;
                    vl.Description = pro.PName;
                    vl.VlIn = ch.PoPrice * ch.PoQty;
                    vl.VlOut = 0;
                    vl.Type = "primary";
                    vl.date = DateTime.Now.Date;
                    vl.Qty = ch.PoQty * pro.PPack;
                    vl.VlBalance = ven1.VlBalance + Convert.ToInt32(ch.PoPrice * ch.PoQty);
                    vl.day = DateTime.Now.Day;
                    vl.year = DateTime.Now.Year;
                    vl.month = DateTime.Now.Month;
                    vl.time = DateTime.Now.ToShortTimeString();
                    vl.Orderid = ch.PoUnique;
                    db.Vendorledger.Add(vl);
                    db.SaveChanges();
                }
                else
                {
                    Vendorledger vl = new Vendorledger();
                    vl.VName = x2.VName;
                    vl.Description = pro.PName;
                    vl.VlIn = ch.PoPrice * ch.PoQty;
                    vl.VlOut = 0;
                    vl.Type = "primary";
                    vl.date = DateTime.Now.Date;
                    vl.Qty = ch.PoQty * pro.PPack;
                    vl.VlBalance =  Convert.ToInt32(ch.PoPrice * ch.PoQty);
                    vl.day = DateTime.Now.Day;
                    vl.year = DateTime.Now.Year;
                    vl.month = DateTime.Now.Month;
                    vl.time = DateTime.Now.ToShortTimeString();
                    vl.Orderid = ch.PoUnique;
                    db.Vendorledger.Add(vl);
                    db.SaveChanges();
                }
            }
            Da d = new Da();
            var ch1 = db.Pos.Where(x => x.PoUnique == id).First();
            var p1 = db.Products.Where(x => x.PId == ch1.PId).First();
            var das_chk = db.Das.Any(x => x.DasType != "Pay");
            var b1 = db.Grns.OrderBy(x => x.GrnId).Last();
            if (das_chk is true)
            {
                var das_chk2 = db.Das.Where(x => x.DasType != "Pay").OrderBy(x => x.DasId).Last();
                d.DasDate = b1.GrnDate;
                d.day = Convert.ToString(DateTime.Now.Day);
                d.month = Convert.ToString(DateTime.Now.Month);
                d.year = Convert.ToString(DateTime.Now.Year);
                d.DasDescrition = xx.VIdNavigation.VName;
                d.DasExpense = "";
                d.typ1 = "primary";
                d.order_id = id;
                d.DasDeit = 0;
                d.DasCredit = credit;
                d.DasBalance = das_chk2.DasBalance + credit;
                d.DasType = "p";
                db.Das.Add(d);
            }
            else
            {
                d.DasDate = b1.GrnDate;
                d.day = Convert.ToString(DateTime.Now.Day);
                d.month = Convert.ToString(DateTime.Now.Month);
                d.year = Convert.ToString(DateTime.Now.Year);
                d.DasDescrition = xx.VIdNavigation.VName;
                d.DasExpense = "";
                d.DasDeit = 0;
                d.typ1 = "primary";
                d.order_id = id;
                d.DasCredit = credit;
                d.DasBalance = credit;
                d.DasType = "p";
                db.Das.Add(d);
            }
            db.SaveChanges();
            return RedirectToAction("po", "front");
        }
		public IActionResult det_po(int id)
		{
			var r = db.PoNos.OrderBy(x => x.PonoId).Last();
			var x = db.Pos.Where(x => x.PoUnique == r.PonoId).ToList();
            var res = db.Products.Select(x => new SelectListItem { Text = x.PName + " , " + x.PPack + "L ", Value = Convert.ToString(x.PId) });
            ViewBag.PId = res;
            ViewBag.id = id;
            return View();
		}
        [HttpPost]
        public IActionResult det_po(Po p,int id)
        {
            if (p.PoQty <= 0)
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("det_po");
            }
            var x1 = db.Pos.Any(x => x.PoUnique == p.PoUnique);
            if(x1 is true)
            {
                var x = db.Pos.Where(x => x.PoUnique == p.PoUnique).First();
                var r = db.Pos.Where(x => x.PoUnique == p.PoUnique);
                var r1 = db.Products.Where(x => x.PId == x.PId).First();
                var x2 = db.Pos.Any(x => x.PoUnique == p.PoUnique && x.PId == p.PId);
                if (x2 is true)
                {
                    var x21 = db.Pos.Where(x => x.PoUnique == p.PoUnique && x.PId == p.PId).First();
                    x21.PoQty = x21.PoQty + p.PoQty;
                    var c = x21.PoQty + p.PoQty;
					var r11 = db.Products.Where(x => x.PId == p.PId).First();
                    var disc = Convert.ToInt32(r11.Trade_disc) * r11.PPack;
                }
                else
                {
                    var count = 0;
                    foreach (var item in r)
                    {
                        count++;
                    }
                    var rj = db.Products.Where(x => x.PId == p.PId).First();

                    var rk = Convert.ToDouble(rj.PPr * 1.00 / rj.PPack * 1.00);
                    var pprint = Convert.ToInt64(rk * 100);
                    var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);
					var ppr1 = pprfloat - Convert.ToDouble(rj.Trade_disc);

					p.baseprltr = Convert.ToString(ppr1);
					var discount = Convert.ToInt32(rj.Trade_disc) * rj.PPack;
					p.PoPrice = rj.PPr - discount;

					p.VId = x.VId;
                    p.PoDate = x.PoDate;
                    p.PoUnique = x.PoUnique;
                    var r11 = db.Products.Where(x => x.PId == p.PId).First();
                    p.Count = count + 1;
                    db.Pos.Add(p);
                }

                p.day = DateTime.Now.Day;
                p.month = DateTime.Now.Month;
                p.year = DateTime.Now.Year;
                p.time = DateTime.Now.ToShortTimeString();
                db.SaveChanges();
                TempData["id"] = p.PoUnique;
                return RedirectToAction("po", "det");
            }
            else
            {
                var ch = db.PoNos.Where(x => x.PonoId == p.PoUnique).First();
                var r1 = db.Products.Where(x => x.PId == p.PId).First();
                var r = Convert.ToDouble(r1.PPr * 1.00 / r1.PPack * 1.00);
                var pprint = Convert.ToInt64(r * 100);
                var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);

                p.VId = Convert.ToInt32(ch.Vendor);
                p.PoDate = DateTime.Now;
                p.PoPrice = r1.PPr;
                p.baseprltr = Convert.ToString(pprfloat);
                p.Count = 1;
                p.day = DateTime.Now.Day;
                p.month = DateTime.Now.Month;
                p.year = DateTime.Now.Year;
                p.time = DateTime.Now.ToShortTimeString();
                db.Pos.Add(p);
                db.SaveChanges();
                TempData["id"] = p.PoUnique;
                return RedirectToAction("po", "det");
            }
        }
        public IActionResult det_order(int id)
        {
            var r = db.OrderNos.OrderBy(x => x.OrdernoId).Last();
            var x = db.Order.Where(x => x.OrUnique == r.OrdernoId).ToList();
            var res = db.Products.Select(x => new SelectListItem { Text = x.PName + " , " + x.PPack + "L ", Value = Convert.ToString(x.PId) });
            ViewBag.PId = res;
            if(id == 0)
            {
                ViewBag.id = TempData["id"];
            }
            else
            {
                ViewBag.id = id;
            }
            return View();
        }
        [HttpPost]
        public IActionResult det_order(Order o)
        {
            int id = Convert.ToInt32(o.OrUnique);
            if (o.Qty <= 0)
            {
                TempData["data"] = o.OrUnique;
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("det_order");
            }
            if (id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]);
            }
            var x1 = db.Order.Any(x => x.OrUnique == o.OrUnique);
            if(x1 is true)
            {
                var chk = db.Order.ToList();
                var x = db.Order.Where(x => x.OrUnique == o.OrUnique).First();
                var r = db.Order.Where(x => x.OrUnique == o.OrUnique);
                var r1 = db.Products.Where(x => x.PId == x.PId).First();
                var x2 = db.Order.Any(x => x.OrUnique == o.OrUnique && x.PId == o.PId);
                if (x2 is true)
                {
                    var x21 = db.Order.Where(x => x.OrUnique == o.OrUnique && x.PId == o.PId).First();
                    x21.Qty = x21.Qty + o.Qty;
                }
                else
                {
                    var po = db.OrderNos.Where(x => x.OrdernoId == o.OrUnique).First();
                    var cus_pro = db.cus_pro.Any(x => x.cid == po.Customer && x.pid == o.PId);
                    if(cus_pro is true)
                    {
                        var cus_pro1 = db.cus_pro.Where(x => x.cid == po.Customer && x.pid == o.PId).First();
                        var count = 0;
                        foreach (var item in r)
                        {
                            count++;
                        }
                        if(cus_pro1.baseprice == 0)
                        {
                            var prod = db.Products.Where(x => x.PId == o.PId).First();
                            o.OPrice = prod.PSp;
                        }
                        else
                        {
                            o.OPrice = cus_pro1.baseprice;
                        }
                        o.CId = x.CId;
                        o.OrUnique = x.OrUnique;
                        o.Count = count;
                        o.day = DateTime.Now.Day;
                        o.month = DateTime.Now.Month;
                        o.year = DateTime.Now.Year;
                        o.time = DateTime.Now.ToShortTimeString();
                        o.delivered = 0;
                        o.type = "Drum";
                        db.Order.Add(o);
                        db.SaveChanges();
                    }
                    else
                    {
                        var count = 0;
                        foreach (var item in r)
                        {
                            count++;
                        }
                        var prod = db.Products.Where(x => x.PId == o.PId).First();
                        o.OPrice = prod.PSp;
                        o.CId = x.CId;
                        o.OrUnique = x.OrUnique;
                        o.Count = count;
                        o.day = DateTime.Now.Day;
                        o.month = DateTime.Now.Month;
                        o.year = DateTime.Now.Year;
                        o.time = DateTime.Now.ToShortTimeString();
                        o.delivered = 0;
                        o.type = "Drum";
                        db.Order.Add(o);
                        db.SaveChanges();
                    }
                }
            }
            else if(x1 is false)
            {
                var po = db.OrderNos.Where(x => x.OrdernoId == o.OrUnique).First();
                var cus_pro = db.cus_pro.Any(x => x.cid == po.Customer && x.pid == o.PId);
                var pro = db.Products.Where(x => x.PId == o.PId).First();
                if (cus_pro is true)
                {
                    var cus_pro1 = db.cus_pro.Where(x => x.cid == po.Customer && x.pid == o.PId).First();
                    if (cus_pro1.baseprice != 0)
                    {
                        o.OPrice = cus_pro1.baseprice;
                        o.CId = po.Customer;
                        o.OrUnique = po.OrdernoId;
                        o.Count = 1;
                        o.day = DateTime.Now.Day;
                        o.month = DateTime.Now.Month;
                        o.year = DateTime.Now.Year;
                        o.time = DateTime.Now.ToShortTimeString();
                        o.delivered = 0;
                        o.type = "Drum";
                        db.Order.Add(o);
                        db.SaveChanges();
                    }
                    else
                    {
                        o.OPrice = pro.PSp;
                        o.CId = po.Customer;
                        o.OrUnique = po.OrdernoId;
                        o.Count = 1;
                        o.day = DateTime.Now.Day;
                        o.month = DateTime.Now.Month;
                        o.year = DateTime.Now.Year;
                        o.time = DateTime.Now.ToShortTimeString();
                        o.delivered = 0;
                        o.type = "Drum";
                        db.Order.Add(o);
                        db.SaveChanges();
                    }
                }
                else
                {
                    o.OPrice = pro.PSp;
                    o.CId = po.Customer;
                    o.OrUnique = po.OrdernoId;
                    o.Count = 1;
                    o.day = DateTime.Now.Day;
                    o.month = DateTime.Now.Month;
                    o.year = DateTime.Now.Year;
                    o.time = DateTime.Now.ToShortTimeString();
                    o.delivered = 0;
                    o.type = "Drum";
                    db.Order.Add(o);
                    db.SaveChanges();

                }
            }
            db.SaveChanges();
            TempData["id"] = id;
            return RedirectToAction("order", "det");
        }
        public IActionResult search_ps()
        {
            var res1 = db.Venders.Select(x => new SelectListItem { Text = x.VName, Value = Convert.ToString(x.VId) });
            var res = db.Products.Select(x => new SelectListItem { Text = x.PName, Value = Convert.ToString(x.PId) });
            ViewBag.PId = res;
            ViewBag.VId = res1;
            return View();
        }
        public IActionResult search_ss()
        {
            var res1 = db.Customers.Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            var res = db.Products.Select(x => new SelectListItem { Text = x.PName, Value = Convert.ToString(x.PId) });
            ViewBag.PId = res;
            ViewBag.CId = res1;
            return View();
        }
        public IActionResult search_dp()
        {
            return View();
        }
        public IActionResult order()
        {
            var res1 = db.Customers.Where(x=>x.CStatus == "Active").OrderBy(x => x.CName).Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            ViewBag.Cid = res1;
            return View();
        }
        [HttpPost]
        public IActionResult order(OrderNo o)
        {
            o.OrdernoStatus = "no";
            db.OrderNos.Add(o);
            db.SaveChanges();

            var x = db.OrderNos.OrderBy(x => x.OrdernoId).Last();
            Invoice i = new Invoice();
            i.SsOrderno = x.OrdernoId;
            i.InDate = DateTime.Now.Date;
            i.InDate1 = DateTime.Now.ToString("dd-MM-yyyy");
            db.Invoices.Add(i);
            db.SaveChanges();
            return RedirectToAction("order_pro");
        }
        public IActionResult order_pro()
        {
            var res1 = db.Products.Select(x => new SelectListItem { Text = x.PName + " , " + x.PPack + "L ", Value = Convert.ToString(x.PId) });
            ViewBag.Pid = res1;
            return View();
        }
        [HttpPost]
        public IActionResult order_pro(Order o,int id,int? price)
        {
            if (o.Qty <= 0 )
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("order_pro");
            }
            var r = db.OrderNos.OrderBy(x => x.OrdernoId).Last();
            var x21 = db.Order.Any(x => x.OrUnique == r.OrdernoId && x.PId == o.PId);
            if (x21 == true)
            {
                var x211 = db.Order.Where(x => x.OrUnique == r.OrdernoId && x.PId == o.PId).First();
                x211.Qty = x211.Qty + o.Qty;
            }
            else
            {
                var cus = db.cus_pro.Any(x => x.cid == r.Customer && x.pid == o.PId);
                if(cus is true)
                {
                    var cus1 = db.cus_pro.Where(x => x.cid == r.Customer && x.pid == o.PId).First();
                    o.OrUnique = r.OrdernoId;
                    var x = db.Customers.Where(x => x.CId == Convert.ToInt32(r.Customer)).First();
                    o.CId = x.CId;
                    o.day = DateTime.Now.Day;
                    o.month = DateTime.Now.Month;
                    o.year = DateTime.Now.Year;
                    o.time = DateTime.Now.ToShortTimeString();
                    o.delivered = 0;
                    o.type = "Drum";
                    if(cus1.baseprice == 0)
                    {
                        var pr = db.Products.Where(x => x.PId == o.PId).First();
                        o.OPrice = pr.PSp;
                    }
                    else
                    {
                        o.OPrice = cus1.baseprice;
                    }
                    var c = db.Order.Where(x => x.OrUnique == r.OrdernoId).Count();
                    if (c == 0)
                    {
                        o.Count = 1;
                    }
                    else if (c > 0)
                    {
                        var cc = db.Order.Where(x => x.OrUnique == r.OrdernoId).Count();
                        o.Count = cc + 1;
                    }
                    db.Order.Add(o);
                }
                else
                {
                    o.OrUnique = r.OrdernoId;
                    var x = db.Customers.Where(x => x.CId == Convert.ToInt32(r.Customer)).First();
                    o.CId = x.CId;
                    o.day = DateTime.Now.Day;
                    o.month = DateTime.Now.Month;
                    o.year = DateTime.Now.Year;
                    o.time = DateTime.Now.ToShortTimeString();
                    o.delivered = 0;
                    o.type = "Drum";
                    var pr = db.Products.Where(x => x.PId == o.PId).First();
                    o.OPrice = pr.PSp;
                    var c = db.Order.Where(x => x.OrUnique == r.OrdernoId).Count();
                    if (c == 0)
                    {
                        o.Count = 1;
                    }
                    else if (c > 0)
                    {
                        var cc = db.Order.Where(x => x.OrUnique == r.OrdernoId).Count();
                        o.Count = cc + 1;
                    }
                    db.Order.Add(o);
                }
            }
            db.SaveChanges();
            if (id == 1)
            {
                return RedirectToAction("order_pro");
            }
            else
            {
                return RedirectToAction("orno", "front");
            }
            return View();
        }
        public IActionResult ss(int id)
        {
            var x = db.Order.Where(x=>x.OId == id).First();
            var r = x.Qty - x.delivered;
            ViewBag.qty = r;
            return View(x);
        }
        [HttpPost]
        public IActionResult ss(int id,int qty)
        {
            if(qty ==0)
            {
                TempData["mess"] = "Enter Integer Value";
                return RedirectToAction("ss");
            }
            var x = db.Order.Where(x => x.OId == id).First();
            var pro = db.Products.Where(x => x.PId == x.PId).First();
            var plx1 = db.Productledgers.OrderBy(x => x.PlId);
            var pid1 = 0;
            foreach (var item in plx1)
            {
                if (item.Pid == x.PId)
                {
                    pid1 = Convert.ToInt32(item.PlId);
                }
            }
            var v = db.Productledgers.Any(x => x.PlId == pid1);
            if (v is true)
            {
                var pl21 = db.Productledgers.Where(x => x.PlId == pid1).First();
                var r = x.Qty - x.delivered;
                if (qty <= r)
                {
                    if (qty <= pl21.PlBalance)
                    {
                        x.delivered = x.delivered + qty;
                        Secondarysale ss = new Secondarysale();
                        var x1 = db.Products.Where(x => x.PId == x.PId).First();

                        var rx = Convert.ToDouble(x1.PSp * 1.00 / x1.PPack * 1.00);
                        var pprint = Convert.ToInt64(rx * 100);
                        var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);
                        ss.PId = x.PId;
                        ss.CId = x.CId;
                        ss.SsQty = qty;
                        ss.Status = 0;
                        ss.date = DateTime.Now.Date;
                        ss.SsOrderno = x.OrUnique;
                        ss.Day = DateTime.Now.Day;
                        ss.Month = DateTime.Now.Month;
                        ss.Year = DateTime.Now.Year;
                        ss.Time = DateTime.Now.ToShortTimeString();
                        ss.Pack = Convert.ToString(pro.PPack);
                        ss.Type = "Drum";
                        ss.Price = Convert.ToInt32(pprfloat);
                        ss.TPrice = x.OPrice;
                        db.Secondarysales.Add(ss);


                        var xv = db.Order.Where(x => x.OId == id).First();
                        var ven = db.Customerledger.Any(x => x.CName == Convert.ToString(xv.CId));
                        if (ven is true)
                        {
                            var ven1 = db.Customerledger.Where(x => x.CName == Convert.ToString(xv.CId)).OrderBy(x => x.ClId).Last();
                            Customerledger vl = new Customerledger();
                            vl.CName = Convert.ToString(xv.CId);
                            vl.Description = Convert.ToString(x.PId);
                            vl.type = "Drum";
                            vl.ClIn = x.OPrice * qty ;
                            vl.ClOut = 0;
                            vl.Qty = qty * pro.PPack;
                            vl.date = DateTime.Now.Date;
                            vl.ClBalance = ven1.ClBalance + Convert.ToInt32(x.OPrice * qty);
                            vl.day = DateTime.Now.Day;
                            vl.year = DateTime.Now.Year;
                            vl.month = DateTime.Now.Month;
                            vl.time = DateTime.Now.ToShortTimeString();
                            vl.Orderid = x.OrUnique;
                            db.Customerledger.Add(vl);
                            db.SaveChanges();
                        }
                        else
                        {
                            Customerledger vl = new Customerledger();
                            vl.CName = Convert.ToString(xv.CId);
                            vl.Description = Convert.ToString(x.PId);
                            vl.ClIn = x.OPrice * qty ;
                            vl.ClOut = 0;
                            vl.Qty = qty * pro.PPack;
                            vl.type = "Drum";
                            vl.date = DateTime.Now.Date;
                            vl.ClBalance = Convert.ToInt32(x.OPrice * qty);
                            vl.day = DateTime.Now.Day;
                            vl.year = DateTime.Now.Year;
                            vl.month = DateTime.Now.Month;
                            vl.time = DateTime.Now.ToShortTimeString();
                            vl.Orderid = x.OrUnique;
                            db.Customerledger.Add(vl);
                            db.SaveChanges();
                        }



                        Da d = new Da();
                        var cus = db.Customers.Where(x1 => x1.CId == x.CId).First();
                        var das = db.Das.Any(x => x.DasDescrition == cus.CName && x.day == Convert.ToString(DateTime.Now.Day) && x.month == Convert.ToString(DateTime.Now.Month) && x.year == Convert.ToString(DateTime.Now.Year) && x.DasType == "Drum");
                        if (das == true)
                        {
                            var das1 = db.Das.Where(x => x.DasDescrition == cus.CName && x.day == Convert.ToString(DateTime.Now.Day) && x.month == Convert.ToString(DateTime.Now.Month) && x.DasType == "Drum" && x.year == Convert.ToString(DateTime.Now.Year)).First();
                            das1.DasDeit = das1.DasDeit + (qty * pro.PPack);
                            das1.DasBalance = das1.DasBalance - (qty * pro.PPack);
                            db.SaveChanges();
                        }
                        else
                        {
                            var dd1 = db.Das.Any(x => x.DasType == "Drum" || x.DasType == "p");
                            if (dd1 is true)
                            {
                                var dd = db.Das.Where(x => x.DasType == "Drum" || x.DasType == "p").OrderBy(x => x.DasId).Last();
                                d.DasDate = DateTime.Now;
                                d.DasDescrition = cus.CName;
                                d.DasExpense = "";
                                d.DasDeit = qty * pro.PPack;
                                d.DasCredit = 0;
                                d.DasBalance = dd.DasBalance - (qty * pro.PPack);
                                d.DasType = "Drum";
                                d.day = Convert.ToString(DateTime.Now.Day);
                                d.month = Convert.ToString(DateTime.Now.Month);
                                d.year = Convert.ToString(DateTime.Now.Year);
                                db.Das.Add(d);
                            }
                            else
                            {
                                d.DasDate = DateTime.Now;
                                d.DasDescrition = cus.CName;
                                d.DasExpense = "";
                                d.DasDeit = qty * pro.PPack;
                                d.DasCredit = 0;
                                d.DasBalance = -(qty * pro.PPack);
                                d.DasType = "Drum";
                                d.day = Convert.ToString(DateTime.Now.Day);
                                d.month = Convert.ToString(DateTime.Now.Month);
                                d.year = Convert.ToString(DateTime.Now.Year);
                                db.Das.Add(d);

                            }

                        }


                        Productledger pl = new Productledger();
                        var pl1 = db.Productledgers.Any(x => x.Pid == x.Pid);
                        if (pl1 == true)
                        {
                            var plx = db.Productledgers.OrderBy(x => x.PlId);
                            var pid = 0;
                            var prod = x.PId;
                            foreach (var item in plx)
                            {
                                if (item.Pid == x.PId)
                                {
                                    pid = Convert.ToInt32(item.PlId);
                                }
                            }
                            var pl2 = db.Productledgers.Where(x => x.PlId == pid).First();
                            pl.CName = cus.CName;
                            pl.Pid = x.PId;
                            pl.PlDate = DateTime.Now;
                            pl.PlIn = 0;
                            pl.PlOut = qty;
                            pl.PlBalance = pl2.PlBalance - qty;
                            pl.day = DateTime.Now.Day;
                            pl.month = Convert.ToString(DateTime.Now.Month);
                            pl.year = DateTime.Now.Year;
                            pl.time = DateTime.Now.ToShortTimeString();
                            var stock = db.Stocks.Where(x => x.PId == prod).First();
                            stock.SQty = stock.SQty - qty;
                            db.SaveChanges();
                            db.Productledgers.Add(pl);
                            db.SaveChanges();
                        }
                        else
                        {
                            pl.CName = cus.CName;
                            pl.Pid = x.PId;
                            pl.PlDate = DateTime.Now;
                            pl.PlIn = 0;
                            pl.PlOut = qty;
                            pl.PlBalance = -1 * qty;
                            pl.day = DateTime.Now.Day;
                            pl.month = Convert.ToString(DateTime.Now.Month);
                            pl.year = DateTime.Now.Year;
                            pl.time = DateTime.Now.ToShortTimeString();
                            db.Productledgers.Add(pl);
                            db.SaveChanges();
                        }

                    }
                    else
                        {
                            TempData["mess"] = "The Entered Qty  Not Available In Stock";
                            return RedirectToAction("ss");
                        }

                }
                else
                {
                    TempData["mess"] = "Enter Correct Qty";
                    return RedirectToAction("ss");
                }
            }
            else
            {
                TempData["mess"] = "The Entered Product Not Available In Stock";
                return RedirectToAction("ss");
            }
            
            return RedirectToAction("orno", "front");
        }
        public IActionResult ss1(int id)
        {
            var x = db.Order.Where(x => x.OId == id).First();
            var r = x.Qty - x.delivered;
            ViewBag.qty = r;
            return View(x);
        }
        [HttpPost]
        public IActionResult ss1(int id, int qty)
        {
            if (qty <= 0)
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("ss1");
            }
            var x = db.Order.Where(x => x.OId == id).First();
            var pro = db.Products.Where(x => x.PId == x.PId).First();
            var plx1 = db.Productledgers.OrderBy(x => x.PlId);
            var pid1 = 0;
            foreach (var item in plx1)
            {
                if (item.Pid == x.PId)
                {
                    pid1 = Convert.ToInt32(item.PlId);
                }
            }

            var v = db.Productledgers.Any(x => x.PlId == pid1);
            if (v is true)
            {
                var pl21 = db.Productledgers.Where(x => x.PlId == pid1).First();
                var r = x.Qty - x.delivered;
                if (qty <= r)
                {
                    if (qty <= pl21.PlBalance)
                    {
                        x.delivered = x.delivered + qty;
                        Secondarysale ss = new Secondarysale();
                        var x1 = db.Products.Where(x => x.PId == x.PId).First();

                        var rx = Convert.ToDouble(x1.PSp * 1.00 / x1.PPack * 1.00);
                        var pprint = Convert.ToInt64(rx * 100);
                        var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);
                        var pr_ltr = x.OPrice / x1.PPack;
                        var pr_price = x1.PPr - (Convert.ToInt32(x1.Trade_disc) * 208);
                        ss.PId = x.PId;
                        ss.CId = x.CId;
                        ss.SsQty = qty;
                        ss.Status = 0;
                        ss.Pr_Price = pr_price;
                        ss.date = DateTime.Now.Date;
                        ss.SsOrderno = x.OrUnique;
                        ss.Day = DateTime.Now.Day;
                        ss.Month = DateTime.Now.Month;
                        ss.Year = DateTime.Now.Year;
                        ss.Time = DateTime.Now.ToShortTimeString();
                        ss.Pack = Convert.ToString(pro.PPack);
                        ss.Type = "Drum";
                        ss.Price = Convert.ToInt32(pr_ltr);
                        ss.TPrice = x.OPrice;
                        db.Secondarysales.Add(ss);
                        db.SaveChanges();
                        var last = db.Secondarysales.OrderBy(x => x.SsId).Last();
                        var prc = db.Products.Where(x => x.PId == last.PId).First();
                        var pr_price1 = prc.PPr - (Convert.ToInt32(prc.Trade_disc) * 208);
                        last.Pr_Price = pr_price1;
                        db.SaveChanges();
                        var ssid = db.Secondarysales.OrderBy(x => x.SsId).Last();

                        var xv = db.Order.Where(x => x.OId == id).First();
                        var ven = db.Customerledger.Any(x => x.CName == Convert.ToString(xv.CId));
                        if (ven is true)
                        {
                            var ven1 = db.Customerledger.Where(x => x.CName == Convert.ToString(xv.CId)).OrderBy(x => x.ClId).Last();
                            Customerledger vl = new Customerledger();
                            vl.CName = Convert.ToString(xv.CId);
                            vl.Description = Convert.ToString(x.PId);
                            vl.type = "Drum";
                            vl.ClIn = x.OPrice * qty ;
                            vl.ClOut = 0;
                            vl.Ss_id = ssid.SsId;
                            vl.Qty = qty * pro.PPack;
                            vl.date = DateTime.Now.Date;
                            vl.ClBalance = ven1.ClBalance + Convert.ToInt32(x.OPrice * qty);
                            vl.day = DateTime.Now.Day;
                            vl.year = DateTime.Now.Year;
                            vl.month = DateTime.Now.Month;
                            vl.time = DateTime.Now.ToShortTimeString();
                            vl.Orderid = x.OrUnique;
                            db.Customerledger.Add(vl);
                            db.SaveChanges();
                        }
                        else
                        {
                            Customerledger vl = new Customerledger();
                            vl.CName = Convert.ToString(xv.CId);
                            vl.Description = Convert.ToString(x.PId);
                            vl.ClIn = x.OPrice * qty ;
                            vl.ClOut = 0;
                            vl.Qty = qty * pro.PPack;
                            vl.type = "Drum";
                            vl.Ss_id = ssid.SsId;
                            vl.date = DateTime.Now.Date;
                            vl.ClBalance = Convert.ToInt32(x.OPrice * qty);
                            vl.day = DateTime.Now.Day;
                            vl.year = DateTime.Now.Year;
                            vl.month = DateTime.Now.Month;
                            vl.time = DateTime.Now.ToShortTimeString();
                            vl.Orderid = x.OrUnique;
                            db.Customerledger.Add(vl);
                            db.SaveChanges();
                        }



                        Da d = new Da();
                        var cus = db.Customers.Where(x1 => x1.CId == x.CId).First();
                        var das = db.Das.Any(x => x.DasDescrition == cus.CName && x.day == Convert.ToString(DateTime.Now.Day) && x.month == Convert.ToString(DateTime.Now.Month) && x.year == Convert.ToString(DateTime.Now.Year) && x.DasType == "Drum");
                        if (das == true)
                        {
                            var das1 = db.Das.Where(x => x.DasDescrition == cus.CName && x.day == Convert.ToString(DateTime.Now.Day) && x.month == Convert.ToString(DateTime.Now.Month) && x.DasType == "Drum" && x.year == Convert.ToString(DateTime.Now.Year)).First();
                            das1.DasDeit = das1.DasDeit + (qty * pro.PPack);
                            das1.DasBalance = das1.DasBalance - (qty * pro.PPack);
                            db.SaveChanges();

                            var das11 = db.Das.Where(x => x.DasType != "Pay").ToList();
                            var count2 = 0;
                            foreach (var item in das11)
                            {
                                item.DasBalance = count2 + item.DasCredit - item.DasDeit;
                                count2 = Convert.ToInt32(item.DasBalance);
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            var dd1 = db.Das.Any(x => x.DasType == "Drum" || x.DasType == "p");
                            if (dd1 is true)
                            {
                                var dd = db.Das.Where(x => x.DasType == "Drum" || x.DasType == "p").OrderBy(x => x.DasId).Last();
                                d.DasDate = DateTime.Now;
                                d.DasDescrition = cus.CName;
                                d.DasExpense = "";
                                d.DasDeit = qty * pro.PPack;
                                d.order_id = Convert.ToInt32(x.OrUnique);
                                d.typ1 = "secondary";
                                d.DasCredit = 0;
                                d.DasBalance = dd.DasBalance - (qty * pro.PPack);
                                d.DasType = "Drum";
                                d.day = Convert.ToString(DateTime.Now.Day);
                                d.month = Convert.ToString(DateTime.Now.Month);
                                d.year = Convert.ToString(DateTime.Now.Year);
                                db.Das.Add(d);
                            }
                            else
                            {
                                d.DasDate = DateTime.Now;
                                d.DasDescrition = cus.CName;
                                d.DasExpense = "";
                                d.DasDeit = qty * pro.PPack;
                                d.DasCredit = 0;
                                d.order_id = Convert.ToInt32(x.OrUnique);
                                d.typ1 = "secondary";
                                d.DasBalance = -(qty * pro.PPack);
                                d.DasType = "Drum";
                                d.day = Convert.ToString(DateTime.Now.Day);
                                d.month = Convert.ToString(DateTime.Now.Month);
                                d.year = Convert.ToString(DateTime.Now.Year);
                                db.Das.Add(d);

                            }

                        }


                        Productledger pl = new Productledger();
                        var pl1 = db.Productledgers.Any(x => x.Pid == x.Pid);
                        var prod = x.PId;
                        if (pl1 == true)
                        {
                            var plx = db.Productledgers.OrderBy(x => x.PlId);
                            var pid = 0;
                            foreach (var item in plx)
                            {
                                if (item.Pid == x.PId)
                                {
                                    pid = Convert.ToInt32(item.PlId);
                                }
                            }
                            var pl2 = db.Productledgers.Where(x => x.PlId == pid).First();
                            pl.CName = cus.CName;
                            pl.Pid = x.PId;
                            pl.PlDate = DateTime.Now;
                            pl.PlIn = 0;
                            pl.PlOut = qty;
                            pl.pono = x.OrUnique;
                            pl.Ss_id = ssid.SsId;
                            pl.Type = "secondary";
                            pl.PlBalance = pl2.PlBalance - qty;
                            pl.day = DateTime.Now.Day;
                            pl.month = Convert.ToString(DateTime.Now.Month);
                            pl.year = DateTime.Now.Year;
                            pl.time = DateTime.Now.ToShortTimeString();

                            var stock = db.Stocks.Where(x => x.PId == prod).First();
                            stock.SQty = stock.SQty - qty;
                            db.SaveChanges();
                            db.Productledgers.Add(pl);
                            db.SaveChanges();
                        }
                        else
                        {
                            pl.CName = cus.CName;
                            pl.Pid = x.PId;
                            pl.Ss_id = ssid.SsId;
                            pl.PlDate = DateTime.Now;
                            pl.PlIn = 0;
                            pl.PlOut = qty;
                            pl.PlBalance = -1 * qty;
                            pl.day = DateTime.Now.Day;
                            pl.month = Convert.ToString(DateTime.Now.Month);
                            pl.year = DateTime.Now.Year;
                            pl.time = DateTime.Now.ToShortTimeString();
                            db.Productledgers.Add(pl);
                            db.SaveChanges();
                        }
                        for (int j = 1; j <= qty; j++)
                        {
                            var sp = db.StockPrice.Where(x => x.PId == prod && x.Status == "active").First();
                            sp.Status = "del";
                            sp.Date = DateTime.Now.Day;
                            sp.Month = DateTime.Now.Month;
                            sp.Year = DateTime.Now.Year;
                            sp.Order_id = Convert.ToInt32(x.OrUnique);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        TempData["mess"] = "The Entered Qty  Not Available In Stock";
                        return RedirectToAction("ss1");
                    }

                }
                else
                {
                    TempData["mess"] = "Enter Correct Qty";
                    return RedirectToAction("ss1");
                }
            }
            else
            {
                TempData["mess"] = "The Entered Product Not Available In Stock";
                return RedirectToAction("ss1");
            }

            return RedirectToAction("pending", "front");
        }
        public IActionResult search_pending()
        {
            var res1 = db.Products.Select(x => new SelectListItem { Text = x.PName, Value = Convert.ToString(x.PId) });
            ViewBag.PId = res1;
            return View();
        }
        public IActionResult loose_drum_manual()
        {
            return View();
        }
        [HttpPost]
        public IActionResult loose_drum_manual(int pid,int cid,int qty)
        {
            int price = 0;
            var cus = db.Customers.Where(x => x.CId == cid).First();
            var loose = db.looseledger.Where(x => x.Description == Convert.ToString(pid)).OrderBy(x => x.LId).Last();
            var cus_pro = db.cus_pro.Where(x => x.pid == pid && x.cid == cid).First();
            var prod = db.Products.Where(x => x.PId == pid).First();
            if(cus.CName == "Bilal Associate")
            {
                var pp = (prod.PPr*1.00) / (208*1.00);
                var prr = (pp * 1.00) * (qty * 1.00);
                price = Convert.ToInt32(prr);
            }
            else
            {
                if(cus_pro.baseprice == 0)
                {
                    var pp = (prod.PSp * 1.00) / (208 * 1.00);
                    var prr = (pp * 1.00) * (qty * 1.00);
                    price = Convert.ToInt32(prr);
                }
                else
                {
                    var pp = (cus_pro.baseprice * 1.00) / (208 * 1.00);
                    var prr = (pp * 1.00) * (qty * 1.00);
                    price = Convert.ToInt32(prr);
                }
            }

            looseledger ll = new looseledger();
            ll.Description = Convert.ToString(pid);
            ll.Qty = 1;
            ll.lIn = qty;
            ll.lOut = 0;
            ll.lBalance = loose.lBalance + qty;
            ll.day = DateTime.Now.Day;
            ll.year = DateTime.Now.Year;
            ll.month = DateTime.Now.Month;
            ll.time = DateTime.Now.ToShortTimeString();
            ll.type = "Pail_add";
            ll.CName = cus.CName;
            db.looseledger.Add(ll);
            db.SaveChanges();
            var cus_last1 = db.Customerledger.Any(x => x.CName == Convert.ToString(cid));
            if(cus_last1 is true)
            {
                var cus_last = db.Customerledger.Where(x => x.CName == Convert.ToString(cid)).OrderBy(x => x.ClId).Last();
                Customerledger cl = new Customerledger();
                cl.CName = Convert.ToString(cid);
                cl.Description = Convert.ToString(pid);
                cl.Qty = qty;
                cl.day = DateTime.Now.Day;
                cl.year = DateTime.Now.Year;
                cl.month = DateTime.Now.Month;
                cl.time = DateTime.Now.ToShortTimeString();
                if (cus.CName == "Bilal Associate")
                {
                    cl.ClIn = qty * price;
                    cl.ClOut = 0;
                    cl.ClBalance = cus_last.ClBalance + cl.ClIn;
                }
                else
                {
                    cl.ClIn = 0;
                    cl.ClOut = qty * price;
                    cl.ClBalance = cus_last.ClBalance - cl.ClOut;
                }
                var lc = db.looseledger.OrderBy(x => x.LId).Last();
                cl.Orderid = lc.LId;
                cl.type = "Pail";
                cl.date = DateTime.Now.Date;
                db.Customerledger.Add(cl);
                db.SaveChanges();
            }
            else 
            {
                Customerledger cl = new Customerledger();
                cl.CName = Convert.ToString(cid);
                cl.Description = Convert.ToString(pid);
                cl.Qty = qty;
                cl.day = DateTime.Now.Day;
                cl.year = DateTime.Now.Year;
                cl.month = DateTime.Now.Month;
                cl.time = DateTime.Now.ToShortTimeString();
                if (cus.CName == "Bilal Associate")
                {
                    cl.ClIn = qty * price;
                    cl.ClOut = 0;
                    cl.ClBalance = cl.ClIn;
                }
                else
                {
                    cl.ClIn = 0;
                    cl.ClOut = qty * price;
                    cl.ClBalance = -cl.ClOut;
                }
                var lc = db.looseledger.OrderBy(x => x.LId).Last();
                cl.Orderid = lc.LId;
                cl.type = "Pail";
                cl.date = DateTime.Now.Date;
                db.Customerledger.Add(cl);
                db.SaveChanges();
            }
            

            return RedirectToAction("loose", "front");
        }
        public IActionResult loose_drum()
        {
            return View();
        }
        [HttpPost]
        public IActionResult loose_drum(int id)
        {
            var pro = db.Products.Where(x => x.PId == id).First();
            var chk = db.Productledgers.Any(x=>x.Pid == pro.PId);
            var cus = db.Customers.Any(x => x.CName == "Bilal Associate");
            if (cus is true)
            {
                if (chk is true)
                {
                    var chk1 = db.Productledgers.Where(x => x.Pid == pro.PId).OrderBy(x => x.PlId).Last();
                    var sp = db.StockPrice.Where(x => x.PId == chk1.Pid && x.Status == "active").First();
                    sp.Status = "del_loose";
                    var loose_pr = sp.Price / sp.Pack;
                    db.SaveChanges();

                    var sto = db.Stocks.Where(x => x.PId == pro.PId).First();
                    sto.SQty = sto.SQty - 1;
                    db.SaveChanges();
                    for (int h = 1; h <= pro.PPack; h++)
                    {
                        looseprice lp = new looseprice();
                        var b = db.LoosePrice.Any();
                        if(b is true)
                        {
                            var b1 = db.LoosePrice.OrderBy(x => x.Id).Last();
                            lp.Id = b1.Id + 1;
                        }
                        else
                        {
                            lp.Id = 1;
                        }
                        lp.PId = pro.PId;
                        lp.Qty = 1;
                        lp.Pack = Convert.ToInt32(pro.PPack);
                        lp.Price = loose_pr;
                        lp.Date = DateTime.Now.Day;
                        lp.Month = DateTime.Now.Month;
                        lp.Year = DateTime.Now.Year;
                        lp.Status = "active";
                        db.LoosePrice.Add(lp);
                        db.SaveChanges();
                    }
                    
                    if (chk1.PlBalance >= 1)
                    {
                        var lo_chk = db.looseledger.Any(x => x.Description == Convert.ToString(id));
                        if (lo_chk is true)
                        {
                            var lo_chk11 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).OrderBy(x => x.LId).Last();
                            looseledger ll = new looseledger();
                            ll.CName = "Bilal Assosiate";
                            ll.Description = Convert.ToString(pro.PId);
                            ll.Qty = 1;
                            ll.lIn = pro.PPack;
                            ll.lOut = 0;
                            ll.lBalance = lo_chk11.lBalance + pro.PPack;
                            ll.day = DateTime.Now.Day;
                            ll.month = DateTime.Now.Month;
                            ll.year = DateTime.Now.Year;
                            ll.time = DateTime.Now.ToShortTimeString();
                            ll.type = "Druml";
                            db.looseledger.Add(ll);
                            db.SaveChanges();
                        }
                        else
                        {
                            looseledger ll = new looseledger();
                            ll.CName = "Bilal Assosiate";
                            ll.Description = Convert.ToString(pro.PId);
                            ll.Qty = 1;
                            ll.lIn = pro.PPack;
                            ll.lOut = 0;
                            ll.lBalance = pro.PPack;
                            ll.day = DateTime.Now.Day;
                            ll.month = DateTime.Now.Month;
                            ll.year = DateTime.Now.Year;
                            ll.time = DateTime.Now.ToShortTimeString();
                            ll.type = "Druml";
                            db.looseledger.Add(ll);
                            db.SaveChanges();
                        }

                        var loos = db.looseledger.Where(x => x.type == "Druml" && x.Description == Convert.ToString(pro.PId)).OrderBy(x => x.LId).Last();

                        var x1 = db.Products.Where(x => x.PId == pro.PId).First();

                        var rx = Convert.ToDouble(x1.PSp * 1.00 / x1.PPack * 1.00);
                        var pprint = Convert.ToInt64(rx * 100);
                        var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);
                        var cus1 = db.Customers.Where(x => x.CName == "Bilal Associate").First();
                        
                        //Secondarysale ss = new Secondarysale();
                        //ss.PId = pro.PId;
                        //ss.CId = cus1.CId;
                        //ss.SsQty = 1;
                        //ss.date = DateTime.Now.Date;
                        //ss.SsOrderno = loos.LId;
                        //ss.Day = DateTime.Now.Day;
                        //ss.Month = DateTime.Now.Month;
                        //ss.Year = DateTime.Now.Year;
                        //ss.Time = DateTime.Now.ToShortTimeString();
                        //ss.Pack = Convert.ToString(pro.PPack);
                        //ss.Type = "Druml";
                        //ss.Price = Convert.ToInt32(pprfloat);
                        //ss.TPrice = pro.PSp;
                        //db.Secondarysales.Add(ss);
                        db.SaveChanges();

                        var lo_chk1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).OrderBy(x => x.LId).Last();
                        var cus_chk = db.Customerledger.Any(x => x.CName == Convert.ToString(cus1.CId));
                        if(cus_chk is true)
                        {
                            var cus_chk1 = db.Customerledger.Where(x => x.CName == Convert.ToString(cus1.CId)).OrderBy(x=>x.ClId).Last();
                            Customerledger cl = new Customerledger();

                            cl.CName = Convert.ToString(cus1.CId);
                            cl.Description = Convert.ToString(pro.PId);
                            cl.Qty = 208;
                            cl.ClIn = pro.PSp ;
                            cl.ClOut = 0;
                            cl.date = DateTime.Now.Date;
                            cl.ClBalance = cus_chk1.ClBalance + (pro.PSp);
                            cl.type = "Druml";
                            cl.day = DateTime.Now.Day;
                            cl.month = DateTime.Now.Month;
                            cl.year = DateTime.Now.Year;
                            cl.time = DateTime.Now.ToShortTimeString();
                            cl.Orderid = lo_chk1.LId;
                            db.Customerledger.Add(cl);
                            db.SaveChanges();
                        }
                        else
                        {
                            Customerledger cl = new Customerledger();
                            cl.CName = Convert.ToString(cus1.CId);
                            cl.Description = Convert.ToString(pro.PId);
                            cl.Qty = 208;
                            cl.ClIn = pro.PSp ;
                            cl.ClOut = 0;
                            cl.ClBalance =  pro.PSp ;
                            cl.type = "Druml";
                            cl.date = DateTime.Now.Date;
                            cl.day = DateTime.Now.Day;
                            cl.month = DateTime.Now.Month;
                            cl.year = DateTime.Now.Year;
                            cl.time = DateTime.Now.ToShortTimeString();
                            cl.Orderid = lo_chk1.LId;
                            db.Customerledger.Add(cl);
                            db.SaveChanges();
                        }

                        var pl_chk = db.Productledgers.Any(x => x.Pid == pro.PId);
                        if(pl_chk is true)
                        {
                            var pl_chk1 = db.Productledgers.Where(x => x.Pid == pro.PId).OrderBy(x=>x.PlId).Last();
                            Productledger pl = new Productledger();
                            pl.CName = cus1.CName;
                            pl.day = DateTime.Now.Day;
                            pl.month = Convert.ToString(DateTime.Now.Month);
                            pl.year = DateTime.Now.Year;
                            pl.time = DateTime.Now.ToShortTimeString();
                            pl.PlDate = DateTime.Now.Date;
                            pl.PlIn = 0;
                            pl.PlOut = 1;
                            pl.Type = "loose";
                            pl.PlBalance = pl_chk1.PlBalance - 1;
                            pl.Pid = pro.PId;
                            pl.pono = loos.LId;
                            db.Productledgers.Add(pl);
                            db.SaveChanges();
                        }
                
                }
                    else
                    {
                        TempData["cust"] = "Product Does Not Available In Stock";
                        return RedirectToAction("loose_drum");
                    }

                }
                else
                {
                    TempData["cust"] = "Product Not Available In Stock";
                    return RedirectToAction("loose_drum");
                }
            }
            else
            {
                TempData["cust"] = "First Open A Customer Account Bilal Associate";
                return RedirectToAction("loose_drum");
            }

            return RedirectToAction("loose","front");
        }
        public IActionResult loose_order(int id)
        {
            var cc = 0;
            if (id == 0)
            {
                cc = Convert.ToInt32(TempData["data"]);
            }
            var x = db.Products.Where(x => x.PId == id).First();
            var res1 = db.Customers.Where(x=>x.CStatus == "Active").OrderBy(x => x.CName).Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            ViewBag.Cid = res1;
            if(id == 0)
            {
                ViewBag.id = cc;
            }
            else
            {
                ViewBag.id = id;
            }
            ViewBag.name = x.PName;
            return View();
        }
        [HttpPost]
        public IActionResult loose_order(int id,int qty, int cus,int price)
        {
            if (qty <= 0)
            {
                TempData["data"] = id;
                TempData["cust"] = "Enter Correct Qty";
                return RedirectToAction("loose_order");
            }
            if(id == 0 || id == null)
            {
                id = Convert.ToInt32(TempData["id"]); 
            }
            var x = db.looseledger.Any(x => x.Description == Convert.ToString(id));
            if(x is true)
            {
                var x1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).OrderBy(x=>x.year).ThenBy(x=>x.month).ThenBy(x=>x.day).Last();
                if (x1.lBalance >= qty)
                {
                    var pro = db.Products.Where(x => x.PId == id).First();

                    var x11 = db.Products.Where(x => x.PId == pro.PId).First();

                    var rx = Convert.ToDouble(x11.PSp * 1.00 / x11.PPack * 1.00);
                    var pprint = Convert.ToInt64(rx * 100);
                    var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);

                    var cust = db.Customers.Where(x => x.CId == cus).First();
                    var tqt = 0;
                    var tpr = 0;
                    for(int h = 1; h <= qty; h++)
                    {
                        var lp = db.StockPrice.Where(x => x.PId == pro.PId || x.Status == "del_loose").OrderBy(x=>x.Id).Last();
                        tqt = tqt + 1;
                        tpr = tpr + (lp.Price/lp.Pack);
                        db.SaveChanges();
                    }

                    looseledger ll = new looseledger();
                    ll.CName = cust.CName;
                    ll.Description = Convert.ToString(pro.PId);
                    ll.Qty = 1;
                    ll.lIn = 0;
                    ll.lOut = qty;
                    ll.lBalance = x1.lBalance - qty;
                    ll.day = DateTime.Now.Day;
                    ll.month = DateTime.Now.Month;
                    ll.year = DateTime.Now.Year;
                    ll.time = DateTime.Now.ToShortTimeString();
                    ll.type = "Pail";
                    db.looseledger.Add(ll);
                    db.SaveChanges();

                    StockPrice sp = new StockPrice();
                    var cv = db.StockPrice.Any();
                    if (cv is true)
                    {
                        var cv1 = db.StockPrice.OrderBy(x => x.Id).Last();
                        sp.Id = cv1.Id + 1;
                    }
                    else
                    {
                        sp.Id = 1;
                    }
                    sp.PId = Convert.ToInt32(pro.PId);
                    sp.Pack = Convert.ToInt32(pro.PPack);
                    sp.Qty = tqt;
                    var xv2 = db.cus_pro.Any(x => x.pid == pro.PId && x.cid == cust.CId);
                    var rx1 = Convert.ToDouble(x11.PPr * 1.00 / x11.PPack * 1.00);
                    var pprint1 = Convert.ToInt64(rx1 * 100);
                    var pprfloat1 = Convert.ToDouble(pprint1 * 1.00 / 100.00);
                    if (xv2 is true)
                    {
                        var xv12 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == cust.CId).First();

                        if (xv12.baseprice > 0)
                        {
                            var rx12 = Convert.ToDouble(xv12.baseprice * 1.00 / pro.PPack * 1.00);
                            var pprint12 = Convert.ToInt64(rx12 * 100);
                            var pprfloat12 = Convert.ToDouble(pprint12 * 1.00 / 100.00);

                            var xn2 = pprfloat12 * qty;
                            sp.Price = Convert.ToInt32(xn2);
                        }
                        else
                        {
                            var xn2 = pprfloat1 * qty;
                            sp.Price = Convert.ToInt32(xn2);
                        }
                    }
                    else
                    {
                        var xn2 = pprfloat1 * qty;
                        sp.Price = Convert.ToInt32(xn2);
                    }
                    sp.Date = DateTime.Now.Day;
                    sp.Month = DateTime.Now.Month;
                    sp.Year = DateTime.Now.Year;
                    sp.Status = "del1";
                    var lx = db.looseledger.OrderBy(x => x.LId).Last();
                    sp.loose_id = lx.LId;
                    db.StockPrice.Add(sp);
                    db.SaveChanges();


                    

                    Secondarysale ss = new Secondarysale();

                    var perpr = ((pro.PPr * 1.00) - (Convert.ToInt32(pro.Trade_disc) * pro.PPack *1.00)) / (pro.PPack * 1.00);
                    var pri = (perpr * 1.00) * (qty * 1.00);

                    ss.PId = pro.PId;
                    ss.Pr_Price = Convert.ToInt32(pri); 
                    ss.CId = cust.CId;
                    ss.SsQty = 1;
                    ss.SsOrderno = lx.LId;
                    ss.Day = DateTime.Now.Day;
                    ss.Month = DateTime.Now.Month;
                    ss.Year = DateTime.Now.Year;
                    ss.Time = DateTime.Now.ToShortTimeString();
                    ss.Pack = Convert.ToString(qty);
                    ss.date = DateTime.Now.Date;
                    ss.Type = "Pail";
                    var xv = db.cus_pro.Any(x => x.pid == pro.PId && x.cid == cust.CId);
                    if (xv is true)
                    {
                        var xv1 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == cust.CId).First();

                        if(xv1.baseprice > 0)
                        {
                            var rx1c = Convert.ToDouble(xv1.baseprice * 1.00 / pro.PPack * 1.00);
                            var pprint1c = Convert.ToInt64(rx1c * 100);
                            var pprfloat1c = Convert.ToDouble(pprint1c * 1.00 / 100.00);

                            var xn = pprfloat1c * qty;
                            ss.Price = Convert.ToInt32(pprfloat1c);
                            ss.TPrice = Convert.ToInt32(xn);
                        }
                        else
                        {
                            var xn = pprfloat * qty;
                            ss.Price = Convert.ToInt32(pprfloat);
                            ss.TPrice = Convert.ToInt32(xn);
                        }
                    }
                    else
                    {
                        var xn = pprfloat * qty;
                        ss.Price = Convert.ToInt32(pprfloat);
                        ss.TPrice = Convert.ToInt32(xn) ;
                    }
                    db.Secondarysales.Add(ss);
                    db.SaveChanges();


                    var cus_chk = db.Customerledger.Any(x => x.CName == Convert.ToString(cust.CId));
                    if (cus_chk is true)
                    {
                        var cus_chk1 = db.Customerledger.Where(x => x.CName == Convert.ToString(cust.CId)).OrderBy(x => x.ClId).Last();

                        var xb = db.Products.Where(x => x.PId == pro.PId).First();


                        Customerledger cl = new Customerledger();
                        cl.CName = Convert.ToString(cust.CId);
                        cl.Description = Convert.ToString(pro.PId);
                        cl.Qty = qty;
                        cl.date = DateTime.Now.Date;
                        var xvs = db.cus_pro.Any(x => x.pid == pro.PId && x.cid == cust.CId);
                        if (xvs is true)
                        {
                            var xv1 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == cust.CId).First();

                            if (xv1.baseprice > 0)
                            {
                                var rx1c = Convert.ToDouble(xv1.baseprice * 1.00 / pro.PPack * 1.00);
                                var pprint1c = Convert.ToInt64(rx1c * 100);
                                var pprfloat1c = Convert.ToDouble(pprint1c * 1.00 / 100.00);

                                var xn = pprfloat1c * qty;

                                cl.ClIn = Convert.ToInt32(xn);
                                cl.ClBalance = cus_chk1.ClBalance + Convert.ToInt32(xn);
                            }
                            else
                            {
                                var xm = pprfloat * qty;
                                cl.ClIn = Convert.ToInt32(xm);
                                cl.ClBalance = cus_chk1.ClBalance + Convert.ToInt32(xm);
                            }
                        }
                        else
                        {
                            var xm = pprfloat * qty;
                            cl.ClIn = Convert.ToInt32(xm);
                            cl.ClBalance = cus_chk1.ClBalance + Convert.ToInt32(xm);
                        }
                        cl.ClOut = 0;
                        cl.type = "Pail";
                        cl.day = DateTime.Now.Day;
                        cl.month = DateTime.Now.Month;
                        cl.year = DateTime.Now.Year;
                        cl.time = DateTime.Now.ToShortTimeString();
                        cl.Orderid = lx.LId;
                        db.Customerledger.Add(cl);
                        db.SaveChanges();
                    }
                    else
                    {
                        Customerledger cl = new Customerledger();
                        cl.CName = Convert.ToString(cust.CId);
                        cl.Description = Convert.ToString(pro.PId);
                        cl.Qty = qty;
                        cl.date = DateTime.Now.Date;
                        var xvs = db.cus_pro.Any(x => x.pid == pro.PId && x.cid == cust.CId);
                        if (xvs is true)
                        {
                            var xv1 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == cust.CId).First();
                            if (xv1.baseprice > 0)
                            {
                                var rx11 = Convert.ToDouble(xv1.baseprice * 1.00 / pro.PPack * 1.00);
                                var pprint11 = Convert.ToInt64(rx11 * 100);
                                var pprfloat11 = Convert.ToDouble(pprint11 * 1.00 / 100.00);
                                var xm = pprfloat11 * qty;

                                cl.ClIn = Convert.ToInt32(xm);
                                cl.ClBalance = Convert.ToInt32(xm);
                            }
                            else
                            {
                                var xm = pprfloat * qty;
                                cl.ClIn = Convert.ToInt32(xm);
                                cl.ClBalance =Convert.ToInt32(xm);
                            }
                        }
                        else
                        {
                            var xm = pprfloat * qty;
                            cl.ClIn = Convert.ToInt32(xm);
                            cl.ClBalance = Convert.ToInt32(xm);
                        }
                        cl.ClOut = 0;
                        cl.type = "Pail";
                        cl.day = DateTime.Now.Day;
                        cl.month = DateTime.Now.Month;
                        cl.year = DateTime.Now.Year;
                        cl.time = DateTime.Now.ToShortTimeString();
                        cl.Orderid = lx.LId;
                        db.Customerledger.Add(cl);
                        db.SaveChanges();
                    }

                }
                else
                {
                    TempData["cust"] = "The Current Drum Dosent Have The Entered Amount Of Litres";
                    return RedirectToAction("loose_order");
                }
            }
            else
            {
                TempData["cust"] = "First Open A Drum";
                return RedirectToAction("loose_order");
            }
            ViewBag.id = id;
            TempData["id"] = id;
            return RedirectToAction("looseledger","front");
        }
        public IActionResult cheq_pay()
        {
            return View();
        }
        [HttpPost]
        public IActionResult cheq_pay(int type)
        {
            if (type == 0)
            {
                pay_type pt = new pay_type();
                pt.PayerType = "Pet";
                db.pay_type.Add(pt);
                db.SaveChanges();
            }
            else
            {
                pay_type pt = new pay_type();
                pt.PayerType = "Cus";
                db.pay_type.Add(pt);
                db.SaveChanges();
            }
            return RedirectToAction("cheque");
        }
        public IActionResult payment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult payment(int type)
        {
            if (type == 0)
            {
                pay_type pt = new pay_type();
                pt.PayerType = "Pet";
                db.pay_type.Add(pt);
                db.SaveChanges();
            }
            else
            {
                pay_type pt = new pay_type();
                pt.PayerType = "Cus";
                db.pay_type.Add(pt);
                db.SaveChanges();
            }
            return RedirectToAction("pay");
        }
        public IActionResult pay()
        {
            var res1 = db.Customers.Where(x=>x.CStatus == "Active").OrderBy(x => x.CName).Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            var res = db.Venders.Select(x => new SelectListItem { Text = x.VName, Value = Convert.ToString(x.VId) });
            ViewBag.VId = res;
            ViewBag.CId = res1;
            return View();
        }
        [HttpPost]
        public IActionResult pay(int cid,int vid,int amount,int bank,string des)
        {
            if (amount <= 0)
            {
                TempData["data"] = "Enter Correct Amount";
                return RedirectToAction("pay");
            }
            if (cid == 0 && vid != null)
            {
                var ven = db.Venders.Where(x => x.VId == vid).First();
                Da d = new Da();
                if (bank == 0)
                {
                    var das = db.Das.Any(x => x.DasType == "Pay");
                    if (das is true)
                    {

                    }
                    else
                    {
                        TempData["data"] = "DAS Is Empty";
                        return RedirectToAction("pay");
                    }
                }

                Pay p = new Pay();
                p.PayerName = ven.VId;
                if (bank == 0)
                {
                    p.Mode = "Cash";
                }
                else
                {
                    var bank1 = db.Bankstatements.Where(x => x.BsId == bank).First();
                    p.Mode = bank1.Bankname;
                }
                if (des == null)
                {
                    p.Description = "";
                }
                else
                {
                    p.Description = des;
                }
                p.PAmount = amount;
                p.Day = DateTime.Now.Day;
                p.Month = DateTime.Now.Month;
                p.Year = DateTime.Now.Year;
                p.Time = DateTime.Now.ToShortTimeString();
                p.Type = "Pet";
                db.Pay.Add(p);
                db.SaveChanges();

                var xv = db.Pay.OrderBy(x => x.PaId).Last();
                
                if (bank == 0)
                {
                    var das1 = db.Das.Where(x => x.DasType == "Pay").OrderBy(x => x.DasId).Last();
                    if (das1.DasBalance >= amount)
                    {
                        d.DasDescrition = ven.VName;
                        d.DasExpense = "Cash Payment" + " ( " + des + " ) ";
                        d.DasDeit = amount;
                        d.DasCredit = 0;
                        d.DasBalance = das1.DasBalance - amount; 
                        if (xv.Mode == "Cash")
                        {
                            d.typ1 = "cash";
                        }
                        else
                        {
                            d.typ1 = "online";
                        }
                        d.ven_cus = "ven";
                        d.day = Convert.ToString(DateTime.Now.Day);
                        d.month = Convert.ToString(DateTime.Now.Month);
                        d.year = Convert.ToString(DateTime.Now.Year);
                        d.DasDate = DateTime.Now.Date;
                        d.DasType = "Pay";
                        d.order_id = xv.PaId;
                        db.Das.Add(d);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["data"] = "Entered Amount Not In DAS";
                        return RedirectToAction("pay");
                    }
                }

                if (bank != 0)
                {
                    var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                    BankDetail bd = new BankDetail();
                    if (bb.Balance >= amount)
                    {
                        var bdl = db.BankDetail.Any(x => x.BdName == bb.BsId);
                        if (bdl is true)
                        {
                            var bdl1 = db.BankDetail.Where(x => x.BdName == bb.BsId).OrderBy(x => x.BdId).Last();
                            bb.Balance = bb.Balance - amount;
                            bd.BdName = bb.BsId;
                            bd.date = DateTime.Now.Date;
                            bd.BdSender = ven.VName;
                            bd.BdIn = 0;
                            bd.BdOut = amount;
                            bd.ven_cus = "ven";
                            bd.BdBalance = bdl1.BdBalance - amount;
                            bd.day = DateTime.Now.Day;
                            bd.month = DateTime.Now.Month;
                            bd.year = DateTime.Now.Year;
                            bd.pay_id = xv.PaId;
                            bd.time = DateTime.Now.ToShortTimeString();
                            db.BankDetail.Add(bd);
                            db.SaveChanges();
                        }
                        else
                        {
                            var bb1 = db.Bankstatements.Where(x => x.BsId == bank).First();
                            bd.BdName = bb1.BsId;
                            bd.BdSender = "Closing";
                            bd.BdIn = bb1.Balance;
                            bd.BdOut = 0;
                            bd.BdBalance = bb1.Balance;
                            bd.date = DateTime.Now.Date;
                            bd.day = DateTime.Now.Day;
                            bd.month = DateTime.Now.Month;
                            bd.year = DateTime.Now.Year;
                            bd.time = DateTime.Now.ToShortTimeString();
                            db.BankDetail.Add(bd);
                            db.SaveChanges();

                            BankDetail bd1 = new BankDetail();
                            bb.Balance = bb.Balance - amount;
                            bd1.BdName = bb.BsId;
                            bd1.BdSender = ven.VName;
                            bd1.BdIn = 0;
                            bd1.BdOut = amount;
                            bd1.ven_cus = "ven";
                            bd1.pay_id = xv.PaId;
                            bd1.date = DateTime.Now.Date;
                            bd1.BdBalance = bb.Balance - amount;
                            bd1.day = DateTime.Now.Day;
                            bd1.month = DateTime.Now.Month;
                            bd1.year = DateTime.Now.Year;
                            bd1.time = DateTime.Now.ToShortTimeString();
                            db.BankDetail.Add(bd1);
                            db.SaveChanges();
                            bb.Balance = bb.Balance - amount;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["data"] = "The Amount Is Not Available In Bank";
                        return RedirectToAction("pay");
                    }
                    db.SaveChanges();
                }

                
                Vendorledger vl = new Vendorledger();
                var chk = db.Vendorledger.Any(x => x.VName == ven.VName);
                if (chk is true)
                {
                    var chk1 = db.Vendorledger.Where(x => x.VName == ven.VName).OrderBy(x => x.VlId).Last();
                    var pay = db.Pay.Where(x => x.PayerName == vid).OrderBy(x => x.PaId).Last();
                    vl.VName = ven.VName;
                    if (bank == 0)
                    {
                        vl.Description = "Cash Payment" + " ( " + des + " ) ";
                    }
                    else
                    {
                        var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                        vl.Description = bb.Bankname + " ( " + des + " ) ";
                    }
                    vl.VlIn = 0;
                    vl.VlOut = amount;
                    vl.date = DateTime.Now.Date;
                    vl.VlBalance = chk1.VlBalance - amount;
                    vl.day = DateTime.Now.Day;
                    vl.month = DateTime.Now.Month;
                    vl.year = DateTime.Now.Year;
                    vl.time = DateTime.Now.ToShortTimeString();
                    vl.Qty = 0;
                    vl.Type = "pay";
                    vl.Orderid = pay.PaId;
                    db.Vendorledger.Add(vl);
                    db.SaveChanges();
                }
                else
                {
                    var pay = db.Pay.Where(x => x.PayerName == vid).OrderBy(x => x.PaId).Last();
                    vl.VName = ven.VName;
                    if (bank == 0)
                    {
                        vl.Description = "Cash Payment" + " ( " + des + " ) ";
                    }
                    else
                    {
                        var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                        vl.Description = bb.Bankname + " ( " + des + " ) ";
                    }
                    vl.VlIn = 0;
                    vl.VlOut = amount;
                    vl.VlBalance = -amount;
                    vl.date = DateTime.Now.Date;
                    vl.day = DateTime.Now.Day;
                    vl.month = DateTime.Now.Month;
                    vl.year = DateTime.Now.Year;
                    vl.time = DateTime.Now.ToShortTimeString();
                    vl.Qty = 0;
                    vl.Type = "pay";
                    vl.Orderid = pay.PaId;
                    db.Vendorledger.Add(vl);
                    db.SaveChanges();
                }


            }
            else if (cid != null && vid == 0)
            {
                var cus = db.Customers.Where(x => x.CId == cid).First();
                Pay p = new Pay();
                p.PayerName = cus.CId;
                if (bank == 0)
                {
                    p.Mode = "Cash";
                }
                else
                {
                    var bank1 = db.Bankstatements.Where(x => x.BsId == bank).First();
                    p.Mode = bank1.Bankname;
                }
                if (des == null)
                {
                    p.Description = "";
                }
                else
                {
                    p.Description = des;
                }
                p.PAmount = amount;
                p.Day = DateTime.Now.Day;
                p.Month = DateTime.Now.Month;
                p.Year = DateTime.Now.Year;
                p.Time = DateTime.Now.ToShortTimeString();
                p.Type = "Cus";
                db.Pay.Add(p);
                db.SaveChanges();

                if (bank != 0)
                {
                    var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                    bb.Balance = bb.Balance + amount;
                    var xv = db.Pay.OrderBy(x => x.PaId).Last();
                    var bdl = db.BankDetail.Any(x => x.BdName == bb.BsId);
                    BankDetail bd = new BankDetail();
                    if (bdl is true)
                    {
                        var bdl1 = db.BankDetail.Where(x => x.BdName == bb.BsId).OrderBy(x => x.BdId).Last();
                        bd.BdName = bb.BsId;
                        bd.BdSender = cus.CName;
                        bd.BdIn = amount;
                        bd.BdOut = 0;
                        bd.ven_cus = "cus";
                        bd.pay_id = xv.PaId;
                        bd.BdBalance = bdl1.BdBalance + amount;
                        bd.day = DateTime.Now.Day;
                        bd.date = DateTime.Now.Date;
                        bd.month = DateTime.Now.Month;
                        bd.year = DateTime.Now.Year;
                        bd.time = DateTime.Now.ToShortTimeString();
                        db.BankDetail.Add(bd);
                        db.SaveChanges();
                    }
                    else
                    {
                        var bb1 = db.Bankstatements.Where(x => x.BsId == bank).First();
                        bd.BdName = bb1.BsId;
                        bd.BdSender = "Closing";
                        bd.BdIn = bb1.Balance;
                        bd.date = DateTime.Now.Date;
                        bd.BdOut = 0;
                        bd.BdBalance = bb.Balance;
                        bd.day = DateTime.Now.Day;
                        bd.month = DateTime.Now.Month;
                        bd.year = DateTime.Now.Year;
                        bd.time = DateTime.Now.ToShortTimeString();
                        db.BankDetail.Add(bd);
                        db.SaveChanges();

                        var bdl1 = db.BankDetail.Where(x => x.BdName == bb.BsId).OrderBy(x => x.BdId).Last();
                        BankDetail bd1 = new BankDetail();
                        bd1.BdName = bb.BsId;
                        bd1.BdSender = cus.CName;
                        bd1.BdIn = amount;
                        bd1.BdOut =  0;
                        bd1.pay_id = xv.PaId;
                        bd1.ven_cus = "cus";
                        bd1.date = DateTime.Now.Date;
                        bd1.BdBalance = bb.Balance + amount;
                        bd1.day = DateTime.Now.Day;
                        bd1.month = DateTime.Now.Month;
                        bd1.year = DateTime.Now.Year;
                        bd1.time = DateTime.Now.ToShortTimeString();
                        db.BankDetail.Add(bd1);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                }

                
                Customerledger cl = new Customerledger();
                var chk = db.Customerledger.Any(x => x.CName == Convert.ToString(cus.CId));
                if (chk is true)
                {
                    var chk1 = db.Customerledger.Where(x => x.CName == Convert.ToString(cus.CId)).OrderBy(x => x.ClId).Last();
                    var pay = db.Pay.Where(x => x.PayerName == cid).OrderBy(x => x.PaId).Last();
                    cl.CName = Convert.ToString(cus.CId);
                    if (bank == 0)
                    {
                        cl.Description = "Cash Payment"+ " ( " + des + " ) ";
                    }
                    else
                    {
                        var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                        cl.Description = bb.Bankname+ " ( " + des + " ) ";
                    }
                    cl.Qty = 0;
                    cl.day = DateTime.Now.Day;
                    cl.date = DateTime.Now.Date;
                    cl.month = DateTime.Now.Month;
                    cl.year = DateTime.Now.Year;
                    cl.time = DateTime.Now.ToShortTimeString();
                    cl.ClIn = 0;
                    cl.ClOut = amount;
                    cl.ClBalance = chk1.ClBalance - amount;
                    cl.Orderid = pay.PaId;
                    cl.type = "Payment";
                    db.Customerledger.Add(cl);
                    db.SaveChanges();
                }
                else
                {
                    var pay = db.Pay.Where(x => x.PayerName == cid).OrderBy(x => x.PaId).Last();
                    cl.CName = Convert.ToString(cus.CId);
                    if (bank == 0)
                    {
                        cl.Description = "Cash Payment"+ " ( " + des + " ) ";
                    }
                    else
                    {
                        var bb = db.Bankstatements.Where(x => x.BsId == bank).First();
                        cl.Description = bb.Bankname+ " ( " + des + " ) ";
                    }
                    cl.Qty = 0;
                    cl.day = DateTime.Now.Day;
                    cl.month = DateTime.Now.Month;
                    cl.year = DateTime.Now.Year;
                    cl.date = DateTime.Now.Date;
                    cl.time = DateTime.Now.ToShortTimeString();
                    cl.ClIn = 0;
                    cl.ClOut = amount;
                    cl.ClBalance = -amount;
                    cl.Orderid = pay.PaId;
                    cl.type = "Payment";
                    db.Customerledger.Add(cl);
                    db.SaveChanges();
                }

                Da d = new Da();
                var das = db.Das.Any(x => x.DasType == "Pay");
                var xv1 = db.Pay.OrderBy(x => x.PaId).Last();
                if (das is true)
                {
                    var das1 = db.Das.Where(x => x.DasType == "Pay").OrderBy(x => x.DasId).Last();
                    if (bank == 0)
                    {
                        d.DasDescrition = cus.CName;
                        d.DasExpense = "Cash Payment"+ " ( " + des + " ) ";
                        d.DasDeit = 0;
                        d.DasCredit = amount;
                        d.DasBalance = das1.DasBalance + amount;
                        d.DasType = "Pay";
                        if(xv1.Mode == "Cash")
                        {
                            d.typ1 = "cash";
                        }
                        else
                        {
                            d.typ1 = "online";
                        }
                        d.ven_cus = "cus";
                        d.order_id = xv1.PaId;
                        d.day = Convert.ToString(DateTime.Now.Day);
                        d.month = Convert.ToString(DateTime.Now.Month);
                        d.year = Convert.ToString(DateTime.Now.Year);
                        d.DasDate = DateTime.Now.Date;
                        db.Das.Add(d);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (bank == 0)
                    {
                        d.DasDescrition = cus.CName;
                        d.DasExpense = "Cash Payment"+ " ( " + des + " ) ";
                        d.DasDeit = 0;
                        d.DasCredit = amount;
                        d.DasBalance = amount;
                        d.ven_cus = "cus";
                        if (xv1.Mode == "Cash")
                        {
                            d.typ1 = "cash";
                        }
                        else
                        {
                            d.typ1 = "online";
                        }
                        d.DasType = "Pay";
                        d.order_id = xv1.PaId;
                        d.day = Convert.ToString(DateTime.Now.Day);
                        d.month = Convert.ToString(DateTime.Now.Month);
                        d.year = Convert.ToString(DateTime.Now.Year);
                        d.DasDate = DateTime.Now.Date;
                        db.Das.Add(d);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("payment","front");
        }
        public IActionResult bank_detail(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public IActionResult bank_detail(int id , BankDetail bd)
        {
            if(bd.BdIn != null && bd.BdOut != null)
            {
                TempData["data"] = "Enter Only One In Or Out";
                return RedirectToAction("bank_detail");
            }
            else
            {
                var x = db.BankDetail.Any(x => x.BdName == id);
                var x2 = db.Bankstatements.Where(x => x.BsId == id).First();
                if (x is true)
                {
                    var x1 = db.BankDetail.Where(x => x.BdName == id).OrderBy(x => x.BdId).Last();
                    if (bd.BdIn == null && bd.BdOut != null)
                    {
                        if (x1.BdBalance >= bd.BdOut)
                        {
                            BankDetail bd1 = new BankDetail();
                            bd1.BdName = x1.BdName;
                            bd1.BdSender = bd.BdSender;
                            bd1.BdIn = 0;
                            bd1.BdOut = bd.BdOut;
                            bd1.BdBalance = x1.BdBalance - bd.BdOut;
                            x2.Balance = x2.Balance - bd.BdOut;
                            bd1.day = DateTime.Now.Day;
                            bd1.month = DateTime.Now.Month;
                            bd1.date = DateTime.Now.Date;
                            bd1.year = DateTime.Now.Year;
                            bd1.time = DateTime.Now.ToShortTimeString();
                            bd1.typ = 0;
                            db.BankDetail.Add(bd1);
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["data"] = "Entered Amount Is Not Available In Bank";
                            return RedirectToAction("bank_detail");
                        }
                    }
                    else if (bd.BdIn != null && bd.BdOut == null)
                    {
                        BankDetail bd1 = new BankDetail();
                        bd1.BdName = x1.BdName;
                        bd1.BdSender = bd.BdSender;
                        bd1.BdIn = bd.BdIn;
                        bd1.BdOut = 0;
                        bd1.BdBalance = x1.BdBalance + bd.BdIn;
                        x2.Balance = x2.Balance + bd.BdIn;
                        bd1.date = DateTime.Now.Date;
                        bd1.day = DateTime.Now.Day;
                        bd1.month = DateTime.Now.Month;
                        bd1.year = DateTime.Now.Year;
                        bd1.time = DateTime.Now.ToShortTimeString(); 
                        bd1.typ = 0;
                        db.BankDetail.Add(bd1);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("bank_statement","front");
        }
        public IActionResult cheque()
        {
            var res1 = db.Customers.Select(x => new SelectListItem { Text = x.CName, Value = Convert.ToString(x.CId) });
            ViewBag.CId = res1;
            var res = db.Bankstatements.Select(x => new SelectListItem { Text = x.Bankname, Value = Convert.ToString(x.BsId) });
            ViewBag.BId = res;
            return View();
        }
        [HttpPost]
        public IActionResult cheque(cheque c,DateTime date, int iid, int vid)
        {
            if (c.Amount <= 0)
            {
                TempData["data"] = "Enter Correct Amount";
                return RedirectToAction("cheque");
            }
            if (c.Description is null)
            {
                c.Description = "";
            }
            c.Day = DateTime.Now.Day;
            c.Month = DateTime.Now.Month;
            c.Year = DateTime.Now.Year;
            c.Time = DateTime.Now.ToShortTimeString();
            c.Pdc_Day = date.Day;
            c.Pdc_Month = date.Month;
            c.Pdc_Year = date.Year;
            c.Type = "";
            c.Status = 0;
            if (iid != null && vid == 0)
            {
                c.CName = iid;
                c.Type = "cus";
            }
            else if (iid == 0 && vid != null)
            {
                c.CName = vid;
                c.Type = "pet";
            }
            db.cheque.Add(c);
            db.SaveChanges();
            return RedirectToAction("cheque", "front");
        }
        public IActionResult cheque_det(int id , string type)
        {
            var cheq = db.cheque.Where(x => x.Ch_Id == id).First();
            var cdt = cheq.Pdc_Day;
            var cmm = cheq.Pdc_Month;
            var cyy = cheq.Pdc_Year;
            var dt = DateTime.Now.Day;
            var mm = DateTime.Now.Month;
            var yy = DateTime.Now.Year;
            if ((dt>=cdt && mm==cmm && yy == cyy) || (mm>cmm && yy==cyy) || (yy > cyy))
            {
                if(type == "pet")
                {
                    var cust = db.Venders.Where(x => x.VId == cheq.CName).First();
                    
                    var bank = db.Bankstatements.Where(x => x.Bankname == cheq.Cheque_of).First();
                    var bank_det = db.BankDetail.Where(x => x.BdName == bank.BsId).OrderBy(x => x.BdId).Last();
                    if (bank.Balance < cheq.Amount)
                    {
                        TempData["data"] = "Bank Dosen't have this amount of money";
                        return RedirectToAction("cheque","front");
                    }
                    else
                    {
                        bank.Balance = bank_det.BdBalance - cheq.Amount;
                        BankDetail bd1 = new BankDetail();
                        bd1.BdName = bank.BsId;
                        bd1.BdSender = Convert.ToString(cust.VName);
                        bd1.BdIn = 0;
                        bd1.typ = 2;
                        bd1.pay_id = cheq.Ch_Id;
                        bd1.ven_cus = "ven";
                        bd1.BdOut = cheq.Amount;
                        bd1.BdBalance = bank_det.BdBalance - cheq.Amount;
                        bd1.day = DateTime.Now.Day;
                        bd1.month = DateTime.Now.Month;
                        bd1.year = DateTime.Now.Year;
                        bd1.date = DateTime.Now.Date;
                        bd1.time = DateTime.Now.ToShortTimeString();
                        db.BankDetail.Add(bd1);
                    }
                    db.SaveChanges();

                    Vendorledger vl = new Vendorledger();
                    var chk = db.Vendorledger.Any(x => x.VName == Convert.ToString(cheq.CName));
                    if (chk is true)
                    {
                        var chk1 = db.Vendorledger.Where(x => x.VName == Convert.ToString(cheq.CName)).OrderBy(x => x.VlId).Last();
                        vl.VName = Convert.ToString(cust.VName);
                        vl.Description = "Cheque From " + bank.Bankname + " ( " + cheq.Description + " ) ";
                        vl.Qty = 0;
                        vl.day = DateTime.Now.Day;
                        vl.date = DateTime.Now.Date;
                        vl.month = DateTime.Now.Month;
                        vl.year = DateTime.Now.Year;
                        vl.time = DateTime.Now.ToShortTimeString();
                        vl.VlIn = 0;
                        vl.Type = "Cheque";
                        vl.VlOut = cheq.Amount;
                        vl.VlBalance = chk1.VlBalance - cheq.Amount;
                        vl.Orderid = cheq.Ch_Id;
                        db.Vendorledger.Add(vl);
                        db.SaveChanges();
                    }
                    else
                    {
                        vl.VName = Convert.ToString(cust.VName);
                        vl.Description = "Cheque From "+ bank.Bankname + " ( " + cheq.Description + " ) ";
                        vl.Qty = 0;
                        vl.day = DateTime.Now.Day;
                        vl.date = DateTime.Now.Date;
                        vl.month = DateTime.Now.Month;
                        vl.year = DateTime.Now.Year;
                        vl.time = DateTime.Now.ToShortTimeString();
                        vl.VlIn = 0;
                        vl.Type = "Cheque";
                        vl.VlOut = cheq.Amount;
                        vl.VlBalance = -cheq.Amount;
                        vl.Orderid = cheq.Ch_Id;
                        db.Vendorledger.Add(vl);
                        db.SaveChanges();
                    }
                    cheq.Status = 1;
                    db.SaveChanges();
                }
                else if(type == "cus")
                {
                    var cust = db.Customers.Where(x => x.CId == cheq.CName).First();
                    if (cheq.Transfer_to == "0")
                    {
                        Da d = new Da();
                        var das12 = db.Das.Any(x => x.DasType == "Pay");
                        if (das12 is true)
                        {
                            var das1 = db.Das.Where(x => x.DasType == "Pay").OrderBy(x => x.DasId).Last();
                            d.DasDescrition = cust.CName;
                            d.DasExpense = "Cash Payment From Cheque" + " ( " + cheq.Description + " ) ";
                            d.DasDeit = 0;
                            d.DasCredit = cheq.Amount;
                            d.DasBalance = das1.DasBalance + cheq.Amount;
                            d.DasType = "Pay";
                            d.typ1 = "cheque";
                            d.order_id = cheq.Ch_Id;
                            d.ven_cus = "cus";
                            d.day = Convert.ToString(DateTime.Now.Day);
                            d.month = Convert.ToString(DateTime.Now.Month);
                            d.year = Convert.ToString(DateTime.Now.Year);
                            d.DasDate = DateTime.Now.Date;
                            db.Das.Add(d);
                            db.SaveChanges();
                        }
                        else
                        {
                            d.DasDescrition = cust.CName;
                            d.DasExpense = "Cash Payment From Cheque" + " ( " + cheq.Description + " ) ";
                            d.DasDeit = 0;
                            d.DasCredit = cheq.Amount;
                            d.DasBalance = cheq.Amount;
                            d.DasType = "Pay";
                            d.typ1 = "cheque";
                            d.order_id = cheq.Ch_Id;
                            d.ven_cus = "cus";
                            d.day = Convert.ToString(DateTime.Now.Day);
                            d.month = Convert.ToString(DateTime.Now.Month);
                            d.year = Convert.ToString(DateTime.Now.Year);
                            d.DasDate = DateTime.Now.Date;
                            db.Das.Add(d);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        var bank = db.Bankstatements.Where(x => x.BsId == Convert.ToInt32(cheq.Transfer_to)).First();
                        var bank_det = db.BankDetail.Where(x => x.BdName == Convert.ToInt32(cheq.Transfer_to)).OrderBy(x => x.BdId).Last();
                        bank.Balance = bank_det.BdBalance + cheq.Amount;
                        BankDetail bd1 = new BankDetail();
                        bd1.BdName = bank.BsId;
                        bd1.BdSender = Convert.ToString(cust.CName);
                        bd1.BdIn = cheq.Amount;
                        bd1.BdOut = 0;
                        bd1.typ = 2;
                        bd1.pay_id = cheq.Ch_Id;
                        bd1.ven_cus = "cus";
                        bd1.BdBalance = bank_det.BdBalance + cheq.Amount;
                        bd1.day = DateTime.Now.Day;
                        bd1.month = DateTime.Now.Month;
                        bd1.date = DateTime.Now.Date;
                        bd1.year = DateTime.Now.Year;
                        bd1.time = DateTime.Now.ToShortTimeString();
                        db.BankDetail.Add(bd1);
                        db.SaveChanges();
                    }

                    Customerledger cl = new Customerledger();
                    var chk = db.Customerledger.Any(x => x.CName == Convert.ToString(cheq.CName));
                    if (chk is true)
                    {
                        var chk1 = db.Customerledger.Where(x => x.CName == Convert.ToString(cheq.CName)).OrderBy(x => x.ClId).Last();
                        cl.CName = Convert.ToString(cust.CId);
                        if (cheq.Transfer_to == "0")
                        {
                            cl.Description = "Cash From Cheque " + " ( " + cheq.Description + " ) ";
                        }
                        else
                        {
                            var bank = db.Bankstatements.Where(x => x.BsId == Convert.ToInt32(cheq.Transfer_to)).First();
                            var bank_det = db.BankDetail.Where(x => x.BdName == Convert.ToInt32(cheq.Transfer_to)).OrderBy(x => x.BdId).Last();
                            cl.Description = bank.Bankname + " ( " + cheq.Description + " ) ";
                        }
                        cl.Qty = 0;
                        cl.day = DateTime.Now.Day;
                        cl.date = DateTime.Now.Date;
                        cl.month = DateTime.Now.Month;
                        cl.year = DateTime.Now.Year;
                        cl.time = DateTime.Now.ToShortTimeString();
                        cl.ClIn = 0;
                        cl.ClOut = cheq.Amount;
                        cl.ClBalance = chk1.ClBalance - cheq.Amount;
                        cl.Orderid = cheq.Ch_Id;
                        cl.type = "Cheque";
                        db.Customerledger.Add(cl);
                        db.SaveChanges();
                    }
                    else
                    {
                        cl.CName = Convert.ToString(cust.CId);
                        if (cheq.Transfer_to == "0")
                        {
                            cl.Description = "Cash From Cheque " + " ( " + cheq.Description + " ) ";
                        }
                        else
                        {
                            var bank = db.Bankstatements.Where(x => x.BsId == Convert.ToInt32(cheq.Transfer_to)).First();
                            var bank_det = db.BankDetail.Where(x => x.BdName == Convert.ToInt32(cheq.Transfer_to)).OrderBy(x => x.BdId).Last();
                            cl.Description = bank.Bankname + " ( " + cheq.Description + " ) ";
                        }
                        cl.Qty = 0;
                        cl.day = DateTime.Now.Day;
                        cl.month = DateTime.Now.Month;
                        cl.year = DateTime.Now.Year;
                        cl.date = DateTime.Now.Date;
                        cl.time = DateTime.Now.ToShortTimeString();
                        cl.ClIn = 0;
                        cl.ClOut = cheq.Amount;
                        cl.ClBalance = -cheq.Amount;
                        cl.Orderid = cheq.Ch_Id;
                        cl.type = "Cheque";
                        db.Customerledger.Add(cl);
                        db.SaveChanges();
                    }
                    cheq.Status = 1;
                    db.SaveChanges();
                }
            }
            else
            {
                TempData["data"] = "First Wait For PDC Date";
            }
            return RedirectToAction("cheque","front");
        }
        public IActionResult del_challan1()
        {
            del_no d = new del_no();
            d.CId = null;
            db.del_no.Add(d);
            db.SaveChanges();
            return RedirectToAction("del_challan");
        }
        public IActionResult del_challan()
        {
            return View();
        }
        [HttpPost]
        public IActionResult del_challan(int id1,int id)
        {
            if(id1 != 0)
            {
                var del_no = db.del_no.OrderBy(x => x.DId).Last();
                var ss = db.Secondarysales.Where(x => x.SsId == id1).First();
                var del = db.delivery.Any(x => x.del_no == del_no.DId);
                if (del is true)
                {
                    var del1 = db.delivery.Where(x => x.del_no == del_no.DId).First();
                    var del_no1 = db.del_no.Where(x => x.DId == del1.del_no).First();
                    if (del_no1.CId == ss.CId)
                    {
                        var chk = db.delivery.Any(x => x.del_no == del_no1.DId && x.p_id == ss.PId);
                        if (chk is true)
                        {
                            var chk1 = db.delivery.Where(x => x.del_no == del_no1.DId && x.p_id == ss.PId).First();
                            chk1.qty = chk1.qty + ss.SsQty;
                            ss.Status = del_no1.DId;
                            db.SaveChanges();
                        }
                        else
                        {
                            delivery d = new delivery();
                            d.del_no = del_no1.DId;
                            d.p_id = ss.PId;
                            d.qty = ss.SsQty;
                            db.delivery.Add(d);
                            ss.Status = del_no1.DId;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["data"] = "Enter Same Customers For Delivery Challan";
                        return RedirectToAction("del_challan");
                    }
                }
                else
                {
                    delivery d = new delivery();
                    d.del_no = del_no.DId;
                    d.p_id = ss.PId;
                    d.qty = ss.SsQty;
                    db.delivery.Add(d);
                    ss.Status = del_no.DId;
                    del_no.CId = ss.CId;
                    db.SaveChanges();
                }
                if (id == 0 || id == 2)
                {
                    return RedirectToAction("del_challan", "front");
                }
                else
                {
                    return RedirectToAction("del_challan");
                }
            }
            else
            {
                if (id == 2)
                {
                    return RedirectToAction("del_challan", "front");
                }
                TempData["data"] = "No Pending Delivery Left .Press End Button";
                return RedirectToAction("del_challan");
            }
        }
        public IActionResult cus_pro(int id,int iid,int cid,int bs)
        {
            if (iid == 0)
            {
                var pro = db.Products.ToList();
                foreach (var item in pro)
                {
                    var x = db.cus_pro.Any(x => x.cid == id && x.pid == item.PId);
                    if (x is false)
                    {
                        cus_pro cp = new cus_pro();
                        cp.cid = id;
                        cp.pid = item.PId;
                        cp.baseprice = 0;
                        db.cus_pro.Add(cp);
                        db.SaveChanges();
                    }

                }
                var cus = db.Customers.Where(x => x.CId == id).First();
                var x1 = db.cus_pro.Where(x => x.cid == id).ToList();
                ViewBag.name = cus.CName;
                return View(x1);
            }
            else
            {
                var cus = db.cus_pro.Where(x => x.Id == iid).First();
                cus.baseprice = bs;
                db.SaveChanges();
                var cus1 = db.Customers.Where(x => x.CId == cid).First();
                ViewBag.name = cus1.CName;
                var x = db.cus_pro.Where(x => x.cid == cid).ToList();
                return View(x);
            }
        }
        public IActionResult das()
        {
            return View();
        }
        [HttpPost]
        public IActionResult das(Da d)
        {
            if(d.DasCredit == null && d.DasDeit == null)
            {
                TempData["data"] = "Enter 'IN' Or 'OUT'";
                return RedirectToAction("das");
            }
            else if(d.DasCredit != null && d.DasDeit != null)
            {
                TempData["data"] = "Enter Only 1 'IN' Or 'OUT'";
                return RedirectToAction("das");
            }
            else if(d.DasCredit != null && d.DasDeit == null)
            {
                if (d.DasCredit <= 0)
                {
                    TempData["data"] = "Enter Correct Qty";
                    return RedirectToAction("das");
                }
                var x = db.Das.Any(x => x.DasType == "Pay");
                if(x is true)
                {
                    var x1 = db.Das.Where(x => x.DasType == "Pay").OrderBy(x=>x.DasId).Last();
                    d.DasDate = DateTime.Now.Date;
                    d.DasBalance = x1.DasBalance + d.DasCredit;
                    d.DasType = "Pay";
                    d.DasDeit = 0;
                    d.day = Convert.ToString(DateTime.Now.Day) ;
                    d.month = Convert.ToString(DateTime.Now.Month);
                    d.year = Convert.ToString(DateTime.Now.Year);
                    d.typ1 = "dd";
                    db.Das.Add(d);
                    db.SaveChanges();
                }
                else
                {
                    d.DasDate = DateTime.Now.Date;
                    d.DasBalance =  d.DasCredit;
                    d.DasType = "Pay";
                    d.DasDeit = 0;
                    d.day = Convert.ToString(DateTime.Now.Day);
                    d.month = Convert.ToString(DateTime.Now.Month);
                    d.year = Convert.ToString(DateTime.Now.Year);
                    d.typ1 = "dd";
                    db.Das.Add(d);
                    db.SaveChanges();
                }
            }
            else if (d.DasCredit == null && d.DasDeit != null)
            {
                if (d.DasDeit <= 0)
                {
                    TempData["data"] = "Enter Correct Qty";
                    return RedirectToAction("das");
                }
                var x = db.Das.Any(x => x.DasType == "Pay");
                if (x is true)
                {
                    var x1 = db.Das.Where(x => x.DasType == "Pay").OrderBy(x => x.DasId).Last();
                    if(x1.DasBalance >= d.DasDeit)
                    {
                        d.DasDate = DateTime.Now.Date;
                        d.DasBalance = x1.DasBalance - d.DasDeit;
                        d.DasType = "Pay";
                        d.DasCredit = 0;
                        d.day = Convert.ToString(DateTime.Now.Day);
                        d.month = Convert.ToString(DateTime.Now.Month);
                        d.year = Convert.ToString(DateTime.Now.Year);
                        d.typ1 = "dd";
                        db.Das.Add(d);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["data"] = "The Entered Cash Is Not Available In DAS To Debit";
                        return RedirectToAction("das");
                    }
                }
                else
                {
                    TempData["data"] = "No Cash In DAS To Debit";
                    return RedirectToAction("das");
                }
            }
            return RedirectToAction("das_pay","front");
        }
        public IActionResult banktrans()
        {
            return View();
        }
        [HttpPost]
        public IActionResult banktrans(int bank1,int bank2,int amount)
        {
            if(amount == 0 || amount == null)
            {
                TempData["data"] = "Enter Amount";
                return RedirectToAction("banktrans");
            }
            else if(bank1 == bank2)
            {
                TempData["data"] = "Select Different Bank";
                return RedirectToAction("banktrans");
            }
            else
            {
                var b1 = db.Bankstatements.Where(x => x.BsId == bank1).First();
                if (b1.Balance >= amount)
                {
                    var b2 = db.Bankstatements.Where(x => x.BsId == bank2).First();
                    var d1 = db.BankDetail.Any(x => x.BdName == bank1);
                    var d2 = db.BankDetail.Any(x => x.BdName == bank2);
                    var dd1 = db.BankDetail.Where(x => x.BdName == bank1).OrderBy(x => x.BdId).Last();
                    var dd2 = db.BankDetail.Where(x => x.BdName == bank2).OrderBy(x => x.BdId).Last();

                    BankDetail bd1 = new BankDetail();
                    bd1.BdName = b1.BsId;
                    bd1.BdSender = "Amount Send In " + b2.Bankname;
                    bd1.BdIn = 0;
                    bd1.BdOut = amount;
                    bd1.BdBalance = dd1.BdBalance - amount;
                    bd1.day = DateTime.Now.Day;
                    bd1.month = DateTime.Now.Month;
                    bd1.year = DateTime.Now.Year;
                    bd1.time = DateTime.Now.ToShortTimeString();
                    db.BankDetail.Add(bd1);
                    db.SaveChanges();

                    BankDetail bd2 = new BankDetail();
                    bd2.BdName = b2.BsId;
                    bd2.BdSender = "Amount Send From " + b1.Bankname;
                    bd2.BdIn = amount;
                    bd2.BdOut = 0;
                    bd2.BdBalance = dd2.BdBalance + amount;
                    bd2.day = DateTime.Now.Day;
                    bd2.month = DateTime.Now.Month;
                    bd2.year = DateTime.Now.Year;
                    bd2.time = DateTime.Now.ToShortTimeString();
                    db.BankDetail.Add(bd2);
                    b1.Balance = b1.Balance - amount;
                    b2.Balance = b2.Balance + amount;
                    db.SaveChanges();

                }
                else
                {
                    TempData["data"] = "The Amount Is Not Present In " + b1.Bankname +" Bank";
                    return RedirectToAction("banktrans");
                }
            }
            return RedirectToAction("bank_statement","front");
        }
        public IActionResult cl_close(int id)
        {
            var x = db.Customers.Where(x => x.CId == id).First();
            ViewBag.id = id;
            ViewBag.name = x.CName;
            return View();
        }
        [HttpPost]
        public IActionResult cl_close(Customerledger cl)
        {
            if(cl.ClIn>0 && cl.ClOut > 0)
            {
                TempData["data"] = "The Only One 'IN' Or 'OUT'";
                return RedirectToAction("cl_close");
            }
            else if ((cl.ClIn == 0 || cl.ClIn == null) && (cl.ClOut == 0 || cl.ClOut == null))
            {
                TempData["data"] = "Enter At Least One 'IN' Or 'OUT'";
                return RedirectToAction("cl_close");
            }
            else if (cl.ClIn > 0)
            {
                cl.Description = "Closing";
                cl.Qty = 0;
                cl.ClOut = 0;
                cl.date = DateTime.Now.Date;
                cl.ClBalance = cl.ClIn;
                cl.type = "closing";
                cl.day = DateTime.Now.Day;
                cl.month = DateTime.Now.Month;
                cl.year = DateTime.Now.Year;
                cl.time = DateTime.Now.ToShortTimeString();
                cl.Orderid = 0;
                db.Customerledger.Add(cl);
                db.SaveChanges();
            }
            else if (cl.ClOut > 0)
            {
                cl.Description = "Closing";
                cl.Qty = 0;
                cl.ClIn = 0;
                cl.date = DateTime.Now.Date;
                cl.ClBalance = -cl.ClOut;
                cl.type = "closing";
                cl.day = DateTime.Now.Day;
                cl.month = DateTime.Now.Month;
                cl.year = DateTime.Now.Year;
                cl.time = DateTime.Now.ToShortTimeString();
                cl.Orderid = 0;
                db.Customerledger.Add(cl);
                db.SaveChanges();
            }
            TempData["id"] = cl.CName;
            return RedirectToAction("cl1", "front");
        }
        public IActionResult vl_close(int id)
        {
            var x = db.Venders.Where(x => x.VId == id).First();
            ViewBag.id = id;
            ViewBag.name = x.VName;
            return View();
        }
        [HttpPost]
        public IActionResult vl_close(Vendorledger vl)
        {
            if (vl.VlIn > 0 && vl.VlOut > 0)
            {
                TempData["data"] = "The Only One 'IN' Or 'OUT'";
                return RedirectToAction("vl_close");
            }
            else if ((vl.VlIn == 0 || vl.VlIn == null) && (vl.VlOut == 0 || vl.VlOut == null))
            {
                TempData["data"] = "Enter At Least One 'IN' Or 'OUT'";
                return RedirectToAction("vl_close");
            }
            else if (vl.VlIn > 0)
            {
                vl.Description = "Closing";
                vl.Qty = 0;
                vl.VlOut = 0;
                vl.date = DateTime.Now.Date;
                vl.VlBalance = vl.VlIn;
                vl.day = DateTime.Now.Day;
                vl.month = DateTime.Now.Month;
                vl.year = DateTime.Now.Year;
                vl.time = DateTime.Now.ToShortTimeString();
                vl.Orderid = 0;
                db.Vendorledger.Add(vl);
                db.SaveChanges();
            }
            else if (vl.VlOut > 0)
            {
                vl.Description = "Closing";
                vl.Qty = 0;
                vl.VlIn = 0;
                vl.date = DateTime.Now.Date;
                vl.VlBalance = -vl.VlOut;
                vl.day = DateTime.Now.Day;
                vl.month = DateTime.Now.Month;
                vl.year = DateTime.Now.Year;
                vl.time = DateTime.Now.ToShortTimeString();
                vl.Orderid = 0;
                db.Vendorledger.Add(vl);
                db.SaveChanges();
            }
            var pp = db.Venders.Where(x => x.VName == vl.VName).First();
            TempData["id"] = pp.VId;
            return RedirectToAction("vl1", "front");
        }
        public IActionResult ps1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ps1(int amount)
        {
            Primarysale ps = new Primarysale();
            ps.VName = "Closing";
            ps.PName = "--";
            ps.DcNo = "--";
            ps.PsPrice = 0;
            ps.PsQty = 0;
            ps.PsPack = 0;
            ps.PsDate = DateTime.Now;
            ps.date = DateTime.Now.Date;
            ps.day = DateTime.Now.Day;
            ps.year = DateTime.Now.Year;
            ps.month = Convert.ToString(DateTime.Now.Month);
            ps.time = DateTime.Now.ToShortTimeString();
            ps.Price = "0";
            ps.Total = Convert.ToString(amount);
            ps.OrderID = 0;
            db.Primarysales.Add(ps);
            db.SaveChanges();
            return RedirectToAction("ps","front");
        }
    }
}




    




