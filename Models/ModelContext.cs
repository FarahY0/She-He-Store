using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HeAndSheStore.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Copon> Copons { get; set; }

    public virtual DbSet<Fav> Favs { get; set; }

    public virtual DbSet<Itorder> Itorders { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Orderr> Orderrs { get; set; }

    public virtual DbSet<Ourteam> Ourteams { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Slider> Sliders { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    public virtual DbSet<Videoservice> Videoservices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = xe)));User Id=FirstProject;Password=12345678;Persist Security Info=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("FIRSTPROJECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Aboutusid).HasName("SYS_C008463");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Aboutusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTUSID");
            entity.Property(e => e.Content)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Description)
                .HasColumnType("NUMBER")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Imagepathtwo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATHTWO");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008489");

            entity.ToTable("BILLING");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Copon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COPON");
            entity.Property(e => e.Country)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Lname)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Zip)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ZIP");

            entity.HasOne(d => d.User).WithMany(p => p.Billings)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_USER");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Blogid).HasName("SYS_C008476");

            entity.ToTable("BLOG");

            entity.Property(e => e.Blogid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("BLOGID");
            entity.Property(e => e.Blogdate)
                .HasColumnType("DATE")
                .HasColumnName("BLOGDATE");
            entity.Property(e => e.Blogtitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BLOGTITLE");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERID4");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Cartid).HasName("SYS_C008458");

            entity.ToTable("CART");

            entity.Property(e => e.Cartid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARTID");
            entity.Property(e => e.Cardid)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ORDERSTATUS");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.Totalprice)
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALPRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Card).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Cardid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PAYMENTID1");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PRODUCTID1");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERID2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008436");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("SYS_C008465");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Maplocation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MAPLOCATION");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Copon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008524");

            entity.ToTable("COPON");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Copontext)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COPONTEXT");
            entity.Property(e => e.Discoundvalue)
                .HasColumnType("NUMBER")
                .HasColumnName("DISCOUNDVALUE");
            entity.Property(e => e.Enddate)
                .HasColumnType("DATE")
                .HasColumnName("ENDDATE");
            entity.Property(e => e.Startdate)
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");
        });

        modelBuilder.Entity<Fav>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008520");

            entity.ToTable("FAV");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Favs)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PRODUCTID5");

            entity.HasOne(d => d.User).WithMany(p => p.Favs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERID9");
        });

        modelBuilder.Entity<Itorder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008508");

            entity.ToTable("ITORDER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Itemprice)
                .HasColumnType("FLOAT")
                .HasColumnName("ITEMPRICE");
            entity.Property(e => e.Orderid)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("QUANTITY");

            entity.HasOne(d => d.Order).WithMany(p => p.Itorders)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKORDERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Itorders)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKPRODUCTID");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Notificationid).HasName("SYS_C008471");

            entity.ToTable("NOTIFICATION");

            entity.Property(e => e.Notificationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("NOTIFICATIONID");
            entity.Property(e => e.Datesent)
                .HasColumnType("DATE")
                .HasColumnName("DATESENT");
            entity.Property(e => e.Message)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERID3");
        });

        modelBuilder.Entity<Orderr>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("SYS_C008502");

            entity.ToTable("ORDERR");

            entity.Property(e => e.Orderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Billingid)
                .HasColumnType("NUMBER")
                .HasColumnName("BILLINGID");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Status)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Totalamount)
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALAMOUNT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Billing).WithMany(p => p.Orderrs)
                .HasForeignKey(d => d.Billingid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKBILLING");

            entity.HasOne(d => d.User).WithMany(p => p.Orderrs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKUSERID");
        });

        modelBuilder.Entity<Ourteam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008485");

            entity.ToTable("OURTEAM");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Empgmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMPGMAIL");
            entity.Property(e => e.Empinstagram)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMPINSTAGRAM");
            entity.Property(e => e.Emplinkedin)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMPLINKEDIN");
            entity.Property(e => e.Empname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMPNAME");
            entity.Property(e => e.Empposition)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMPPOSITION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008443");

            entity.ToTable("PAYMENT");

            entity.HasIndex(e => e.Cardnumber, "SYS_C008444").IsUnique();

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Billingid)
                .HasColumnType("NUMBER")
                .HasColumnName("BILLINGID");
            entity.Property(e => e.Cardholdername)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDERNAME");
            entity.Property(e => e.Cardnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cardtype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CARDTYPE");
            entity.Property(e => e.Cartid)
                .HasColumnType("NUMBER")
                .HasColumnName("CARTID");
            entity.Property(e => e.Cvv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRATIONDATE");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Billing).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Billingid)
                .HasConstraintName("FK_BILL");

            entity.HasOne(d => d.Product).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("FK_PAYMENTPRODUCT");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PAYMENTID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("SYS_C008448");

            entity.ToTable("PRODUCT");

            entity.Property(e => e.Productid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.Productname)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PRODUCTNAME");
            entity.Property(e => e.Sale)
                .HasColumnType("NUMBER")
                .HasColumnName("SALE");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("STATUS");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CATEGORYID");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("SYS_C008451");

            entity.ToTable("REVIEW");

            entity.Property(e => e.Reviewid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REVIEWID");
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Rating)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RATING");
            entity.Property(e => e.Reviewdate)
                .HasColumnType("DATE")
                .HasColumnName("REVIEWDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_REVIEWID2");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_REVIEWID1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008427");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Serviceid).HasName("SYS_C008474");

            entity.ToTable("SERVICE");

            entity.Property(e => e.Serviceid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SERVICEID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Servicename)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SERVICENAME");
            entity.Property(e => e.Videourl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIDEOURL");
        });

        modelBuilder.Entity<Slider>(entity =>
        {
            entity.HasKey(e => e.Slideid).HasName("SYS_C008467");

            entity.ToTable("SLIDER");

            entity.Property(e => e.Slideid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SLIDEID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEURL");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("SYS_C008482");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.TestimonialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIAL_ID");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.Createdate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDATE");
            entity.Property(e => e.Isapproved)
                .HasPrecision(1)
                .HasColumnName("ISAPPROVED");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TESTIMONIAL");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008432");

            entity.ToTable("USER_LOGIN");

            entity.HasIndex(e => e.Email, "SYS_C008433").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Confirmnewpassword)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONFIRMNEWPASSWORD");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Newpassword)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NEWPASSWORD");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Rememberme)
                .HasColumnType("NUMBER")
                .HasColumnName("REMEMBERME");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK_ROLEID1");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERID");
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008429");

            entity.ToTable("USERR");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");

            entity.HasOne(d => d.Role).WithMany(p => p.Userrs)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ROLEID");
        });

        modelBuilder.Entity<Videoservice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008480");

            entity.ToTable("VIDEOSERVICE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Videourl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIDEOURL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
