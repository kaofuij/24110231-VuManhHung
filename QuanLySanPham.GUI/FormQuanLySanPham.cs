using System;
using System.Linq;
using System.Windows.Forms;
using QuanLySanPham.BUS;
using QuanLySanPham.DAL.Entities;

namespace QuanLySanPham.GUI
{
    public partial class FormQuanLySanPham : Form
    {
        private LoaiSanPhamBUS loaiBUS = new LoaiSanPhamBUS();
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();

        public FormQuanLySanPham()
        {
            InitializeComponent();

            // phim enter la nut nhap, esc la nut thoat
            this.AcceptButton = this.btnNhap;
            this.CancelButton = this.btnThoat;
        }

        private void FormQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                LoadLoai();
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // load combobox loai san pham
        private void LoadLoai()
        {
            var list = loaiBUS.GetAll();
            cboLoaiSP.DataSource = list;
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        // load du lieu len gridview
        private void LoadData()
        {
            var list = sanPhamBUS.GetAll()
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

            dgvSanPham.DataSource = list;
        }

        // xoa trang form ve mac dinh
        private void ResetForm()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();

            dtpNgayNhap.Value = DateTime.Today;
            cboLoaiSP.SelectedIndex = -1;
            dgvSanPham.ClearSelection();

            txtMaSP.ReadOnly = false;
            btnNhap.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            txtMaSP.Focus();
        }

        // click chon 1 dong tren gridview
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count)
                return;

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            txtMaSP.Text = row.Cells["colMaSP"].Value?.ToString();
            txtTenSP.Text = row.Cells["colTenSP"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["colNgayNhap"].Value?.ToString(), out DateTime ngay))
            {
                dtpNgayNhap.Value = ngay;
            }

            txtSoLuong.Text = row.Cells["colSoLuong"].Value?.ToString();
            txtDonGia.Text = row.Cells["colDonGia"].Value?.ToString();

            if (row.Cells["colMaLoai"].Value != null)
            {
                cboLoaiSP.SelectedValue = row.Cells["colMaLoai"].Value.ToString();
            }

            // khong cho sua ma khi dang chon dong
            txtMaSP.ReadOnly = true;
            btnNhap.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        // nut moi
        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // lay du lieu tu form
        private SanPham GetFormData()
        {
            string ma = txtMaSP.Text.Trim();
            string ten = txtTenSP.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                txtMaSP.Focus();
                throw new Exception("Mã sản phẩm không được để trống!");
            }

            if (string.IsNullOrEmpty(ten))
            {
                txtTenSP.Focus();
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            if (cboLoaiSP.SelectedIndex == -1 || cboLoaiSP.SelectedValue == null)
            {
                cboLoaiSP.Focus();
                throw new Exception("Vui lòng chọn loại sản phẩm!");
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0)
            {
                txtSoLuong.Focus();
                throw new Exception("Số lượng phải là số nguyên > 0!");
            }

            string giaStr = txtDonGia.Text.Trim().Replace(",", "").Replace(".", "");
            if (!decimal.TryParse(giaStr, out decimal gia) || gia <= 0)
            {
                txtDonGia.Focus();
                throw new Exception("Đơn giá phải là số > 0!");
            }

            return new SanPham
            {
                MaSP = ma,
                TenSP = ten,
                NgayNhap = dtpNgayNhap.Value,
                SoLuong = sl,
                DonGia = gia,
                MaLoai = cboLoaiSP.SelectedValue.ToString()
            };
        }

        // nut nhap / them
        private void btnNhap_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham sp = GetFormData();
                sanPhamBUS.Add(sp);

                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // nut sua
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham sp = GetFormData();
                sanPhamBUS.Update(sp);

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // nut xoa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm " + ma + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    sanPhamBUS.Delete(ma);
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // nut thoat
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // xac nhan thoat form (ap dung cho ca nut thoat, phim esc va nut X)
        private void FormQuanLySanPham_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
