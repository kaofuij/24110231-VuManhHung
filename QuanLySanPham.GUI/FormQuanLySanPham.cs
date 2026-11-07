using System;
using System.Linq;
using System.Windows.Forms;
using QuanLySanPham.BUS;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.GUI
{
    public partial class FormQuanLySanPham : Form
    {
        private readonly LoaiSanPhamBUS _loaiSanPhamBUS = new LoaiSanPhamBUS();
        private readonly SanPhamBUS _sanPhamBUS = new SanPhamBUS();

        public FormQuanLySanPham()
        {
            InitializeComponent();

            // Yêu cầu Câu 1b:
            // Phím Enter thực hiện nút Nhập; Phím Esc thực hiện nút Thoát
            this.AcceptButton = this.btnNhap;
            this.CancelButton = this.btnThoat;
        }

        private void FormQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                // Câu 4a: Nạp danh sách loại sản phẩm vào cboLoaiSP
                LoadLoaiSanPham();

                // Câu 4a: Nạp danh sách sản phẩm vào dgvSanPham
                LoadData();

                // Câu 1c: Thiết lập trạng thái ban đầu của form
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nạp danh sách loại sản phẩm vào ComboBox
        /// </summary>
        private void LoadLoaiSanPham()
        {
            var listLoai = _loaiSanPhamBUS.GetAllLoaiSanPham();
            cboLoaiSP.DataSource = listLoai;
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        /// <summary>
        /// Nạp danh sách sản phẩm vào DataGridView và hiển thị tên loại sản phẩm (Câu 4a)
        /// </summary>
        private void LoadData()
        {
            var listSanPham = _sanPhamBUS.GetAllSanPham()
                .Select(s => new
                {
                    MaSP = s.MaSP,
                    TenSP = s.TenSP,
                    NgayNhap = s.NgayNhap,
                    SoLuong = s.SoLuong,
                    DonGia = s.DonGia,
                    TenLoai = s.LoaiSanPham != null ? s.LoaiSanPham.TenLoai : "",
                    MaLoai = s.MaLoai
                }).ToList();

            dgvSanPham.DataSource = listSanPham;
        }

        /// <summary>
        /// Đặt lại trạng thái Form theo đúng yêu cầu Câu 1c:
        /// - Làm sạch dữ liệu nhập
        /// - Bỏ chọn dòng hiện tại
        /// - Đưa con trỏ về txtMaSP
        /// - Bật nút Nhập, tắt nút Sửa và Xóa
        /// - ComboBox ở trạng thái chưa chọn
        /// </summary>
        private void ResetFormState()
        {
            // Làm sạch các ô nhập liệu
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();

            // Đặt ngày nhập về ngày hiện tại
            dtpNgayNhap.Value = DateTime.Today;

            // ComboBox ở trạng thái chưa chọn (-1)
            cboLoaiSP.SelectedIndex = -1;

            // Bỏ chọn dòng trên DataGridView
            dgvSanPham.ClearSelection();

            // Cho phép nhập mã sản phẩm
            txtMaSP.ReadOnly = false;

            // Bật nút Nhập, tắt nút Sửa và Xóa
            btnNhap.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Đưa con trỏ nhập liệu về ô txtMaSP
            txtMaSP.Focus();
        }

        /// <summary>
        /// Khi chọn dòng trên DataGridView (Câu 1c & Câu 4a):
        /// - Tắt nút Nhập, bật nút Sửa/Xóa và không cho sửa mã
        /// - Điền đúng toàn bộ thông tin và chọn ComboBox theo khóa ngoại
        /// </summary>
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count)
                return;

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            txtMaSP.Text = row.Cells["colMaSP"].Value?.ToString();
            txtTenSP.Text = row.Cells["colTenSP"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["colNgayNhap"].Value?.ToString(), out DateTime ngayNhap))
            {
                dtpNgayNhap.Value = ngayNhap;
            }

            txtSoLuong.Text = row.Cells["colSoLuong"].Value?.ToString();
            txtDonGia.Text = row.Cells["colDonGia"].Value?.ToString();

            // Chọn ComboBox theo khóa ngoại MaLoai
            if (row.Cells["colMaLoai"].Value != null)
            {
                cboLoaiSP.SelectedValue = row.Cells["colMaLoai"].Value.ToString();
            }

            // Trạng thái theo Câu 1c: tắt Nhập, bật Sửa/Xóa, khóa ô mã sản phẩm
            txtMaSP.ReadOnly = true;
            btnNhap.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        /// <summary>
        /// Nút Mới (Câu 1c)
        /// </summary>
        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetFormState();
        }

        /// <summary>
        /// Lấy đối tượng SanPham từ giao diện và kiểm tra tính hợp lệ cơ bản tại GUI
        /// </summary>
        private SanPham GetSanPhamFromInput()
        {
            string maSP = txtMaSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSP))
            {
                txtMaSP.Focus();
                throw new ArgumentException("Mã sản phẩm không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(tenSP))
            {
                txtTenSP.Focus();
                throw new ArgumentException("Tên sản phẩm không được để trống.");
            }

            if (cboLoaiSP.SelectedValue == null || cboLoaiSP.SelectedIndex == -1)
            {
                cboLoaiSP.Focus();
                throw new ArgumentException("Vui lòng chọn loại sản phẩm.");
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                txtSoLuong.Focus();
                throw new ArgumentException("Số lượng sản phẩm phải là số nguyên lớn hơn 0.");
            }

            // Loại bỏ dấu phân cách hàng nghìn nếu có khi parse đơn giá
            string donGiaStr = txtDonGia.Text.Trim().Replace(",", "").Replace(".", "");
            if (!decimal.TryParse(donGiaStr, out decimal donGia) || donGia <= 0)
            {
                txtDonGia.Focus();
                throw new ArgumentException("Đơn giá sản phẩm phải là số lớn hơn 0.");
            }

            return new SanPham
            {
                MaSP = maSP,
                TenSP = tenSP,
                NgayNhap = dtpNgayNhap.Value,
                SoLuong = soLuong,
                DonGia = donGia,
                MaLoai = cboLoaiSP.SelectedValue.ToString()
            };
        }

        /// <summary>
        /// Nút Nhập (Thêm sản phẩm mới - Câu 4b, 4c)
        /// </summary>
        private void btnNhap_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham sanPham = GetSanPhamFromInput();

                // Gửi qua tầng BUS để kiểm tra quy tắc nghiệp vụ và lưu xuống DAL
                _sanPhamBUS.AddSanPham(sanPham);

                MessageBox.Show("Thêm sản phẩm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Nút Sửa (Cập nhật thông tin - Câu 4b, 4c)
        /// </summary>
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham sanPham = GetSanPhamFromInput();

                // Gửi qua tầng BUS để cập nhật
                _sanPhamBUS.UpdateSanPham(sanPham);

                MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Nút Xóa (Câu 4b, 4d): Có xác nhận trước khi xóa
        /// </summary>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa trong danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Câu 4d: Hộp thoại xác nhận trước khi xóa
            DialogResult dr = MessageBox.Show(
                string.Format("Bạn có chắc chắn muốn xóa sản phẩm có mã '{0}' không?", maSP),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    _sanPhamBUS.DeleteSanPham(maSP);
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetFormState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi xóa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Nút Thoát: Gọi this.Close() để sự kiện FormClosing xử lý duy nhất 1 lần xác nhận (Câu 4d)
        /// </summary>
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Xử lý xác nhận khi đóng Form (Câu 4d):
        /// - Áp dụng thống nhất cho nút Thoát, phím Esc (CancelButton), hoặc nút X góc trên bên phải.
        /// - Tránh hiển thị hai lần hộp thoại đóng Form theo đúng yêu cầu Câu 4d.
        /// </summary>
        private void FormQuanLySanPham_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No)
            {
                e.Cancel = true; // Hủy thao tác đóng form
            }
        }
    }
}
