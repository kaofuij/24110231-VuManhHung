using System;
using System.Collections.Generic;
using QuanLySanPham.DAL;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public List<SanPham> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(SanPham sp)
        {
            Validate(sp);

            sp.MaSP = sp.MaSP.Trim();
            sp.TenSP = sp.TenSP.Trim();

            // kiem tra trung ma
            if (dal.CheckExists(sp.MaSP))
            {
                throw new Exception("Mã sản phẩm đã tồn tại!");
            }

            dal.Add(sp);
        }

        public void Update(SanPham sp)
        {
            Validate(sp);

            sp.MaSP = sp.MaSP.Trim();
            sp.TenSP = sp.TenSP.Trim();

            if (!dal.CheckExists(sp.MaSP))
            {
                throw new Exception("Không tìm thấy sản phẩm để sửa!");
            }

            dal.Update(sp);
        }

        public void Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
            {
                throw new Exception("Vui lòng chọn sản phẩm cần xóa!");
            }

            ma = ma.Trim();
            if (!dal.CheckExists(ma))
            {
                throw new Exception("Sản phẩm không tồn tại hoặc đã bị xóa!");
            }

            dal.Delete(ma);
        }

        private void Validate(SanPham sp)
        {
            if (sp == null)
            {
                throw new Exception("Dữ liệu không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(sp.MaSP))
            {
                throw new Exception("Mã sản phẩm không được để trống!");
            }

            if (sp.MaSP.Trim().Length > 20)
            {
                throw new Exception("Mã sản phẩm không quá 20 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(sp.TenSP))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            if (sp.TenSP.Trim().Length > 150)
            {
                throw new Exception("Tên sản phẩm không quá 150 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(sp.MaLoai))
            {
                throw new Exception("Vui lòng chọn loại sản phẩm!");
            }

            if (sp.NgayNhap.Date > DateTime.Today)
            {
                throw new Exception("Ngày nhập không được lớn hơn ngày hiện tại!");
            }

            if (sp.SoLuong <= 0)
            {
                throw new Exception("Số lượng phải là số nguyên lớn hơn 0!");
            }

            if (sp.DonGia <= 0)
            {
                throw new Exception("Đơn giá phải lớn hơn 0!");
            }
        }
    }
}
