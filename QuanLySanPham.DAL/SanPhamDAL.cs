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
            using (var db = new QuanLySanPhamDbContext())
            {
                QuanLySanPhamDatabaseInitializer.SeedData(db);
                return db.SanPhams.Include(s => s.LoaiSanPham).ToList();
            }
        }

        public SanPham GetById(string ma)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                return db.SanPhams.Include(s => s.LoaiSanPham).FirstOrDefault(s => s.MaSP == ma);
            }
        }

        public bool CheckExists(string ma)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                return db.SanPhams.Any(s => s.MaSP.ToLower() == ma.ToLower());
            }
        }

        public void Add(SanPham sp)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
        }

        public void Update(SanPham sp)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                var item = db.SanPhams.Find(sp.MaSP);
                if (item != null)
                {
                    item.TenSP = sp.TenSP;
                    item.NgayNhap = sp.NgayNhap;
                    item.SoLuong = sp.SoLuong;
                    item.DonGia = sp.DonGia;
                    item.MaLoai = sp.MaLoai;
                    db.SaveChanges();
                }
            }
        }

        public void Delete(string ma)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                var item = db.SanPhams.Find(ma);
                if (item != null)
                {
                    db.SanPhams.Remove(item);
                    db.SaveChanges();
                }
            }
        }
    }
}
