using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class financioContext : DbContext
    {
        public financioContext()
        {
        }

        public financioContext(DbContextOptions<financioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Cardtype> Cardtype { get; set; }
        public virtual DbSet<Credittransaction> Credittransaction { get; set; }
        public virtual DbSet<Debittransaction> Debittransaction { get; set; }
        public virtual DbSet<Financiouser> Financiouser { get; set; }
        public virtual DbSet<Ifsc> Ifsc { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Scheme> Scheme { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-TSNLJH2;Database=financio;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("bank");

                entity.HasIndex(e => e.Bankname)
                    .HasName("UQ__bank__206168F8CD2C74B5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bankname)
                    .IsRequired()
                    .HasColumnName("bankname")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("card");

                entity.HasIndex(e => e.Accountnumber)
                    .HasName("UQ__card__E762EC1A7C94637E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accountnumber)
                    .IsRequired()
                    .HasColumnName("accountnumber")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Bankid).HasColumnName("bankid");

                entity.Property(e => e.Cardlimit)
                    .HasColumnName("cardlimit")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Cardtypeid).HasColumnName("cardtypeid");

                entity.Property(e => e.Financiouser).HasColumnName("financiouser");

                entity.Property(e => e.Ifscid).HasColumnName("ifscid");

                entity.Property(e => e.Isactive)
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Registrationdate)
                    .HasColumnName("registrationdate")
                    .HasColumnType("date");

                entity.Property(e => e.Validupto)
                    .HasColumnName("validupto")
                    .HasColumnType("date");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Bankid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__card__bankid__6754599E");

                entity.HasOne(d => d.Cardtype)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Cardtypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__card__cardtypeid__66603565");

                entity.HasOne(d => d.FinanciouserNavigation)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Financiouser)
                    .HasConstraintName("FK__card__financious__6383C8BA");

                entity.HasOne(d => d.Ifsc)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Ifscid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__card__ifscid__68487DD7");
            });

            modelBuilder.Entity<Cardtype>(entity =>
            {
                entity.ToTable("cardtype");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cardname)
                    .IsRequired()
                    .HasColumnName("cardname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Charge)
                    .HasColumnName("charge")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Creditlimit)
                    .HasColumnName("creditlimit")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Credittransaction>(entity =>
            {
                entity.ToTable("credittransaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Debittransactionid).HasColumnName("debittransactionid");

                entity.Property(e => e.Transactiondatetime)
                    .HasColumnName("transactiondatetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Debittransaction)
                    .WithMany(p => p.Credittransaction)
                    .HasForeignKey(d => d.Debittransactionid)
                    .HasConstraintName("FK__credittra__debit__5165187F");
            });

            modelBuilder.Entity<Debittransaction>(entity =>
            {
                entity.ToTable("debittransaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balanceleft)
                    .HasColumnName("balanceleft")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Financiouser).HasColumnName("financiouser");

                entity.Property(e => e.Installmentamount)
                    .HasColumnName("installmentamount")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Isactive)
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lastpaymentdatetime)
                    .HasColumnName("lastpaymentdatetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Schemeid).HasColumnName("schemeid");

                entity.Property(e => e.Transactiondatetime)
                    .HasColumnName("transactiondatetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FinanciouserNavigation)
                    .WithMany(p => p.Debittransaction)
                    .HasForeignKey(d => d.Financiouser)
                    .HasConstraintName("FK__debittran__finan__48CFD27E");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Debittransaction)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__debittran__produ__49C3F6B7");

                entity.HasOne(d => d.Scheme)
                    .WithMany(p => p.Debittransaction)
                    .HasForeignKey(d => d.Schemeid)
                    .HasConstraintName("FK__debittran__schem__4AB81AF0");
            });

            modelBuilder.Entity<Financiouser>(entity =>
            {
                entity.ToTable("financiouser");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__financio__AB6E616496272F12")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__financio__F3DBC5723517E826")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isadmin)
                    .HasColumnName("isadmin")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ifsc>(entity =>
            {
                entity.ToTable("ifsc");

                entity.HasIndex(e => e.Ifsccode)
                    .HasName("UQ__ifsc__0BDE08BC057DE6CF")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bankid).HasColumnName("bankid");

                entity.Property(e => e.Ifsccode)
                    .IsRequired()
                    .HasColumnName("ifsccode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Ifsc)
                    .HasForeignKey(d => d.Bankid)
                    .HasConstraintName("FK__ifsc__bankid__35BCFE0A");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Extrafeatures)
                    .HasColumnName("extrafeatures")
                    .HasColumnType("text");

                entity.Property(e => e.Imageurl)
                    .IsRequired()
                    .HasColumnName("imageurl")
                    .HasColumnType("text");

                entity.Property(e => e.Productdetails)
                    .IsRequired()
                    .HasColumnName("productdetails")
                    .HasColumnType("text");

                entity.Property(e => e.Productname)
                    .IsRequired()
                    .HasColumnName("productname")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Scheme>(entity =>
            {
                entity.ToTable("scheme");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Schemeduration).HasColumnName("schemeduration");

                entity.Property(e => e.Schemename)
                    .IsRequired()
                    .HasColumnName("schemename")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
