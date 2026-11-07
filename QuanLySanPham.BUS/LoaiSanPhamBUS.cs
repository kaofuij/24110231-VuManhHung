using System.Collections.Generic;
using QuanLySanPham.DAL;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.BUS
{
    public class LoaiSanPhamBUS
    {
        private LoaiSanPhamDAL dal = new LoaiSanPhamDAL();

        public List<LoaiSanPham> GetAll()
        {
            return dal.GetAll();
        }
    }
}
