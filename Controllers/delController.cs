using be.Models;
using Microsoft.AspNetCore.Mvc;

namespace be.Controllers
{
    public class delController : Controller
    {
        BentContext  db = new BentContext();
        public IActionResult user(int id)
        {
            var res = db.Users.Where(x=>x.UId == id).First();
            db.Users.Remove(res);
            db.SaveChanges();
            return RedirectToAction("user","front");
        }
        public IActionResult product(int id)
        {
            var res = db.Products.Where(x => x.PId == id).First();
            db.Products.Remove(res);
            var x = db.cus_pro.Where(x => x.pid == id).ToList();
            foreach(var item in x)
            {
                db.cus_pro.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("product", "front");
        }
        public IActionResult customer(int id)
        {
            var res = db.Customers.Where(x => x.CId == id).First();
            db.Customers.Remove(res);
            var x = db.cus_pro.Where(x => x.cid == id).ToList();
            foreach (var item in x)
            {
                db.cus_pro.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("customer", "front");
        }
        public IActionResult vendor(int id)
        {
            var res = db.Venders.Where(x => x.VId == id).First();
            db.Venders.Remove(res);
            db.SaveChanges();
            return RedirectToAction("vendor", "front");
        }
        public IActionResult bank_statement(int id)
        {
            var res = db.Bankstatements.Where(x => x.BsId == id).First();
            db.Bankstatements.Remove(res);
            db.SaveChanges();
            return RedirectToAction("bank_statement", "front");
        }
        public IActionResult followup(int id)
        {
            var res = db.Followups.Where(x => x.FuId == id).First();
            db.Followups.Remove(res);
            db.SaveChanges();
            return RedirectToAction("followup", "front");
        }
        public IActionResult po(int id)
        {
            var res = db.Pos.Where(x => x.PoUnique == id);
            var res1 = db.PoNos.Where(x=>x.PonoId== id).First();
            db.PoNos.Remove(res1);
            foreach(var item in res)
            {
                db.Pos.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("po", "front");
        }
		public IActionResult po1(int id)
		{
            var x = db.Pos.Where(x => x.PoId == id).First();
            var iid = x.PoUnique;
            db.Pos.Remove(x);
            db.SaveChanges();
            var p1 = db.Pos.Any(x => x.PoUnique == iid);
            if(p1 is true)
            {
                var p = db.Pos.Where(x => x.PoUnique == iid);
                var count = 1;
                foreach (var item in p)
                {
                    item.Count = count;
                    count = count + 1;
                }
            }
            db.SaveChanges();
            TempData["id"] = x.PoUnique;
			return RedirectToAction("po", "det");
		}
        public IActionResult order(int id)
        {
            var x = db.Order.Where(x => x.OId == id).First();
            var iid = x.OrUnique;
            db.Order.Remove(x);
            db.SaveChanges();
            var p = db.Order.Where(x => x.OrUnique == iid);
            var count = 1;
            foreach (var item in p)
            {
                item.Count = count;
                count = count + 1;
            }
            db.SaveChanges();
            TempData["id"] = x.OrUnique;
            return RedirectToAction("order", "det");
        }
        public IActionResult order1(int id)
        {
            var re = db.Order.Any(x => x.OrUnique == id);
            var res1 = db.OrderNos.Where(x => x.OrdernoId == id).First();
            db.OrderNos.Remove(res1);
            if(re is true)
            {
                var res = db.Order.Where(x => x.OrUnique == id);
                foreach (var item in res)
                {
                    db.Order.Remove(item);
                }
            }
            db.SaveChanges();
            return RedirectToAction("orno", "front");
        }
        public IActionResult cheque(int id)
        {
            var res = db.cheque.Where(x => x.Ch_Id == id).First();
            db.cheque.Remove(res);
            db.SaveChanges();
            return RedirectToAction("cheque", "front");
        }
        public IActionResult das(int id)
        {
            var res = db.Das.Where(x => x.DasId == id).First();
            db.Das.Remove(res);
            db.SaveChanges();

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
            var x1 = db.BankDetail.Where(x => x.BdId == id).First();
            var bb = db.Bankstatements.Where(x => x.BsId == x1.BdName).First();
            db.BankDetail.Remove(x1);
            db.SaveChanges();

            var x2 = db.BankDetail.Where(x => x.BdName == bb.BsId).ToList();
            var count = 0;
            var balance = 0;
            foreach (var item in x2)
            {
                if (item.BdOut == 0 || item.BdOut == null)
                {
                    if (count == 0)
                    {
                        item.BdBalance = item.BdIn;
                        balance = Convert.ToInt32(item.BdIn);
                    }
                    else
                    {
                        item.BdBalance = balance + item.BdIn;
                        balance = balance + Convert.ToInt32(item.BdIn);
                    }
                }
                else if (item.BdIn == 0 || item.BdIn == null)
                {
                    if (count == 0)
                    {
                        item.BdBalance = -item.BdOut;
                        balance = Convert.ToInt32(-item.BdOut);
                    }
                    else
                    {
                        item.BdBalance = balance - item.BdOut;
                        balance = balance - Convert.ToInt32(item.BdOut);
                    }
                }
                count = count + 1;
                db.SaveChanges();
            }

            bb.Balance = balance;
            db.SaveChanges();
            return RedirectToAction("bank_statement", "front");
        }
        public IActionResult loose_det(int id,int loose_id)
        {
            var loose_ledger = db.looseledger.Where(x => x.LId == loose_id).First();
            db.looseledger.Remove(loose_ledger);
            db.SaveChanges();

            var loose_ledger1 = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
            var count = 0;
            foreach(var item in loose_ledger1)
            {
                item.lBalance = count + item.lIn - item.lOut;
                count = Convert.ToInt32(item.lBalance);
                db.SaveChanges();
            }

            var stock_price = db.StockPrice.Where(x => x.loose_id == loose_id).First();
            db.StockPrice.Remove(stock_price);
            db.SaveChanges();

            var cus = db.Customerledger.Where(x => x.type == "Pail" && x.Orderid == loose_id).First();
            db.Customerledger.Remove(cus);
            db.SaveChanges();

            var cus_set = db.Customerledger.Where(x => x.CName == cus.CName).ToList();
            var count1 = 0;
            foreach(var item in cus_set)
            {
                item.ClBalance = count1 + item.ClIn - item.ClOut;
                count1 = Convert.ToInt32(item.ClBalance);
                db.SaveChanges();
            }

            var ss = db.Secondarysales.Where(x => x.Type == "Pail" && x.SsOrderno == loose_id).First();
            db.Secondarysales.Remove(ss);
            db.SaveChanges();

            TempData["id"] = Convert.ToInt32(id);
            return RedirectToAction("looseledger", "front");
        }
        public IActionResult loose_det_add(int id, int loose_id)
        {
            var ll = db.looseledger.Where(x => x.LId == loose_id).First();
            var ll_bal = db.looseledger.Where(x => x.Description == Convert.ToString(id) && x.LId != loose_id).ToList();
            var count = 0;
            foreach(var item in ll_bal)
            {
                item.lBalance = count + item.lIn - item.lOut;
                count = Convert.ToInt32(item.lBalance);
            }

            if (count < 0)
            {
                TempData["mess"] = "This Action can't be performed as it will make the balance negative";
                TempData["id"] = Convert.ToInt32(id);
                return RedirectToAction("looseledger", "front");
            }

            db.looseledger.Remove(ll);
            db.SaveChanges();

            var loose_set = db.looseledger.Where(x => x.Description == Convert.ToString(id)).ToList();
            var count11 = 0;
            foreach (var item in loose_set)
            {
                item.lBalance = count11 + item.lIn - item.lOut;
                count11 = Convert.ToInt32(item.lBalance);
                db.SaveChanges();
            }

            var cus = db.Customerledger.Where(x => x.type == "Pail" && x.Orderid == loose_id).First();
            db.Customerledger.Remove(cus);

            var cus_set = db.Customerledger.Where(x => x.CName == cus.CName).ToList();
            var count1 = 0;
            foreach (var item in cus_set)
            {
                item.ClBalance = count1 + item.ClIn - item.ClOut;
                count1 = Convert.ToInt32(item.ClBalance);
                db.SaveChanges();
            }

            db.SaveChanges();

            TempData["id"] = Convert.ToInt32(id);
            return RedirectToAction("looseledger", "front");
        }
        public IActionResult online_pay(int id)
        {
            var pay = db.Pay.Where(x => x.PaId == id).First();
            
            if(pay.Mode == "Cash")
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

                if (count<0)
                {
                    TempData["data"] = "Not Much Amount Available In Das To Delete";
                    TempData["id"] = id;
                    return RedirectToAction("payment", "front");
                }

                var das = db.Das.Where(x => x.typ1 == "cash" && x.order_id == id).First();
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
                var bank = db.BankDetail.Where(x=>x.BdName == pay.PayerName).ToList();
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

                if (count < 0)
                {
                    TempData["mess"] = "Not Much Amoount Available In Bank To Deduct";
                    TempData["id"] = id;
                    return RedirectToAction("payment", "front");
                }

                var bank1 = db.BankDetail.Where(x => x.pay_id == id && x.typ == null).First();
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

            if (pay.Type == "Pet")
            {
                var vendor = db.Vendorledger.Where(x => x.Type == "pay" && x.Orderid == pay.PaId).First();
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
            else if (pay.Type == "Cus")
            {
                var customer = db.Customerledger.Where(x => x.type == "Payment" && x.Orderid == pay.PaId).First();
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


            db.Pay.Remove(pay);
            db.SaveChanges();



            return RedirectToAction("payment", "front");
        }
    }
}
