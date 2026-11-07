using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QuanLySanPham.DAL.Entities;
using QuanLySanPham.DAL.Migrations;

namespace QuanLySanPham.DAL
{
    public class QuanLySanPhamDbContext : DbContext
    {
        // Đọc chuỗi kết nối "QuanLySanPhamConnection" từ file App.config
        public QuanLySanPhamDbContext() : base("name=QuanLySanPhamConnection")
        {
            // Tự động Migration cập nhật CSDL và nạp dữ liệu mẫu
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<QuanLySanPhamDbContext, Configuration>());
        }

        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình độ chính xác decimal(18,2) cho trường DonGia
            modelBuilder.Entity<SanPham>()
                .Property(s => s.DonGia)
                .HasPrecision(18, 2);

            // Cấu hình mối quan hệ 1 - N: một LoaiSanPham có nhiều SanPham
            modelBuilder.Entity<LoaiSanPham>()
                .HasMany(l => l.SanPhams)
                .WithRequired(s => s.LoaiSanPham)
                .HasForeignKey(s => s.MaLoai)
                .WillCascadeOnDelete(false);
        }
    }

    /// <summary>
    /// Class hỗ trợ nạp sẵn dữ liệu mẫu đúng đề bài
    /// </summary>
    public static class QuanLySanPhamDatabaseInitializer
    {
        public static void SeedData(QuanLySanPhamDbContext context)
        {
            // 1. Khởi tạo danh mục Loại Sản Phẩm (theo dữ liệu đề bài)
            var defaultLoais = new List<LoaiSanPham>
            {
                new LoaiSanPham { MaLoai = "MT", TenLoai = "Máy tính" },
                new LoaiSanPham { MaLoai = "PK", TenLoai = "Phụ kiện" },
                new LoaiSanPham { MaLoai = "MH", TenLoai = "Màn hình" }
            };

            foreach (var loai in defaultLoais)
            {
                if (!context.LoaiSanPhams.Any(l => l.MaLoai == loai.MaLoai))
                {
                    context.LoaiSanPhams.Add(loai);
                }
            }
            context.SaveChanges();

            // 2. Khởi tạo 5 Sản Phẩm mẫu đúng như bảng giao diện trong đề bài
            var defaultSanPhams = new List<SanPham>
            {
                new SanPham
                {
                    MaSP = "SP001",
                    TenSP = "Laptop Dell Inspiron 15",
                    NgayNhap = new DateTime(2026, 7, 1),
                    SoLuong = 10,
                    DonGia = 15500000m,
                    MaLoai = "MT"
                },
                new SanPham
                {
                    MaSP = "SP002",
                    TenSP = "Chuột không dây Logitech M331",
                    NgayNhap = new DateTime(2026, 6, 15),
                    SoLuong = 25,
                    DonGia = 450000m,
                    MaLoai = "PK"
                },
                new SanPham
                {
                    MaSP = "SP003",
                    TenSP = "Bàn phím cơ Razer BlackWidow",
                    NgayNhap = new DateTime(2026, 6, 20),
                    SoLuong = 15,
                    DonGia = 1900000m,
                    MaLoai = "PK"
                },
                new SanPham
                {
                    MaSP = "SP004",
                    TenSP = "Màn hình LG 24MP400 24\"",
                    NgayNhap = new DateTime(2026, 7, 5),
                    SoLuong = 12,
                    DonGia = 2650000m,
                    MaLoai = "MH"
                },
                new SanPham
                {
                    MaSP = "SP005",
                    TenSP = "Tai nghe Sony WH-CH510",
                    NgayNhap = new DateTime(2026, 7, 10),
                    SoLuong = 20,
                    DonGia = 1150000m,
                    MaLoai = "PK"
                }
            };

            foreach (var sp in defaultSanPhams)
            {
                if (!context.SanPhams.Any(s => s.MaSP == sp.MaSP))
                {
                    context.SanPhams.Add(sp);
                }
            }
            context.SaveChanges();
        }
    }
}
