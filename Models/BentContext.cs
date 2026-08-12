using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using be.Models;

namespace be.Models;

public partial class BentContext : DbContext
{
    public BentContext()
    {
    }

    public BentContext(DbContextOptions<BentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bankstatement> Bankstatements { get; set; }
    public virtual DbSet<BankDetail> BankDetail { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<looseprice> LoosePrice { get; set; }
    public virtual DbSet<StockPrice> StockPrice { get; set; }
    public virtual DbSet<del_no> del_no { get; set; }

    public virtual DbSet<Da> Das { get; set; }

    public virtual DbSet<Dc> Dcs { get; set; }
    public virtual DbSet<cus_pro> cus_pro { get; set; }
    public virtual DbSet<delivery> delivery { get; set; }

    public virtual DbSet<Followup> Followups { get; set; }

    public virtual DbSet<Grn> Grns { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Mcr> Mcrs { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderNo> OrderNos { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<cheque> cheque { get; set; }
    public virtual DbSet<Po> Pos { get; set; }

    public virtual DbSet<PoNo> PoNos { get; set; }

    public virtual DbSet<Primarysale> Primarysales { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<pay_type> pay_type { get; set; }
    public virtual DbSet<Pay> Pay { get; set; }
    public virtual DbSet<Customerledger> Customerledger { get; set; }

    public virtual DbSet<looseledger> looseledger { get; set; }

    public virtual DbSet<Vendorledger> Vendorledger { get; set; }

    public virtual DbSet<Productledger> Productledgers { get; set; }

    public virtual DbSet<Secondarysale> Secondarysales { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vender> Venders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
           => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=check_bent;Trusted_Connection=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bankstatement>(entity =>
        {
            entity.HasKey(e => e.BsId).HasName("PK__bankstat__1A331041AF06F5FC");

            entity.ToTable("bankstatement");

            entity.Property(e => e.BsId).HasColumnName("bs_id");
            entity.Property(e => e.Accountno)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("accountno");
            entity.Property(e => e.Accounttitle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("accounttitle");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.Bankname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("bankname");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK__customer__213EE7748C1FF0C7");

            entity.ToTable("customers");

            entity.Property(e => e.CId).HasColumnName("c_id");
            entity.Property(e => e.CAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("c_address");
            entity.Property(e => e.CEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_email");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.CPhone)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_phone");
            entity.Property(e => e.CStatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_status");
            entity.Property(e => e.auth_per).HasColumnName("auth_per");
        });

        modelBuilder.Entity<del_no>(entity =>
        {
            entity.HasKey(e => e.DId).HasName("PK__das__A5D7BC54AD5D9D87");

            entity.ToTable("del_no");
            entity.Property(e => e.DId).HasColumnName("id");
            entity.Property(e => e.CId).HasColumnName("c_id");
        });

        modelBuilder.Entity<delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__das__A5D7BC54AD5D9D95");

            entity.ToTable("delivery");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.del_no).HasColumnName("del_no");
            entity.Property(e => e.qty).HasColumnName("qty");
            entity.Property(e => e.p_id).HasColumnName("pid");
        });

        modelBuilder.Entity<cus_pro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__das__A5D7BC54AD5D9D81");

            entity.ToTable("cus_pro");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.cid).HasColumnName("c_id");
            entity.Property(e => e.pid).HasColumnName("p_id");
            entity.Property(e => e.baseprice).HasColumnName("base_price");
        });

        modelBuilder.Entity<Da>(entity =>
        {
            entity.HasKey(e => e.DasId).HasName("PK__das__A5D7BC54AD5D9D44");

            entity.ToTable("das");

            entity.Property(e => e.DasId).HasColumnName("das_id");
            entity.Property(e => e.DasBalance).HasColumnName("das_balance");
            entity.Property(e => e.DasCredit).HasColumnName("das_credit");
            entity.Property(e => e.DasType).HasColumnName("type");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.typ1).HasColumnName("typ1");
            entity.Property(e => e.order_id).HasColumnName("order_id");
            entity.Property(e => e.ven_cus).HasColumnName("ven_cus");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.DasDate)
                .HasColumnType("date")
                .HasColumnName("das_date");
            entity.Property(e => e.DasDeit).HasColumnName("das_deit");
            entity.Property(e => e.DasDescrition)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("das_descrition");
            entity.Property(e => e.DasExpense)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("das_expense");
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.BdId).HasName("PK__das__A5D7BC54AD5D9D96");

            entity.ToTable("bank_detail");

            entity.Property(e => e.BdId).HasColumnName("bd_id");
            entity.Property(e => e.BdName).HasColumnName("bd_name");
            entity.Property(e => e.BdSender).HasColumnName("bd_sender");
            entity.Property(e => e.date).HasColumnName("date");
            entity.Property(e => e.BdIn).HasColumnName("bd_in");
            entity.Property(e => e.typ).HasColumnName("typ");
            entity.Property(e => e.pay_id).HasColumnName("pay_id");
            entity.Property(e => e.BdOut).HasColumnName("bd_out");
            entity.Property(e => e.BdBalance).HasColumnName("bd_balance");
            entity.Property(e => e.ven_cus).HasColumnName("ven_cus");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");
        });

        modelBuilder.Entity<Dc>(entity =>
        {
            entity.HasKey(e => e.DcId).HasName("PK__dc__33FDC9754A869852");

            entity.ToTable("dc");

            entity.Property(e => e.DcId).HasColumnName("dc_id");
            entity.Property(e => e.DcDate)
                .HasColumnType("date")
                .HasColumnName("dc_date");
            entity.Property(e => e.SsOrderno).HasColumnName("ss_orderno");
        });

        modelBuilder.Entity<Followup>(entity =>
        {
            entity.HasKey(e => e.FuId).HasName("PK__followup__ACE7DEEEE74A45FC");

            entity.ToTable("followup");

            entity.Property(e => e.FuId).HasColumnName("fu_id");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.FuDate)
                .HasColumnType("date")
                .HasColumnName("fu_date");
            entity.Property(e => e.FuDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("fu_description");
            entity.Property(e => e.FuEntered)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("fu_entered");
        });

        modelBuilder.Entity<Grn>(entity =>
        {
            entity.HasKey(e => e.GrnId).HasName("PK__grn__39D8A22AF3EC2A63");

            entity.ToTable("grn");

            entity.Property(e => e.GrnId).HasColumnName("grn_id");
            entity.Property(e => e.GrnDate)
                .HasColumnType("date")
                .HasColumnName("grn_date");
            entity.Property(e => e.PoId).HasColumnName("po_id");
            entity.Property(e => e.Pono).HasColumnName("pono");
            entity.Property(e => e.GrnDc).HasColumnName("grn_dc");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");
            entity.HasOne(d => d.Po).WithMany(p => p.Grns)
                .HasForeignKey(d => d.PoId)
                .HasConstraintName("FK__grn__po_id__44FF419A");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InId).HasName("PK__invoice__1CD08BE9212A9ABA");

            entity.ToTable("invoice");

            entity.Property(e => e.InId).HasColumnName("in_id");
            entity.Property(e => e.InDate)
                .HasColumnType("date")
                .HasColumnName("in_date");
            entity.Property(e => e.SsOrderno).HasColumnName("ss_orderno");
            entity.Property(e => e.InDate1).HasColumnName("in_date1");
        });

