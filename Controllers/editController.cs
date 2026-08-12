using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace be.Controllers
{
    public class editController : Controller
    {
        BentContext db = new BentContext();
        public IActionResult user(int id)
        {
            var res = db.Users.Where(x => x.UId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult user(User u)
        {
            var res = db.Users.Where(x => x.UId == u.UId).First();
            res.UName = u.UName;
            res.UEmail = u.UEmail;
            res.UPhone = u.UPhone;
            res.URole = u.URole;
            res.Pass = u.Pass;
            db.SaveChanges();
            return RedirectToAction("user", "front");
        }
        public IActionResult product(int id)
        {
            var res = db.Products.Where(x => x.PId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult product(Product u)
        {
            var res = db.Products.Where(x => x.PId == u.PId).First();
            res.PName = u.PName;
            res.PPack = u.PPack;
            res.PPr = u.PPr;
            res.PSp = u.PSp;
            res.PCode = u.PCode;
            res.Trade_disc = u.Trade_disc;
            db.SaveChanges();
            return RedirectToAction("product", "front");
        }
        public IActionResult customer(int id)
        {
            var res = db.Customers.Where(x => x.CId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult customer(Customer c)
        {
            var res = db.Customers.Where(x => x.CId == c.CId).First();
            res.CName = c.CName;
            res.CEmail = c.CEmail;
            res.CPhone = c.CPhone;
            res.CAddress = c.CAddress;
            res.CStatus = c.CStatus;
            res.auth_per = c.auth_per;
            db.SaveChanges();
            return RedirectToAction("customer", "front");
        }
        public IActionResult vendor(int id)
        {
            var res = db.Venders.Where(x => x.VId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult vendor(Vender v)
        {
            var res = db.Venders.Where(x => x.VId == v.VId).First();
            res.VName = v.VName;
            res.VEmail = v.VEmail;
            res.VPhone = v.VPhone;
            res.VNtn = v.VNtn;
            db.SaveChanges();
            return RedirectToAction("vendor", "front");
        }
        public IActionResult bank_statement(int id)
        {
            var res = db.Bankstatements.Where(x => x.BsId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult bank_statement(Bankstatement b)
        {
            var res = db.Bankstatements.Where(x => x.BsId == b.BsId).First();
            res.Bankname = b.Bankname;
            res.Accountno = b.Accountno;
            res.Accounttitle = b.Accounttitle;
            res.Balance = b.Balance;
            db.SaveChanges();
            return RedirectToAction("bank_statement", "front");
        }
        public IActionResult followup(int id)
        {
            var res = db.Followups.Where(x => x.FuId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult followup(Followup f)
        {
            var res = db.Followups.Where(x => x.FuId == f.FuId).First();
            res.CName = f.CName;
            res.FuDate = f.FuDate;
            res.FuDescription = f.FuDescription;
            res.FuEntered = f.FuEntered;
            db.SaveChanges();
            return RedirectToAction("followup", "front");
        }
        public IActionResult po(int id)
        {
            var res = db.Pos.Where(x => x.PoId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult po(Po p)
        {
            if (p.PoQty <= 0)
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("po");
            }
            var res = db.Pos.Where(x => x.PoId == p.PoId).First();
            var pro = db.Products.Where(x => x.PId == res.PId).First();
            res.PoQty = p.PoQty;
            var disc = Convert.ToInt32(pro.Trade_disc) * pro.PPack;

            res.PoPrice = p.PoPrice;
            var r = Convert.ToDouble(p.PoPrice * 1.00 / pro.PPack * 1.00);
            var pprint = Convert.ToInt64(r * 100);
            var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);

            res.baseprltr = Convert.ToString(pprfloat);

            db.SaveChanges();
            TempData["id"] = res.PoUnique;
            return RedirectToAction("po", "det");
        }
        public IActionResult order(int id)
        {
            var res = db.Order.Where(x => x.OId == id).First();
            return View(res);
        }
        [HttpPost]
        public IActionResult order(Order o)
        {
            if (o.Qty <= 0)
            {
                TempData["mess"] = "Enter Correct Qty";
                return RedirectToAction("order");
            }
            var res = db.Order.Where(x => x.OId == o.OId).First();
            var pro = db.Products.Where(x => x.PId == res.PId).First();
            res.Qty = o.Qty;
            res.OPrice = o.OPrice;
            db.SaveChanges();
            TempData["id"] = res.OrUnique;
            return RedirectToAction("order", "det");
        }
        public IActionResult cheque(int id)
        {
            var res = db.cheque.Where(x => x.Ch_Id == id).First();
            var res1 = db.Bankstatements.Select(x => new SelectListItem { Text = x.Bankname, Value = Convert.ToString(x.Bankname) });
            ViewBag.BId = res1;
            var day = Convert.ToInt32(res.Day);
            var month = Convert.ToInt32(res.Month);
            var year = Convert.ToInt32(res.Year);
            DateTime date = new DateTime(year, month, day);
            ViewBag.date = date.ToString("yyyy-MM-dd");


            var day1 = Convert.ToInt32(res.Pdc_Day);
            var month1 = Convert.ToInt32(res.Pdc_Month);
            var year1 = Convert.ToInt32(res.Pdc_Year);
            DateTime date1 = new DateTime(year1, month1, day1);
            ViewBag.date1 = date1.ToString("yyyy-MM-dd");
            return View(res);
        }
        [HttpPost]
        public IActionResult cheque(cheque c, DateTime date, DateTime edate)
        {
            var res = db.cheque.Where(x => x.Ch_Id == c.Ch_Id).First();
            res.CName = c.CName;
            res.Amount = c.Amount;
            res.Cheque_of = c.Cheque_of;
            res.Transfer_to = c.Transfer_to;
            var count = date.Day + date.Month + date.Year;
            if (count > 3)
            {
                res.Pdc_Day = date.Day;
                res.Pdc_Month = date.Month;
                res.Year = date.Year;
            }
            if (c.Description != null)
            {
                res.Description = c.Description;
            }
            res.Day = edate.Day;
            res.Month = edate.Month;
            res.Year = edate.Year;
            res.Status = 0;
            db.SaveChanges();
            return RedirectToAction("cheque", "front");
        }
        public IActionResult cus_pro_price(int id)
        {
            var x = db.cus_pro.Where(x => x.Id == id).First();
            return View(x);
        }
        public IActionResult das(int id)
        {
            var x = db.Das.Where(x => x.DasId == id).First();
            return View(x);
        }
        [HttpPost]
        public IActionResult das(Da d)
        {
            var x = db.Das.Where(x => x.DasId == d.DasId).First();
            if (x.DasCredit == 0 || x.DasCredit == null)
            {
                if (d.DasCredit == 0 || d.DasCredit == null)
                {
                    x.DasExpense = d.DasExpense;
                    x.DasDescrition = d.DasDescrition;
                    x.DasCredit = 0;
                    x.DasDeit = d.DasDeit;
                    db.SaveChanges();
                }
                else
                {
                    TempData["data"] = "Edit Debit Only";
                    return RedirectToAction("das");
                }
            }
            else if (x.DasDeit == 0 || x.DasDeit == null)
            {
                if (d.DasDeit == 0 || d.DasDeit == null)
                {
                    x.DasExpense = d.DasExpense;
                    x.DasDescrition = d.DasDescrition;
                    x.DasCredit = d.DasCredit;
                    x.DasDeit = 0;
                    db.SaveChanges();
                }
                else
                {
                    TempData["data"] = "Edit Credit Only";
                    return RedirectToAction("das");
                }
            }

            var x1 = db.Das.Where(x => x.DasType == "Pay").ToList();
            var count = 0;
            var balance = 0;
            foreach (var item in x1)
            {
                if (item.DasDeit == 0 || item.DasDeit == null)
                {
                    if (count == 0)
                    {
                        item.DasBalance = item.DasCredit;
                        balance = Convert.ToInt32(item.DasCredit);
                    }
                    else
                    {
                        item.DasBalance = balance + item.DasCredit;
                        balance = balance + Convert.ToInt32(item.DasCredit);
                    }
                }
                else if (item.DasCredit == 0 || item.DasCredit == null)
                {
                    if (count == 0)
                    {
                        item.DasBalance = -item.DasDeit;
                        balance = Convert.ToInt32(-item.DasDeit);
                    }
                    else
                    {
                        item.DasBalance = balance - item.DasDeit;
                        balance = balance - Convert.ToInt32(item.DasDeit);
                    }
                }
                count = count + 1;
                db.SaveChanges();
            }
            return RedirectToAction("das_pay", "front");
        }
        public IActionResult bank_detail(int id)
        {
            var x = db.BankDetail.Where(x => x.BdId == id).First();
            return View(x);
        }
        [HttpPost]
        public IActionResult bank_detail(BankDetail bd)
        {
            if ((bd.BdIn > 0) && (bd.BdOut > 0) || (bd.BdIn == 0) && (bd.BdOut == 0))
            {
                TempData["data"] = "Enter Only One In Or Out";
                return RedirectToAction("bank_detail", "edit");
            }
            else
            {
                var x1 = db.BankDetail.Where(x => x.BdId == bd.BdId).First();
                var bb = db.Bankstatements.Where(x => x.BsId == x1.BdName).First();
                x1.BdSender = bd.BdSender;
                if (bd.BdIn == null || bd.BdIn == 0)
                {
                    x1.BdIn = 0;
                    x1.BdOut = bd.BdOut;
                }
                else if (bd.BdOut == null || bd.BdOut == 0)
                {
                    x1.BdIn = bd.BdIn;
                    x1.BdOut = 0;
                }
                db.SaveChanges();

                var x2 = db.BankDetail.Where(x => x.BdName == bb.BsId).ToList();
                var balance = 0;
                foreach (var item in x2)
                {
                    item.BdBalance = balance + item.BdIn - item.BdOut;
                    balance = Convert.ToInt32(item.BdBalance);
                    db.SaveChanges();
                }

                bb.Balance = balance;
                db.SaveChanges();
                return RedirectToAction("bank_statement", "front");
            }
            return View();
        }
        public IActionResult undo_po(int id)
        {
            var count = 0;
            var stock_price = db.StockPrice.Where(x => x.Pono == id).ToList();
            foreach (var item in stock_price)
            {
                if (item.Status != "active")
                {
                    TempData["mess"] = "Product From This Purchase Order Has Been Used";
                    return RedirectToAction("po", "front");
                }
            }

            var po1 = db.Pos.Any(x => x.PoUnique == id);
            if (po1 is true)
            {
                var po = db.Pos.Where(x => x.PoUnique == id).ToList();
                foreach (var item in po)
                {
                    var stock = db.Stocks.Where(x => x.PId == item.PId).First();
                    if (stock.SQty < item.PoQty)
                    {
                        TempData["mess"] = "Stock Dosen't have that qty";
                        return RedirectToAction("po", "front");
                    }
                }
                foreach (var item in po)
                {
                    var stock = db.Stocks.Where(x => x.PId == item.PId).First();
                    if (stock.SQty < item.PoQty)
                    {
                        TempData["mess"] = "Stock Dosen't have that qty";
                        return RedirectToAction("po", "front");
                    }
                    else
                    {
                        var p = db.Pos.Where(x => x.PoUnique == id && x.PId == item.PId).First();
                        stock.SQty = stock.SQty - p.PoQty;
                        db.SaveChanges();
                    }
                }
            }

            foreach (var item in stock_price)
            {
                db.StockPrice.Remove(item);
                db.SaveChanges();
            }

            var pono = db.PoNos.Where(x => x.PonoId == id).First();
            pono.PonoStatus = "no";
            db.SaveChanges();

            var grn = db.Grns.Where(x => x.Pono == id).First();
            db.Grns.Remove(grn);
            db.SaveChanges();

            var product = db.Productledgers.Where(x => x.Type == "primary" && x.pono == id).ToList();
            foreach (var item in product)
            {
                db.Productledgers.Remove(item);
                db.SaveChanges();

                var count1 = 0;
                var product_list = db.Productledgers.Where(x => x.Pid == item.Pid).ToList();
                foreach (var items in product_list)
                {
                    items.PlBalance = count1 + items.PlIn - items.PlOut;
                    count1 = Convert.ToInt32(items.PlBalance);
                    db.SaveChanges();
                }

            }

            var primary = db.Primarysales.Where(x => x.OrderID == id).ToList();
            foreach (var item in primary)
            {
                db.Primarysales.Remove(item);
                db.SaveChanges();
            }

            var vendor = db.Vendorledger.Where(x => x.Type == "primary" && x.Orderid == id).ToList();
            foreach (var item in vendor)
            {
                db.Vendorledger.Remove(item);
                db.SaveChanges();
            }

            var count_vv = 0;
            var vv = db.Vendorledger.ToList();
            foreach (var item in vv)
            {
                item.VlBalance = count_vv + item.VlIn - item.VlOut;
                count_vv = Convert.ToInt32(item.VlBalance);
                db.SaveChanges();
            }

            var das = db.Das.Where(x => x.typ1 == "primary" && x.order_id == id).First();
            db.Das.Remove(das);
            db.SaveChanges();

            var count_das = 0;
            var das1 = db.Das.Where(x => x.DasType != "Pay").ToList();
            foreach (var item in das1)
            {
                item.DasBalance = count_das + item.DasCredit - item.DasDeit;
                count_das = Convert.ToInt32(item.DasBalance);
                db.SaveChanges();
            }

            return RedirectToAction("po", "front");
        }
        public IActionResult undo_order(int id, int unique_id)
        {
            var order = db.Order.Where(x => x.OId == id).First();
            var del = order.delivered;
            order.delivered = 0;
            db.SaveChanges();

            var ss = db.Secondarysales.Where(x => x.SsOrderno == unique_id && x.PId == order.PId).ToList();
            foreach (var item in ss)
            {
                var stock = db.Stocks.Where(x => x.PId == item.PId).First();
                stock.SQty = stock.SQty + item.SsQty;
                db.SaveChanges();

                db.Secondarysales.Remove(item);
                db.SaveChanges();
            }

            var product_ledger = db.Productledgers.Where(x => x.pono == order.OrUnique && x.Type == "secondary" && x.Pid == order.PId).ToList();
            foreach (var item in product_ledger)
            {
                db.Productledgers.Remove(item);
                db.SaveChanges();
            }

            var product_ledger_balance = db.Productledgers.Any(x => x.Pid == order.PId);
            if (product_ledger_balance is true)
            {
                var product_ledger_balance1 = db.Productledgers.Where(x => x.Pid == order.PId).ToList();
                var count = 0;
                foreach (var item in product_ledger_balance1)
                {
                    item.PlBalance = count + item.PlIn - item.PlOut;
                    count = Convert.ToInt32(item.PlBalance);
                    db.SaveChanges();
                }
            }

            var stock_price = db.StockPrice.Where(x => x.Order_id == order.OId).ToList();
            foreach (var item in stock_price)
            {
                item.Status = "active";
                item.Order_id = 0;
                db.SaveChanges();
            }

            var customer = db.Customerledger.Where(x => x.Orderid == unique_id && x.CName == Convert.ToString(order.CId) && x.Description == Convert.ToString(order.PId)).ToList();
            foreach (var item in customer)
            {
                db.Customerledger.Remove(item);
                db.SaveChanges();
            }

            var customer1 = db.Customerledger.Where(x => x.CName == Convert.ToString(order.CId)).ToList();
            var count1 = 0;
            foreach (var item in customer1)
            {
                item.ClBalance = count1 + item.ClIn - item.ClOut;
                count1 = Convert.ToInt32(item.ClBalance);
                db.SaveChanges();
            }

            var cus = db.Customers.Where(x => x.CId == order.CId).First();
            var das = db.Das.Where(x => x.DasDescrition == cus.CName && x.typ1 == "secondary").First();
            var pro = db.Products.Where(x => x.PId == order.PId).First();
            var ltr = pro.PPack * del;
            das.DasDeit = das.DasDeit - ltr;
            db.SaveChanges();

            var das1 = db.Das.Where(x => x.DasType != "Pay").ToList();
            var count2 = 0;
            foreach (var item in das1)
            {
                item.DasBalance = count2 + item.DasCredit - item.DasDeit;
                count2 = Convert.ToInt32(item.DasBalance);
                db.SaveChanges();
            }

            TempData["id"] = unique_id;
            return RedirectToAction("order", "det");
        }
        public IActionResult undo_loose_drum(int id)
        {
            var x = db.looseledger.Any(x => x.Description == Convert.ToString(id));
            if (x is true)
            {
                var x1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).OrderBy(x => x.LId).Last();
                var pro = db.Products.Where(x => x.PId == id).First();
                if (x1.lBalance >= pro.PPack)
                {
                    for (int i = 1; i <= pro.PPack; i++)
                    {
                        var loose_pr = db.LoosePrice.Where(x => x.PId == id).OrderBy(x => x.Date).Last();
                        db.LoosePrice.Remove(loose_pr);
                        db.SaveChanges();
                    }

                    var loose_ledger = db.looseledger.Where(x => x.Description == Convert.ToString(id) && x.type == "Druml").OrderBy(x => x.LId).Last();
                    db.looseledger.Remove(loose_ledger);
                    db.SaveChanges();

                    var loose_ledger1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
                    var count = 0;
                    foreach (var item in loose_ledger1)
                    {
                        item.lBalance = count + item.lIn - item.lOut;
                        count = Convert.ToInt32(item.lBalance);
                        db.SaveChanges();
                    }

                    var stock = db.Stocks.Where(x => x.PId == id).First();
                    stock.SQty = stock.SQty + 1;
                    db.SaveChanges();

                    var product_ledger = db.Productledgers.Where(x => x.Type == "loose" && x.Pid == id).OrderBy(x => x.PlId).Last();
                    db.Productledgers.Remove(product_ledger);
                    db.SaveChanges();

                    var product_ledger1 = db.Productledgers.Where(x => x.Pid == id).ToList();
                    var count1 = 0;
                    foreach (var item in product_ledger1)
                    {
                        item.PlBalance = count1 + item.PlIn - item.PlOut;
                        count1 = Convert.ToInt32(item.PlBalance);
                        db.SaveChanges();
                    }

                    var customer = db.Customers.Where(x => x.CName == "Bilal Associate").First();
                    var customer_ledger = db.Customerledger.Where(x => x.CName == Convert.ToString(customer.CId) && x.type == "Druml" && x.Description == Convert.ToString(id)).OrderBy(x => x.ClId).Last();
                    db.Customerledger.Remove(customer_ledger);
                    db.SaveChanges();

                    var customer_ledger1 = db.Customerledger.Where(x => x.CName == Convert.ToString(customer.CId)).ToList();
                    var count3 = 0;
                    foreach (var item in customer_ledger1)
                    {
                        item.ClBalance = count3 + item.ClIn - item.ClOut;
                        count3 = Convert.ToInt32(item.ClBalance);
                        db.SaveChanges();
                    }

                    var stock_price = db.StockPrice.Where(x => x.PId == id && x.Status == "del_loose").OrderBy(x => x.Id).Last();
                    stock_price.Status = "active";
                    db.SaveChanges();

                }
                else
                {
                    TempData["mess"] = "The Current Ltrs Dosen't Match The Drum Packaging";
                    return RedirectToAction("loose", "front");
                }
            }
            return RedirectToAction("loose", "front");
        }
        public IActionResult loose_det(string id, int loose_id)
        {
            if (loose_id == 0 || loose_id == null)
            {
                loose_id = Convert.ToInt32(TempData["loose_id"]);
            }
            ViewBag.id = Convert.ToInt32(id);
            ViewBag.loose_id = loose_id;
            var loose_ledger = db.looseledger.Where(x => x.LId == loose_id).First();
            return View(loose_ledger);
        }
        [HttpPost]
        public IActionResult loose_det(int id, int loose_id, int qty)
        {
            var loose_ledger = db.looseledger.Where(x => x.LId == loose_id).First();
            if (qty == loose_ledger.lOut)
            {
                TempData["id"] = Convert.ToInt32(id);
                return RedirectToAction("looseledger", "front");
            }
            else
            {
                loose_ledger.lOut = qty;

                var loose_set1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
                var count1 = 0;
                foreach (var item in loose_set1)
                {
                    item.lBalance = count1 + item.lIn - item.lOut;
                    count1 = Convert.ToInt32(item.lBalance);
                    db.SaveChanges();
                }
                if (count1 < 0)
                {
                    TempData["loose_id"] = loose_id;
                    TempData["mess"] = "The Current Ltrs Dosen't Match The Drum Packaging";
                    return RedirectToAction("loose_det", "edit");
                }
                db.SaveChanges();

                var loose_set = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
                var count = 0;
                foreach (var item in loose_set)
                {
                    item.lBalance = count + item.lIn - item.lOut;
                    count = Convert.ToInt32(item.lBalance);
                    db.SaveChanges();
                }

                var sp = db.StockPrice.Where(x => x.loose_id == loose_id).First();
                var pr_ltr = sp.Price / sp.Qty;
                sp.Qty = qty;
                sp.Price = pr_ltr * qty;
                db.SaveChanges();


                var cus = db.Customerledger.Where(x => x.type == "Pail" && x.Orderid == loose_id).First();
                var pro = db.Products.Where(x => x.PId == Convert.ToInt32(cus.Description)).First();
                var xvs = db.cus_pro.Any(x => x.pid == pro.PId && x.cid == Convert.ToInt32(cus.CName));
                var rx = Convert.ToDouble(pro.PSp * 1.00 / pro.PPack * 1.00);
                var pprint = Convert.ToInt64(rx * 100);
                var pprfloat = Convert.ToDouble(pprint * 1.00 / 100.00);


                var ss = db.Secondarysales.Where(x => x.Type == "Pail" && x.SsOrderno == loose_id).First();
                if (xvs is true)
                {
                    var xv1 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == Convert.ToInt32(cus.CName)).First();
                    if (xv1.baseprice > 0)
                    {
                        var rx1c = Convert.ToDouble(xv1.baseprice * 1.00 / pro.PPack * 1.00);
                        var pprint1c = Convert.ToInt64(rx1c * 100);
                        var pprfloat1c = Convert.ToDouble(pprint1c * 1.00 / 100.00);
                        var xn = pprfloat1c * qty;

                        ss.Pack = Convert.ToString(qty);
                        ss.Price = Convert.ToInt32(pprfloat1c);
                        ss.TPrice = Convert.ToInt32(xn);
                    }
                    else
                    {
                        var xm = pprfloat * qty;
                        ss.Pack = Convert.ToString(qty);
                        ss.Price = Convert.ToInt32(pprfloat);
                        ss.TPrice = Convert.ToInt32(xm);
                    }
                }
                else
                {
                    var xm = pprfloat * qty;
                    ss.Pack = Convert.ToString(qty);
                    ss.Price = Convert.ToInt32(pprfloat);
                    ss.TPrice = Convert.ToInt32(xm);
                }
                db.SaveChanges();
                if (xvs is true)
                {
                    var xv1 = db.cus_pro.Where(x => x.pid == pro.PId && x.cid == Convert.ToInt32(cus.CName)).First();

                    if (xv1.baseprice > 0)
                    {
                        var rx1c = Convert.ToDouble(xv1.baseprice * 1.00 / pro.PPack * 1.00);
                        var pprint1c = Convert.ToInt64(rx1c * 100);
                        var pprfloat1c = Convert.ToDouble(pprint1c * 1.00 / 100.00);
                        var xn = pprfloat1c * qty;

                        cus.ClIn = Convert.ToInt32(xn);
                        cus.Qty = qty;
                    }
                    else
                    {
                        var xm = pprfloat * qty;
                        cus.ClIn = Convert.ToInt32(xm);
                        cus.Qty = qty;
                    }
                }
                else
                {
                    var xm = pprfloat * qty;
                    cus.ClIn = Convert.ToInt32(xm);
                    cus.Qty = qty;
                }
                db.SaveChanges();

                var cus_set = db.Customerledger.Where(x => x.CName == cus.CName).ToList();
                var countx = 0;
                foreach (var item in cus_set)
                {
                    item.ClBalance = countx + item.ClIn - item.ClOut;
                    countx = Convert.ToInt32(item.ClBalance);
                    db.SaveChanges();
                }
            }

            TempData["id"] = Convert.ToInt32(id);
            return RedirectToAction("looseledger", "front");
        }
        public IActionResult drumopendate(int loose_id)
        {
            var res = db.looseledger.Where(x => x.LId == loose_id).First();
            ViewBag.loose_id = loose_id;
            var day = Convert.ToInt32(res.day);
            var month = Convert.ToInt32(res.month);
            var year = Convert.ToInt32(res.year);
            DateTime date = new DateTime(year, month, day);
            ViewBag.date = date.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public IActionResult drumopendate(int id1, DateTime date)
        {
            var res = db.looseledger.Where(x => x.LId == id1).First();
            res.day = date.Day;
            res.month = date.Month;
            res.year = date.Year;
            db.SaveChanges();

            if(res.type == "Druml")
            {
                var pl = db.Productledgers.Where(x => x.Type == "loose" && x.pono == id1).First();
                pl.day = date.Day;
                pl.month = Convert.ToString(date.Month);
                pl.year = date.Year;
                pl.PlDate = date;
                db.SaveChanges();
            }

            var cl = db.Customerledger.Where(x => (x.type == "Druml" || x.type == "Pail") && x.Orderid == id1).First();
            cl.date = date;
            cl.day = date.Day;
            cl.month = date.Month;
            cl.year = date.Year;
            db.SaveChanges();
            TempData["id"] = res.Description;
            return RedirectToAction("looseledger","front");
        }
        public IActionResult loose_det_add(int id, int loose_id)
        {
            var ll = db.looseledger.Where(x => x.LId == loose_id).First();
            ViewBag.loose_id = ll.LId;
            ViewBag.id = ll.Description;
            return View(ll);
        }
        [HttpPost]
        public IActionResult loose_det_add(int qty, int id, int loose_id)
        {
            var ll = db.looseledger.Where(x => x.LId == loose_id).First();
            if (ll.lIn == qty)
            {
                TempData["id"] = Convert.ToInt32(id);
                return RedirectToAction("looseledger", "front");
            }
            else
            {
                ll.lIn = qty;
                db.SaveChanges();

                var loose_set = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
                var count1 = 0;
                foreach (var item in loose_set)
                {
                    item.lBalance = count1 + item.lIn - item.lOut;
                    count1 = Convert.ToInt32(item.lBalance);
                    db.SaveChanges();
                }

                var cus = db.Customerledger.Where(x => x.type == "Pail" && x.Orderid == loose_id).First();
                if (ll.CName == "Bilal Associate")
                {
                    var pr = cus.ClIn / cus.Qty;
                    var tpr = pr * qty;
                    cus.ClIn = tpr;
                    cus.Qty = qty;
                }
                else
                {
                    var pr = cus.ClOut / cus.Qty;
                    var tpr = pr * qty;
                    cus.ClOut = tpr;
                    cus.Qty = qty;
                }
                db.SaveChanges();

                var cus_set = db.Customerledger.Where(x => x.CName == cus.CName).ToList();
                var count = 0;
                foreach (var item in cus_set)
                {
                    item.ClBalance = count + item.ClIn - item.ClOut;
                    count = Convert.ToInt32(item.ClBalance);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("looseledger", "front");
        }
        public IActionResult online_pay(int id)
        {
            if (id == 0 || id == null)
            {
                ViewBag.id = TempData["id"];
            }
            var pay = db.Pay.Where(x => x.PaId == id).First();
            ViewBag.id = id;
            var day = Convert.ToInt32(pay.Day);
            var month = Convert.ToInt32(pay.Month);
            var year = Convert.ToInt32(pay.Year);
            DateTime date = new DateTime(year, month, day);
            ViewBag.date = date.ToString("yyyy-MM-dd");
            return View(pay);
        }
        [HttpPost]
        public IActionResult online_pay(int id, int amount,DateTime date)
        {
            var pay = db.Pay.Where(x => x.PaId == id).First();

            if (pay.Mode == "Cash")
            {

                var d = db.Das.Where(x => x.DasType == "Pay").ToList();
                var count = 0;
                foreach (var item in d)
                {
                    if (item.typ1 == "cash" && item.order_id == id)
                    {

                    }
                    else
                    {
                        count = Convert.ToInt32(count + item.DasCredit - item.DasDeit);
                    }
                }

                if (pay.Type == "Pet")
                {
                    if (amount > count)
                    {
                        TempData["mess"] = "Not Much Amoount Available In Das";
                        TempData["id"] = id;
                        return RedirectToAction("online_pay", "edit");
                    }
                }
                else if (pay.Type == "Cus")
                {
                    if (count + amount < 0)
                    {
                        TempData["mess"] = "Not Much Amoount Available In Das To Deduct";
                        TempData["id"] = id;
                        return RedirectToAction("online_pay", "edit");
                    }
                }

                var das = db.Das.Where(x => x.typ1 == "cash" && x.order_id == id).First();
                if (das.ven_cus == "ven")
                {
                    das.DasDeit = amount;
                }
                else
                {
                    das.DasCredit = amount;
                }
                das.day = Convert.ToString(date.Day);
                das.month = Convert.ToString(date.Month);
                das.year = Convert.ToString(date.Year);
                das.DasDate = date;
                db.SaveChanges();

                var d1 = db.Das.Where(x => x.DasType == "Pay").ToList();
                var count1 = 0;
                foreach (var item in d1)
                {
                    item.DasBalance = Convert.ToInt32(count1 + item.DasCredit - item.DasDeit);
                    count1 = Convert.ToInt32(item.DasBalance);
                    db.SaveChanges();
                }

            }
            else
            {
                var bank = db.BankDetail.Where(x => x.BdName == pay.PayerName).ToList();
                var count = 0;
                foreach (var item in bank)
                {
                    if (item.pay_id == id && item.typ == null)
                    {

                    }
                    else
                    {
                        count = Convert.ToInt32(count + item.BdIn - item.BdOut);
                    }
                }

                if (pay.Type == "Pet")
                {
                    if (amount > count)
                    {
                        TempData["mess"] = "Not Much Amoount Available In Bank";
                        TempData["id"] = id;
                        return RedirectToAction("online_pay", "edit");
                    }
                }
                else if (pay.Type == "Cus")
                {
                    if (count + amount < 0)
                    {
                        TempData["mess"] = "Not Much Amoount Available In Bank To Deduct";
                        TempData["id"] = id;
                        return RedirectToAction("online_pay", "edit");
                    }
                }

                var bank1 = db.BankDetail.Where(x => x.pay_id == id && x.typ == null).First();
                if (bank1.ven_cus == "ven")
                {
                    bank1.BdOut = amount;
                }
                else
                {
                    bank1.BdIn = amount;
                }
                bank1.day = date.Day;
                bank1.month = date.Month;
                bank1.year = date.Year;
                bank1.date = date;
                db.SaveChanges();

                var d1 = db.BankDetail.Where(x => x.BdName == bank1.BdName).ToList();
                var count1 = 0;
                foreach (var item in d1)
                {
                    item.BdBalance = Convert.ToInt32(count1 + item.BdIn - item.BdOut);
                    count1 = Convert.ToInt32(item.BdBalance);
                    db.SaveChanges();
                }

                var bankstatement = db.Bankstatements.Where(x => x.BsId == bank1.BdName).First();
                bankstatement.Balance = count1;
            }

            if (pay.Type == "Pet")
            {
                var vendor = db.Vendorledger.Where(x => x.Type == "pay" && x.Orderid == pay.PaId).First();
                vendor.VlOut = amount;
                vendor.day = date.Day;
                vendor.month = date.Month;
                vendor.year = date.Year;
                vendor.date = date;
                db.SaveChanges();

                var vendor_ledger = db.Vendorledger.Where(x => x.VName == vendor.VName).ToList();
                var countr = 0;
                foreach (var item in vendor_ledger)
                {
                    item.VlBalance = countr + item.VlIn - item.VlOut;
                    countr = Convert.ToInt32(item.VlBalance);
                    db.SaveChanges();
                }

            }
            else if (pay.Type == "Cus")
            {
                var customer = db.Customerledger.Where(x => x.type == "Payment" && x.Orderid == pay.PaId).First();
                customer.ClOut = amount;
                customer.day = date.Day;
                customer.month = date.Month;
                customer.year = date.Year;
                customer.date = date;
                db.SaveChanges();

                var customer_ledger = db.Customerledger.Where(x => x.CName == customer.CName).ToList();
                var countr = 0;
                foreach (var item in customer_ledger)
                {
                    item.ClBalance = countr + item.ClIn - item.ClOut;
                    countr = Convert.ToInt32(item.ClBalance);
                    db.SaveChanges();
                }
            }

            pay.PAmount = amount;
            pay.Day = date.Day;
            pay.Month = date.Month;
            pay.Year = date.Year;
            db.SaveChanges();

            return RedirectToAction("payment", "front");
        }
        public IActionResult undo_cheque(int id)
        {
            var cheque = db.cheque.Where(x => x.Ch_Id == id).First();
            if (cheque.Transfer_to == "0")
            {
                var d = db.Das.Where(x => x.DasType == "Pay").ToList();
                var count = 0;
                foreach (var item in d)
                {
                    if (item.typ1 == "cheque" && item.order_id == id)
                    {

                    }
                    else
                    {
                        count = Convert.ToInt32(count + item.DasCredit - item.DasDeit);
                    }
                }
                if (count < 0)
                {
                    TempData["data"] = "Not Much Amoount Available In Das";
                    TempData["id"] = id;
                    return RedirectToAction("cheque", "front");
                }

                var das = db.Das.Where(x => x.typ1 == "cheque" && x.order_id == id).First();
                db.Das.Remove(das);
                db.SaveChanges();

                var d1 = db.Das.Where(x => x.DasType == "Pay").ToList();
                var count1 = 0;

                foreach (var item in d1)
                {
                    item.DasBalance = Convert.ToInt32(count1 + item.DasCredit - item.DasDeit);
                    count1 = Convert.ToInt32(item.DasBalance);
                    db.SaveChanges();
                }
            }
            else
            {
                var bank_id = 0;
                if (cheque.Type == "pet")
                {
                    var bankc = db.Bankstatements.Where(x => x.Bankname == cheque.Cheque_of).First();
                    bank_id = bankc.BsId;
                }
                else if (cheque.Type == "cus")
                {
                    bank_id = Convert.ToInt32(cheque.Transfer_to);
                }


                var bank = db.BankDetail.Where(x => x.BdName == bank_id).ToList();
                var count = 0;
                foreach (var item in bank)
                {
                    if (item.pay_id == id && item.typ == 2)
                    {

                    }
                    else
                    {
                        count = Convert.ToInt32(count + item.BdIn - item.BdOut);
                    }
                }

                if (count < 0)
                {
                    TempData["mess"] = "Not Much Amoount Available In Bank To Deduct";
                    TempData["id"] = id;
                    return RedirectToAction("cheque", "front");
                }

                var bank1 = db.BankDetail.Where(x => x.pay_id == id && x.typ == 2).First();
                db.BankDetail.Remove(bank1);
                db.SaveChanges();

                var d1 = db.BankDetail.Where(x => x.BdName == bank1.BdName).ToList();
                var count1 = 0;
                foreach (var item in d1)
                {
                    item.BdBalance = Convert.ToInt32(count1 + item.BdIn - item.BdOut);
                    count1 = Convert.ToInt32(item.BdBalance);
                    db.SaveChanges();
                }

                var bankstatement = db.Bankstatements.Where(x => x.BsId == bank1.BdName).First();
                bankstatement.Balance = count1;
                db.SaveChanges();
            }

            if (cheque.Type == "pet")
            {
                var vendor = db.Vendorledger.Where(x => x.Type == "Cheque" && x.Orderid == cheque.Ch_Id).First();
                db.Vendorledger.Remove(vendor);
                db.SaveChanges();

                var vendor_ledger = db.Vendorledger.Where(x => x.VName == vendor.VName).ToList();
                var countr = 0;
                foreach (var item in vendor_ledger)
                {
                    item.VlBalance = countr + item.VlIn - item.VlOut;
                    countr = Convert.ToInt32(item.VlBalance);
                    db.SaveChanges();
                }

            }
            else if (cheque.Type == "cus")
            {
                var customer = db.Customerledger.Where(x => x.type == "Cheque" && x.Orderid == cheque.Ch_Id).First();
                db.Customerledger.Remove(customer);
                db.SaveChanges();

                var customer_ledger = db.Customerledger.Where(x => x.CName == customer.CName).ToList();
                var countr = 0;
                foreach (var item in customer_ledger)
                {
                    item.ClBalance = countr + item.ClIn - item.ClOut;
                    countr = Convert.ToInt32(item.ClBalance);
                    db.SaveChanges();
                }
            }


            cheque.Status = 0;
            db.SaveChanges();


            return RedirectToAction("cheque", "front");
        }
        public IActionResult ss(int id)
        {
            var res = db.Secondarysales.Where(x => x.SsId == id).First();
            ViewBag.id = id;
            var cus = db.Customers.Where(x => x.CId == res.CId).First();
            var pro = db.Products.Where(x => x.PId == res.PId).First();
            ViewBag.cus = cus.CName;
            ViewBag.pro = pro.PName;
            var day = Convert.ToInt32(res.Day);
            var month = Convert.ToInt32(res.Month);
            var year = Convert.ToInt32(res.Year);
            DateTime date = new DateTime(year, month, day);
            ViewBag.date = date.ToString("yyyy-MM-dd");
            return View(res);
        }
        [HttpPost]
        public IActionResult ss(int id1, DateTime date)
        {
            var ss = db.Secondarysales.Where(x => x.SsId == id1).First();
            ss.Day = date.Day;
            ss.Month = date.Month;
            ss.Year = date.Year;
            ss.date = date.Date;
            db.SaveChanges();

            if (ss.Type == "Pail")
            {
                var pl = db.looseledger.Where(x => x.type == "Pail" && x.LId == ss.SsOrderno).First();
                pl.day = date.Day;
                pl.month = date.Month;
                pl.year = date.Year;
                db.SaveChanges();
            }
            else if (ss.Type == "Drum")
            {
                var pl = db.Productledgers.Where(x => x.Type == "secondary" && x.pono == ss.SsOrderno && x.Ss_id == ss.SsId  && x.Pid == ss.PId).First();
                pl.day = date.Day;
                pl.month = Convert.ToString(date.Month);
                pl.year = date.Year;
                pl.PlDate = date.Date;
                db.SaveChanges();
            }


            if (ss.Type == "Pail")
            {
                var cl = db.Customerledger.Where(x => x.type == "Pail" && x.Orderid == ss.SsOrderno && x.Ss_id == ss.SsId).First();
                cl.day = date.Day;
                cl.month = date.Month;
                cl.year = date.Year;
                cl.date = date.Date;
                db.SaveChanges();
            }
            else if (ss.Type == "Drum")
            {
                var cl = db.Customerledger.Where(x => x.type == "Drum" && x.Orderid == ss.SsOrderno).First();
                cl.day = date.Day;
                cl.month = date.Month;
                cl.year = date.Year;
                cl.date = date.Date;
                db.SaveChanges();
            }




            return RedirectToAction("ss", "front");
        }
        public IActionResult ps(int id)
        {
            var res = db.Primarysales.Where(x => x.PsId == id).First();
            ViewBag.id = id;
            var pro = db.Products.Where(x => x.PId == Convert.ToInt32(res.PName)).First();
            ViewBag.ven = res.VName;
            ViewBag.pro = pro.PName;
            ViewBag.orid = res.OrderID;

            var day = Convert.ToInt32(res.day);
            var month = Convert.ToInt32(res.month);
            var year = Convert.ToInt32(res.year);
            DateTime date = new DateTime(year, month, day);
            ViewBag.date = date.ToString("yyyy-MM-dd");
            return View(res);

        }
        [HttpPost]
        public IActionResult ps(int id1, DateTime date)
        {
            var ps = db.Primarysales.Where(x => x.PsId == id1).First();
            var ps2 = db.Primarysales.Where(x => x.OrderID == ps.OrderID).ToList();
            foreach(var ps1 in ps2)
            {
                ps1.day = date.Day;
                ps1.month = Convert.ToString(date.Month);
                ps1.year = date.Year;
                ps1.date = date.Date;
            }
            db.SaveChanges();

            var pl1 = db.Productledgers.Where(x => x.Type == "primary" && x.pono == ps.OrderID).ToList();
            foreach(var pl in pl1)
            {
                pl.day = date.Day;
                pl.month = Convert.ToString(date.Month);
                pl.year = date.Year;
                pl.PlDate = date.Date;
                db.SaveChanges();
            }

            var vl1 = db.Vendorledger.Where(x => x.Type == "primary" && x.Orderid == ps.OrderID).ToList();
            foreach(var vl in vl1)
            {
                vl.day = date.Day;
                vl.month = date.Month;
                vl.year = date.Year;
                vl.date = date.Date;
                db.SaveChanges();
            }

            var das = db.Das.Where(x => x.DasType == "p" && x.typ1 == "primary" && x.order_id == ps.OrderID).First();
            das.day = Convert.ToString(date.Day);
            das.month = Convert.ToString(date.Month);
            das.year = Convert.ToString(date.Year);
            das.DasDate = date.Date;
            db.SaveChanges();

            var stock1 = db.StockPrice.Where(x => x.Status == "active" && x.Pono == ps.OrderID).ToList();
            foreach(var stock in stock1)
            {
                stock.Date = date.Day;
                stock.Month = date.Month;
                stock.Year = date.Year;
                db.SaveChanges();
            }
            

            return RedirectToAction("ps", "front");
        }
    }
}
