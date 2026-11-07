using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.DAL
{
    public class SanPhamDAL
    {
        public List<SanPham> GetAll()
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                // Đảm bảo dữ liệu mẫu đã được nạp nếu cơ sở dữ liệu mới khởi tạo
                QuanLySanPhamDatabaseInitializer.SeedData(context);

                // Dùng Include để nạp thuộc tính điều hướng LoaiSanPham (Eager Loading)
                return context.SanPhams
                    .Include(s => s.LoaiSanPham)
                    .OrderBy(s => s.MaSP)
                    .ToList();
            }
        }

        public SanPham GetById(string maSP)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                return context.SanPhams
                    .Include(s => s.LoaiSanPham)
                    .FirstOrDefault(s => s.MaSP == maSP);
            }
        }

        public bool CheckExists(string maSP)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                return context.SanPhams.Any(s => s.MaSP.ToLower() == maSP.ToLower());
            }
        }

        public void Add(SanPham sanPham)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                context.SanPhams.Add(sanPham);
                context.SaveChanges();
            }
        }

        public void Update(SanPham sanPham)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                var existing = context.SanPhams.Find(sanPham.MaSP);
                if (existing == null)
                {
                    throw new Exception(string.Format("Không tìm thấy sản phẩm có mã '{0}' để cập nhật.", sanPham.MaSP));
                }

                existing.TenSP = sanPham.TenSP;
                existing.NgayNhap = sanPham.NgayNhap;
                existing.SoLuong = sanPham.SoLuong;
                existing.DonGia = sanPham.DonGia;
                existing.MaLoai = sanPham.MaLoai;

                context.SaveChanges();
            }
        }

        public void Delete(string maSP)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                var existing = context.SanPhams.Find(maSP);
                if (existing == null)
                {
                    throw new Exception(string.Format("Không tìm thấy sản phẩm có mã '{0}' để xóa.", maSP));
                }

                context.SanPhams.Remove(existing);
                context.SaveChanges();
            }
        }
    }
}