        modelBuilder.Entity<Mcr>(entity =>
        {
            entity.HasKey(e => e.McrId).HasName("PK__mcr__07ECD20CB63A73D7");

            entity.ToTable("mcr");

            entity.Property(e => e.McrId).HasColumnName("mcr_id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.McrStatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("mcr_status");
            entity.Property(e => e.SupplyDate)
                .HasColumnType("date")
                .HasColumnName("supply_date");
        });

        modelBuilder.Entity<OrderNo>(entity =>
        {
            entity.HasKey(e => e.OrdernoId).HasName("PK__order_no__6ADDB4B5F44E122C");

            entity.ToTable("order_no");

            entity.Property(e => e.OrdernoId).HasColumnName("orderno_id");
            entity.Property(e => e.Customer).HasColumnName("customer");
            entity.Property(e => e.OrdernoStatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("orderno_status");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OId).HasName("PK__order1__6ADDB4B5F44E122C");
            entity.ToTable("order1");

            entity.Property(e => e.OId).HasColumnName("o_id");
            entity.Property(e => e.CId).HasColumnName("c_id");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.delivered).HasColumnName("delivered");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.OrUnique).HasColumnName("or_unique");
            entity.Property(e => e.OPrice).HasColumnName("o_price");
            entity.Property(e => e.type).HasColumnName("type");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");

        });

        modelBuilder.Entity<cheque>(entity =>
        {
            entity.HasKey(e => e.Ch_Id).HasName("PK__order1__6ADDB4B5F44E145C");
            entity.ToTable("cheque");

            entity.Property(e => e.Ch_Id).HasColumnName("id");
            entity.Property(e => e.CName).HasColumnName("c_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Cheque_of).HasColumnName("cheque_of");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Pdc_Day).HasColumnName("pdc_day");
            entity.Property(e => e.Pdc_Month).HasColumnName("pdc_month");
            entity.Property(e => e.Pdc_Year).HasColumnName("pdc_year");
            entity.Property(e => e.Transfer_to).HasColumnName("transfer_to");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Type).HasColumnName("type");

        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__payment__7AAD1CEABCF42D30");

            entity.ToTable("payment");

            entity.Property(e => e.PayId).HasColumnName("pay_id");
            entity.Property(e => e.CId).HasColumnName("c_id");
            entity.Property(e => e.PayAmount).HasColumnName("pay_amount");
            entity.Property(e => e.PayDate)
                .HasColumnType("date")
                .HasColumnName("pay_date");
            entity.Property(e => e.PayDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pay_description");
            entity.Property(e => e.PayTo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pay_to");
            entity.Property(e => e.PayType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pay_type");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CId)
                .HasConstraintName("FK__payment__c_id__5AEE82B9");
        });

        modelBuilder.Entity<Po>(entity =>
        {
            entity.HasKey(e => e.PoId).HasName("PK__po__368DA7F064311052");

            entity.ToTable("po");

            entity.Property(e => e.PoId).HasColumnName("po_id");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PoDate)
                .HasColumnType("date")
                .HasColumnName("po_date");
            entity.Property(e => e.PoPrice).HasColumnName("po_price");
            entity.Property(e => e.PoQty).HasColumnName("po_qty");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.baseprltr).HasColumnName("base_pr_ltr");
            entity.Property(e => e.time).HasColumnName("time");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.PoUnique).HasColumnName("po_unique");
            entity.Property(e => e.VId).HasColumnName("v_id");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.Pos)
                .HasForeignKey(d => d.PId)
                .HasConstraintName("FK__po__p_id__4222D4EF");

            entity.HasOne(d => d.VIdNavigation).WithMany(p => p.Pos)
                .HasForeignKey(d => d.VId)
                .HasConstraintName("FK__po__v_id__412EB0B6");
        });


        modelBuilder.Entity<PoNo>(entity =>
        {
            entity.HasKey(e => e.PonoId).HasName("PK__po_no__CBBED65397F6CF14");

            entity.ToTable("po_no");

            entity.Property(e => e.PonoId).HasColumnName("pono_id");
            entity.Property(e => e.Vendor).HasColumnName("vendor");
            entity.Property(e => e.PonoStatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pono_status");
        });

        modelBuilder.Entity<Pay>(entity =>
        {
            entity.HasKey(e => e.PaId).HasName("PK__po_no__CBBED65397F6CF22");

            entity.ToTable("pay");

            entity.Property(e => e.PaId).HasColumnName("pa_id");
            entity.Property(e => e.PayerName).HasColumnName("payer_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.PAmount).HasColumnName("p_amount");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Mode).HasColumnName("mode");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<pay_type>(entity =>
        {
            entity.HasKey(e => e.PaId).HasName("PK__po_no__CBBED65397F6CF32");

            entity.ToTable("pay_type");

            entity.Property(e => e.PaId).HasColumnName("pa_id");
            entity.Property(e => e.PayerType).HasColumnName("payer_type");
        });

        modelBuilder.Entity<Primarysale>(entity =>
        {
            entity.HasKey(e => e.PsId).HasName("PK__primarys__5CFD143FA9E165D5");

            entity.ToTable("primarysales");

            entity.Property(e => e.PsId).HasColumnName("ps_id");
            entity.Property(e => e.DcNo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("dc_no");
            entity.Property(e => e.PName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("p_name");
            entity.Property(e => e.PsDate)
                .HasColumnType("date")
                .HasColumnName("ps_date");
            entity.Property(e => e.PsPack).HasColumnName("ps_pack");
            entity.Property(e => e.PsPrice).HasColumnName("ps_price");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.date).HasColumnName("date");
            entity.Property(e => e.year).HasColumnName("year1");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.OrderID).HasColumnName("order_id");
            entity.Property(e => e.time).HasColumnName("time");
			entity.Property(e => e.Price).HasColumnName("price");
			entity.Property(e => e.Total).HasColumnName("total");
			entity.Property(e => e.PsQty).HasColumnName("ps_qty");
            entity.Property(e => e.VName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__products__82E06B917BE22364");

            entity.ToTable("products");

            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("p_name");
            entity.Property(e => e.PPack).HasColumnName("p_pack");
            entity.Property(e => e.PCode).HasColumnName("p_code");
            entity.Property(e => e.PPr).HasColumnName("p_pr");
            entity.Property(e => e.PSp).HasColumnName("p_sp");
			entity.Property(e => e.Trade_disc).HasColumnName("Trade_disc");
			entity.Property(e => e.PType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("p_type");
        });

        modelBuilder.Entity<Productledger>(entity =>
        {
            entity.HasKey(e => e.PlId).HasName("PK__productl__0CBEC885B61BB4B4");

            entity.ToTable("productledger");

            entity.Property(e => e.PlId).HasColumnName("pl_id");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.PlBalance).HasColumnName("pl_balance");
            entity.Property(e => e.PlDate)
                .HasColumnType("date")
                .HasColumnName("pl_date");
            entity.Property(e => e.Pid).HasColumnName("p_id");
            entity.Property(e => e.Ss_id).HasColumnName("ss_id");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.pono).HasColumnName("pono");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.PlIn).HasColumnName("pl_in");
            entity.Property(e => e.PlOut).HasColumnName("pl_out");
        });

        modelBuilder.Entity<Vendorledger>(entity =>
        {
            entity.HasKey(e => e.VlId).HasName("PK__productl__0CBEC885B71BB4B5");

            entity.ToTable("vendor_ledger");

            entity.Property(e => e.VlId).HasColumnName("id");
            entity.Property(e => e.VName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Orderid).HasColumnName("order_id");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.date).HasColumnName("date");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.VlIn).HasColumnName("vl_in");
            entity.Property(e => e.VlOut).HasColumnName("vl_out");
            entity.Property(e => e.VlBalance).HasColumnName("vl_balance");
        });

        modelBuilder.Entity<Customerledger>(entity =>
        {
            entity.HasKey(e => e.ClId).HasName("PK__productl__0CBEC855B71BB49");

            entity.ToTable("customer_ledger");

            entity.Property(e => e.ClId).HasColumnName("cl_id");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Orderid).HasColumnName("order_id");
            entity.Property(e => e.Ss_id).HasColumnName("ss_id");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.date).HasColumnName("date");
            entity.Property(e => e.time).HasColumnName("time");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.ClIn).HasColumnName("cl_in");
            entity.Property(e => e.ClOut).HasColumnName("cl_out");
            entity.Property(e => e.type).HasColumnName("type");
            entity.Property(e => e.ClBalance).HasColumnName("cl_balance");
        });

        modelBuilder.Entity<looseledger>(entity =>
        {
            entity.HasKey(e => e.LId).HasName("PK__productl__0CBEC845B71BB43");

            entity.ToTable("loose");

            entity.Property(e => e.LId).HasColumnName("l_id");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("c_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.lIn).HasColumnName("l_in");
            entity.Property(e => e.lOut).HasColumnName("l_out");
            entity.Property(e => e.lBalance).HasColumnName("l_balance");
            entity.Property(e => e.day).HasColumnName("day");
            entity.Property(e => e.month).HasColumnName("month");
            entity.Property(e => e.year).HasColumnName("year");
            entity.Property(e => e.time).HasColumnName("time");
            entity.Property(e => e.type).HasColumnName("type");
        });

        modelBuilder.Entity<Secondarysale>(entity =>
        {
            entity.HasKey(e => e.SsId).HasName("PK__secondar__A445C6A2F4288630");

            entity.ToTable("secondarysales");

            entity.Property(e => e.SsId).HasColumnName("ss_id");
            entity.Property(e => e.CId).HasColumnName("c_id");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Pr_Price).HasColumnName("pr_price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Pack).HasColumnName("pack");
            entity.Property(e => e.date).HasColumnName("date");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.TPrice).HasColumnName("t_price");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.SsOrderno).HasColumnName("ss_orderno");
            entity.Property(e => e.SsQty).HasColumnName("ss_qty");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Secondarysales)
                .HasForeignKey(d => d.CId)
                .HasConstraintName("FK__secondarys__c_id__4E88ABD4");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.Secondarysales)
                .HasForeignKey(d => d.PId)
                .HasConstraintName("FK__secondarys__p_id__4D94879B");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__stock__2F3684F4E084A368");

            entity.ToTable("stock");

            entity.Property(e => e.SId).HasColumnName("s_id");
            entity.Property(e => e.PName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("p_name");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PPack).HasColumnName("p_pack");
            entity.Property(e => e.SQty).HasColumnName("s_qty");
        });

        modelBuilder.Entity<StockPrice>(entity =>
        {

            entity.ToTable("stock_price");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PId).HasColumnName("pid");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Pono).HasColumnName("pono");
            entity.Property(e => e.Pack).HasColumnName("pack");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Order_id).HasColumnName("order_id");
            entity.Property(e => e.loose_id).HasColumnName("loose_id");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<looseprice>(entity =>
        {

            entity.ToTable("loose_price");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PId).HasColumnName("pid");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Pack).HasColumnName("pack");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PK__users__B51D3DEA704F15F1");

            entity.ToTable("users");

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("u_email");
            entity.Property(e => e.UName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("u_name");
            entity.Property(e => e.Pass)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UPhone)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("u_phone");
            entity.Property(e => e.URole).HasColumnName("u_role");
        });

        modelBuilder.Entity<Vender>(entity =>
        {
            entity.HasKey(e => e.VId).HasName("PK__venders__AD3D844187A5EE39");

            entity.ToTable("venders");

            entity.Property(e => e.VId).HasColumnName("v_id");
            entity.Property(e => e.VEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_email");
            entity.Property(e => e.VName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_name");
            entity.Property(e => e.VNtn)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_ntn");
            entity.Property(e => e.VPhone)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("v_phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<be.Models.Order>? Order_1 { get; set; }
}
