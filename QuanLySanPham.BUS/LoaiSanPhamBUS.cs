using System.Collections.Generic;
using QuanLySanPham.DAL;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.BUS
{
    public class LoaiSanPhamBUS
    {
        private readonly LoaiSanPhamDAL _loaiSanPhamDAL = new LoaiSanPhamDAL();

        public List<LoaiSanPham> GetAllLoaiSanPham()
        {
            return _loaiSanPhamDAL.GetAll();
        }

        public LoaiSanPham GetLoaiSanPhamById(string maLoai)
        {
            if (string.IsNullOrWhiteSpace(maLoai))
                return null;

            return _loaiSanPhamDAL.GetById(maLoai.Trim());
        }
    }
}
