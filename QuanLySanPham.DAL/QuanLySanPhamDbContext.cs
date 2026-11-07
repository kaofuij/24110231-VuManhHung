using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.DAL
{
    public class QuanLySanPhamDbContext : DbContext
    {
        public QuanLySanPhamDbContext() : base("name=QuanLySanPhamConnection")
        {
            Database.SetInitializer(new QuanLySanPhamDatabaseInitializer());
        }

        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // set kieu decimal 18,2 cho don gia
            modelBuilder.Entity<SanPham>()
                .Property(s => s.DonGia)
                .HasPrecision(18, 2);

            // quan he 1-n: 1 loai co nhieu san pham
            modelBuilder.Entity<LoaiSanPham>()
                .HasMany(l => l.SanPhams)
                .WithRequired(s => s.LoaiSanPham)
                .HasForeignKey(s => s.MaLoai)
                .WillCascadeOnDelete(false);
        }
    }

    public class QuanLySanPhamDatabaseInitializer : CreateDatabaseIfNotExists<QuanLySanPhamDbContext>
    {
        protected override void Seed(QuanLySanPhamDbContext context)
        {
            SeedData(context);
            base.Seed(context);
        }

        // them du lieu mau neu csdl chua co
        public static void SeedData(QuanLySanPhamDbContext db)
        {
            try
            {
                if (!db.LoaiSanPhams.Any())
                {
                    db.LoaiSanPhams.Add(new LoaiSanPham { MaLoai = "MT", TenLoai = "Máy tính" });
                    db.LoaiSanPhams.Add(new LoaiSanPham { MaLoai = "PK", TenLoai = "Phụ kiện" });
                    db.LoaiSanPhams.Add(new LoaiSanPham { MaLoai = "MH", TenLoai = "Màn hình" });
                    db.SaveChanges();
                }

                if (!db.SanPhams.Any())
                {
                    db.SanPhams.Add(new SanPham { MaSP = "SP001", TenSP = "Laptop Dell Inspiron 15", NgayNhap = new DateTime(2026, 7, 1), SoLuong = 10, DonGia = 15500000m, MaLoai = "MT" });
                    db.SanPhams.Add(new SanPham { MaSP = "SP002", TenSP = "Chuột không dây Logitech M331", NgayNhap = new DateTime(2026, 6, 15), SoLuong = 25, DonGia = 450000m, MaLoai = "PK" });
                    db.SanPhams.Add(new SanPham { MaSP = "SP003", TenSP = "Bàn phím cơ Razer BlackWidow", NgayNhap = new DateTime(2026, 6, 20), SoLuong = 15, DonGia = 1900000m, MaLoai = "PK" });
                    db.SanPhams.Add(new SanPham { MaSP = "SP004", TenSP = "Màn hình LG 24MP400 24\"", NgayNhap = new DateTime(2026, 7, 5), SoLuong = 12, DonGia = 2650000m, MaLoai = "MH" });
                    db.SanPhams.Add(new SanPham { MaSP = "SP005", TenSP = "Tai nghe Sony WH-CH510", NgayNhap = new DateTime(2026, 7, 10), SoLuong = 20, DonGia = 1150000m, MaLoai = "PK" });
                    db.SaveChanges();
                }
            }
            catch
            {
                // bo qua neu du lieu da duoc them
            }
        }
    }
}
