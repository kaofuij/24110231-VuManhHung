using System.Collections.Generic;
using System.Linq;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.DAL
{
    public class LoaiSanPhamDAL
    {
        public List<LoaiSanPham> GetAll()
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                // Đảm bảo dữ liệu mẫu đã được nạp nếu cơ sở dữ liệu mới khởi tạo
                QuanLySanPhamDatabaseInitializer.SeedData(context);
                return context.LoaiSanPhams.OrderBy(l => l.MaLoai).ToList();
            }
        }

        public LoaiSanPham GetById(string maLoai)
        {
            using (var context = new QuanLySanPhamDbContext())
            {
                return context.LoaiSanPhams.Find(maLoai);
            }
        }
    }
}
