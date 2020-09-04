using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerceApi.Models
{
    public partial class ECommerceDBContext : DbContext
    {
        public ECommerceDBContext()
        {
        }

        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PaymentData> PaymentData { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductByCategory> ProductByCategory { get; set; }
        public virtual DbSet<ProductByOrder> ProductByOrder { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=localhost;initial catalog=ECommerceDB;persist security info=True;user id=sa;password=Aa123456;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.IdBrand)
                    .HasName("PK__Brand__290988DCBED703AF");

                entity.Property(e => e.IdBrand).HasColumnName("ID_Brand");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("PK__Category__6DB3A68AFF530EE9");

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment)
                    .HasName("PK__Comment__E19B6D4CA86395D1");

                entity.Property(e => e.IdComment).HasColumnName("ID_Comment");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FkOrder).HasColumnName("FK_Order");

                entity.Property(e => e.FkProduct).HasColumnName("FK_Product");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.FkOrderNavigation)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.FkOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__FK_Orde__37A5467C");

                entity.HasOne(d => d.FkProductNavigation)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.FkProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__FK_Prod__38996AB5");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("PK__Order__EC9FA955ABA980CD");

                entity.Property(e => e.IdOrder).HasColumnName("ID_Order");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.FkPaymentData).HasColumnName("FK_PaymentData");

                entity.Property(e => e.FkUser).HasColumnName("FK_User");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.FkPaymentDataNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.FkPaymentData)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__FK_Paymen__398D8EEE");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__FK_User__3A81B327");
            });

            modelBuilder.Entity<PaymentData>(entity =>
            {
                entity.HasKey(e => e.IdPaymentData)
                    .HasName("PK__PaymentD__BF0E9FF9428E790F");

                entity.Property(e => e.IdPaymentData).HasColumnName("ID_PaymentData");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.LastDigitCard)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("PK__Product__522DE496B32AF2D7");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FkBrand).HasColumnName("FK_Brand");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.FkBrandNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FkBrand)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__FK_Bran__3B75D760");
            });

            modelBuilder.Entity<ProductByCategory>(entity =>
            {
                entity.HasKey(e => e.IdProductByCategory)
                    .HasName("PK__ProductB__61EACB59220D5BB6");

                entity.Property(e => e.IdProductByCategory).HasColumnName("ID_ProductByCategory");

                entity.Property(e => e.FkCategory).HasColumnName("FK_Category");

                entity.Property(e => e.FkProduct).HasColumnName("FK_Product");

                entity.HasOne(d => d.FkCategoryNavigation)
                    .WithMany(p => p.ProductByCategory)
                    .HasForeignKey(d => d.FkCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductBy__FK_Ca__3C69FB99");

                entity.HasOne(d => d.FkProductNavigation)
                    .WithMany(p => p.ProductByCategory)
                    .HasForeignKey(d => d.FkProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductBy__FK_Pr__3D5E1FD2");
            });

            modelBuilder.Entity<ProductByOrder>(entity =>
            {
                entity.HasKey(e => e.IdProductByOrder)
                    .HasName("PK__ProductB__59012DA5DAF84A11");

                entity.Property(e => e.IdProductByOrder).HasColumnName("ID_ProductByOrder");

                entity.Property(e => e.FkOrder).HasColumnName("FK_Order");

                entity.Property(e => e.FkProduct).HasColumnName("FK_Product");

                entity.HasOne(d => d.FkOrderNavigation)
                    .WithMany(p => p.ProductByOrder)
                    .HasForeignKey(d => d.FkOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductBy__FK_Or__3E52440B");

                entity.HasOne(d => d.FkProductNavigation)
                    .WithMany(p => p.ProductByOrder)
                    .HasForeignKey(d => d.FkProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductBy__FK_Pr__3F466844");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__43DCD32DA10C46C4");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.IdToken)
                    .HasName("PK__Token__ECDC228E11BDC323");

                entity.Property(e => e.IdToken).HasColumnName("ID_Token");

                entity.Property(e => e.Expiration).HasColumnType("datetime");

                entity.Property(e => e.FkUser).HasColumnName("FK_User");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Token)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Token__FK_User__403A8C7D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__User__ED4DE4423963E3D8");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FkRole).HasColumnName("FK_Role");

                entity.Property(e => e.LastName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkRoleNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FkRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__FK_Role__412EB0B6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
