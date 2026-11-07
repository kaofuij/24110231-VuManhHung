using System.Collections.Generic;
using System.Linq;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.DAL
{
    public class LoaiSanPhamDAL
    {
        public List<LoaiSanPham> GetAll()
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                QuanLySanPhamDatabaseInitializer.SeedData(db);
                return db.LoaiSanPhams.ToList();
            }
        }

        public LoaiSanPham GetById(string maLoai)
        {
            using (var db = new QuanLySanPhamDbContext())
            {
                return db.LoaiSanPhams.Find(maLoai);
            }
        }
    }
}
