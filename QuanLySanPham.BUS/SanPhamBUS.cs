using System;
using System.Collections.Generic;
using QuanLySanPham.DAL;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.BUS
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL _sanPhamDAL = new SanPhamDAL();

        public List<SanPham> GetAllSanPham()
        {
            return _sanPhamDAL.GetAll();
        }

        public SanPham GetSanPhamById(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                return null;

            return _sanPhamDAL.GetById(maSP.Trim());
        }

        /// <summary>
        /// Thêm mới sản phẩm có kiểm tra nghiệp vụ và kiểm tra trùng mã (Câu 4c)
        /// </summary>
        public void AddSanPham(SanPham sanPham)
        {
            // 1. Kiểm tra tính hợp lệ nghiệp vụ
            ValidateSanPham(sanPham);

            sanPham.MaSP = sanPham.MaSP.Trim();
            sanPham.TenSP = sanPham.TenSP.Trim();

            // 2. Kiểm tra trùng mã sản phẩm sau khi đã loại bỏ khoảng trắng đầu/cuối
            if (_sanPhamDAL.CheckExists(sanPham.MaSP))
            {
                throw new InvalidOperationException(string.Format("Mã sản phẩm '{0}' đã tồn tại trong hệ thống. Vui lòng chọn mã khác!", sanPham.MaSP));
            }

            _sanPhamDAL.Add(sanPham);
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm (giữ nguyên khóa chính theo Câu 4c)
        /// </summary>
        public void UpdateSanPham(SanPham sanPham)
        {
            // 1. Kiểm tra nghiệp vụ
            ValidateSanPham(sanPham);

            sanPham.MaSP = sanPham.MaSP.Trim();
            sanPham.TenSP = sanPham.TenSP.Trim();

            // 2. Kiểm tra sản phẩm có tồn tại để sửa không
            if (!_sanPhamDAL.CheckExists(sanPham.MaSP))
            {
                throw new InvalidOperationException(string.Format("Không tìm thấy sản phẩm có mã '{0}' để cập nhật!", sanPham.MaSP));
            }

            _sanPhamDAL.Update(sanPham);
        }

        /// <summary>
        /// Xóa sản phẩm theo mã sản phẩm
        /// </summary>
        public void DeleteSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
            {
                throw new ArgumentException("Mã sản phẩm không được để trống khi thực hiện xóa.");
            }

            string cleanMaSP = maSP.Trim();
            if (!_sanPhamDAL.CheckExists(cleanMaSP))
            {
                throw new InvalidOperationException(string.Format("Sản phẩm có mã '{0}' không tồn tại hoặc đã bị xóa trước đó.", cleanMaSP));
            }

            _sanPhamDAL.Delete(cleanMaSP);
        }

        /// <summary>
        /// Hàm kiểm tra toàn bộ quy tắc nghiệp vụ theo yêu cầu Câu 4c:
        /// - Mã và tên sản phẩm không trống;
        /// - Chọn loại sản phẩm;
        /// - Ngày nhập không lớn hơn ngày hiện tại;
        /// - Số lượng là số nguyên lớn hơn 0;
        /// - Đơn giá là số lớn hơn 0.
        /// </summary>
        private void ValidateSanPham(SanPham sanPham)
        {
            if (sanPham == null)
            {
                throw new ArgumentNullException("sanPham", "Dữ liệu sản phẩm không được rỗng (null).");
            }

            // Mã sản phẩm không trống
            if (string.IsNullOrWhiteSpace(sanPham.MaSP))
            {
                throw new ArgumentException("Mã sản phẩm không được để trống.");
            }

            if (sanPham.MaSP.Trim().Length > 20)
            {
                throw new ArgumentException("Mã sản phẩm không được vượt quá 20 ký tự.");
            }

            // Tên sản phẩm không trống
            if (string.IsNullOrWhiteSpace(sanPham.TenSP))
            {
                throw new ArgumentException("Tên sản phẩm không được để trống.");
            }

            if (sanPham.TenSP.Trim().Length > 150)
            {
                throw new ArgumentException("Tên sản phẩm không được vượt quá 150 ký tự.");
            }

            // Bắt buộc chọn loại sản phẩm
            if (string.IsNullOrWhiteSpace(sanPham.MaLoai))
            {
                throw new ArgumentException("Vui lòng chọn loại sản phẩm.");
            }

            // Ngày nhập không lớn hơn ngày hiện tại
            if (sanPham.NgayNhap.Date > DateTime.Today)
            {
                throw new ArgumentException("Ngày nhập không được lớn hơn ngày hiện tại.");
            }

            // Số lượng là số nguyên lớn hơn 0
            if (sanPham.SoLuong <= 0)
            {
                throw new ArgumentException("Số lượng sản phẩm phải là số nguyên lớn hơn 0.");
            }

            // Đơn giá là số lớn hơn 0
            if (sanPham.DonGia <= 0)
            {
                throw new ArgumentException("Đơn giá sản phẩm phải là số lớn hơn 0.");
            }
        }
    }
}
