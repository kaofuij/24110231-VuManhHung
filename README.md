# BÁO CÁO BÀI KIỂM TRA - MÔN: LẬP TRÌNH NÂNG CAO C#
## ĐỀ THI: MÃ ĐỀ 02 - QUẢN LÝ SẢN PHẨM (TỔNG ĐIỂM: 10)
- **Sinh viên thực hiện**: Vũ Mạnh Hùng
- **Mã sinh viên**: 24110231
- **Công nghệ**: C# WinForms (.NET Framework 4.8), Entity Framework 6.4.4 Code First, SQL Server / LocalDB
- **Mô hình kiến trúc**: Mô hình 3 tầng chuẩn (Three-Layer Architecture: GUI - BUS - DAL)

---

## 1. CẤU TRÚC DỰ ÁN THEO MÔ HÌNH 3 TẦNG (THREE-LAYER)
Dự án được phân chia độc lập thành 3 project theo đúng chuẩn thiết kế phần mềm doanh nghiệp:

```text
24110231-VuManhHung/
├── QuanLySanPham.sln                     # File Solution quản lý 3 project
├── README.md                             # Tài liệu hướng dẫn & giải thích vấn đáp
├── QuanLySanPham.DAL/                    # TẦNG TRUY CẬP DỮ LIỆU (Data Access Layer)
│   ├── Entities/
│   │   ├── LoaiSanPham.cs                # Thực thể Loại sản phẩm (Khóa chính MaLoai, quan hệ 1-N)
│   │   └── SanPham.cs                    # Thực thể Sản phẩm (Khóa chính MaSP, Khóa ngoại MaLoai)
│   ├── Migrations/
│   │   ├── Configuration.cs              # Cấu hình EF Code First Migrations
│   │   └── 202611070000000_InitialCreate.cs # Migration tạo bảng LoaiSanPham và SanPham
│   ├── QuanLySanPhamDbContext.cs         # DbContext kết nối CSDL & cấu hình Fluent API, Seed Data
│   ├── LoaiSanPhamDAL.cs                 # Truy vấn dữ liệu Loại sản phẩm
│   ├── SanPhamDAL.cs                     # Thêm, Sửa, Xóa, Truy vấn dữ liệu Sản phẩm bằng EF
│   └── App.config                        # Chuỗi kết nối CSDL và cấu hình EntityFramework
├── QuanLySanPham.BUS/                    # TẦNG NGHIỆP VỤ (Business Logic Layer)
│   ├── LoaiSanPhamBUS.cs                 # Xử lý nghiệp vụ cho Loại sản phẩm
│   └── SanPhamBUS.cs                     # Kiểm tra ràng buộc dữ liệu, trùng mã, nghiệp vụ thêm/sửa/xóa
└── QuanLySanPham.GUI/                    # TẦNG GIAO DIỆN NGƯỜI DÙNG (Presentation Layer)
    ├── FormQuanLySanPham.cs              # Xử lý sự kiện giao diện (Event Handlers)
    ├── FormQuanLySanPham.Designer.cs     # Thiết kế giao diện Windows Forms trực quan
    ├── Program.cs                        # Điểm khởi chạy ứng dụng
    └── App.config                        # File cấu hình chuỗi kết nối của ứng dụng chạy
```

---

## 2. HƯỚNG DẪN CẤU HÌNH KẾT NỐI VÀ CHẠY ỨNG DỤNG

### Bước 1: Mở Solution
- Mở thư mục `d:\cocde\24110231-VuManhHung`.
- Click đúp vào file **`QuanLySanPham.sln`** để mở dự án trong Visual Studio.

### Bước 2: Cấu hình chuỗi kết nối trong `App.config` (Tùy máy kiểm tra của cô)
File `App.config` nằm tại `QuanLySanPham.GUI/App.config` (và `QuanLySanPham.DAL/App.config`):
```xml
<connectionStrings>
    <!-- Mặc định dùng SQL Server LocalDB có sẵn khi cài Visual Studio -->
    <add name="QuanLySanPhamConnection"
         connectionString="Data Source=(localdb)\mssqllocaldb;Initial Catalog=QuanLySanPhamDB;Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```
- **Nếu máy dùng SQL Server mặc định**: Sửa `Data Source=.` hoặc `Data Source=localhost`
- **Nếu máy dùng SQL Server Express**: Sửa `Data Source=.\SQLEXPRESS`
- **Nếu máy dùng LocalDB**: Giữ nguyên `Data Source=(localdb)\mssqllocaldb`

### Bước 3: Chạy ứng dụng và cơ chế tự động tạo CSDL (Code First)
- Nhấn phím **F5** hoặc bấm nút **Start** trong Visual Studio.
- Cơ chế `QuanLySanPhamDatabaseInitializer` kế thừa `CreateDatabaseIfNotExists<QuanLySanPhamDbContext>` sẽ:
  1. Tự động kiểm tra xem database `QuanLySanPhamDB` đã có trên máy chưa. Nếu chưa có, EF sẽ **tự động tạo toàn bộ bảng** với đầy đủ khóa chính, khóa ngoại, kiểu dữ liệu.
  2. Tự động nạp sẵn **3 danh mục Loại sản phẩm** (`MT - Máy tính`, `PK - Phụ kiện`, `MH - Màn hình`).
  3. Tự động nạp sẵn **5 Sản phẩm mẫu** đúng chuẩn như trên hình đề bài mà không bị trùng lặp.
- Dữ liệu thêm, sửa, xóa được lưu vĩnh viễn trong CSDL, khi tắt ứng dụng bật lại dữ liệu vẫn được bảo toàn.

---

## 3. CHI TIẾT BẢNG ĐIỂM VÀ CÁC YÊU CẦU ĐÃ HOÀN THÀNH 100%

### Câu 1: Thiết kế giao diện (2 điểm)
- **a) (1,0 điểm)**: 
  - Giao diện có đầy đủ các control đúng như hình tham khảo: GroupBox *Thông tin sản phẩm*, ComboBox *Loại sản phẩm*, TextBox *Mã SP*, *Tên SP*, *Số lượng*, *Đơn giá*, DateTimePicker *Ngày nhập*.
  - Nhóm 5 nút chức năng: *Mới*, *Nhập*, *Sửa*, *Xóa*, *Thoát*.
  - Bảng DataGridView đầy đủ 6 cột hiển thị: `Mã SP`, `Tên sản phẩm`, `Ngày nhập`, `Số lượng`, `Đơn giá`, `Loại sản phẩm`.
  - Form hiển thị chính giữa màn hình: `StartPosition = FormStartPosition.CenterScreen`.
  - Tiêu đề Form đúng chuẩn: `"Quản lý sản phẩm - 24110231 - Vũ Mạnh Hùng"`.
- **b) (0,5 điểm)**:
  - Phím **Enter** kích hoạt nút **Nhập** (`this.AcceptButton = this.btnNhap`).
  - Phím **Esc** kích hoạt nút **Thoát** (`this.CancelButton = this.btnThoat`).
  - Thứ tự phím Tab hợp lý tuần tự qua các ô nhập liệu, các nút và bảng dữ liệu.
- **c) (0,5 điểm)**:
  - Khi Form Load hoặc nhấn nút **Mới**: Làm sạch các ô nhập liệu, đưa ngày nhập về ngày hiện tại, bỏ chọn trên DataGridView, ComboBox ở trạng thái chưa chọn (`SelectedIndex = -1`), mở khóa ô nhập mã (`ReadOnly = false`), bật nút Nhập (`Enabled = true`), tắt nút Sửa và Xóa (`Enabled = false`), con trỏ focus vào `txtMaSP`.
  - Khi chọn một dòng hợp lệ trên DataGridView: Đổ dữ liệu lên các điều khiển, chọn đúng ComboBox theo khóa ngoại, khóa không cho sửa mã (`txtMaSP.ReadOnly = true`), tắt nút Nhập (`Enabled = false`), bật nút Sửa và Xóa (`Enabled = true`).

### Câu 2: Entity Framework Code First (3 điểm)
- **a) (1,5 điểm)**:
  - Xây dựng 2 Entity: `LoaiSanPham` và `SanPham`.
  - Sử dụng Data Annotations:
    - Khóa chính: `[Key]` ở `MaLoai` (string 10) và `MaSP` (string 20).
    - Thuộc tính bắt buộc: `[Required]`.
    - Độ dài chuỗi: `[StringLength(10)]`, `[StringLength(100)]`, `[StringLength(20)]`, `[StringLength(150)]`.
    - Kiểu dữ liệu decimal: `[Column(TypeName = "decimal")]` kết hợp Fluent API `HasPrecision(18, 2)` cho thuộc tính `DonGia`.
    - Khóa ngoại: `[ForeignKey("MaLoai")]`.
    - Navigation Properties ở cả 2 entity: `public virtual ICollection<SanPham> SanPhams { get; set; }` và `public virtual LoaiSanPham LoaiSanPham { get; set; }`.
- **b) (1,5 điểm)**:
  - Lớp `QuanLySanPhamDbContext` kế thừa `DbContext`.
  - Chuỗi kết nối đọc từ `App.config` thông qua `base("name=QuanLySanPhamConnection")`.
  - Có sẵn cấu hình Migrations trong thư mục `Migrations/`.

### Câu 3: Tổ chức mô hình Three-Layer (2 điểm)
- **a) (0,75 điểm)**:
  - Phân chia đúng 3 project với trách nhiệm rõ ràng:
    - `GUI`: Chỉ tương tác giao diện và hiển thị thông báo. Tuyệt đối không gọi `DbContext`, `DbSet` hay `SaveChanges`.
    - `BUS`: Tiếp nhận yêu cầu từ GUI, kiểm tra toàn bộ quy tắc nghiệp vụ hợp lệ và mã trùng trước khi gọi DAL.
    - `DAL`: Truy cập CSDL bằng Entity Framework. Tuyệt đối không tham chiếu thư viện WinForms.
- **b) (0,75 điểm)**:
  - Luồng đi đúng chuẩn 1 chiều: `GUI -> BUS -> DAL -> CSDL`.
  - Tầng DAL hoàn toàn độc lập với GUI.
  - Sử dụng trực tiếp Entity (`SanPham`, `LoaiSanPham`) để truyền tải dữ liệu giữa các tầng.
- **c) (0,5 điểm)**:
  - Xử lý ngoại lệ chặt chẽ với `try-catch`. Khi lưu thất bại hoặc vi phạm ràng buộc dữ liệu, ngoại lệ từ tầng BUS/DAL được bắt và hiển thị cảnh báo lên MessageBox; tuyệt đối không báo thành công khi có lỗi.

### Câu 4: Chức năng và kiểm tra dữ liệu (3 điểm)
- **a) (0,5 điểm)**:
  - Form Load nạp danh sách loại sản phẩm vào `cboLoaiSP`.
  - Nạp danh sách sản phẩm vào `dgvSanPham` kèm tên loại sản phẩm (sử dụng `.Include(s => s.LoaiSanPham)` ở DAL).
  - Khi click vào 1 dòng, đổ toàn bộ thông tin lên Form và chọn ComboBox tương ứng với mã loại của dòng đó.
- **b) (1,0 điểm)**:
  - Thêm, sửa, xóa sản phẩm đầy đủ thông qua BUS và DAL bằng EF. Sau khi thực hiện xong tự động load lại DataGridView và làm mới form.
- **c) (1,0 điểm)**:
  - Toàn bộ validation được đặt tại `SanPhamBUS.cs`:
    1. Mã sản phẩm không rỗng (sau khi `Trim()`).
    2. Tên sản phẩm không rỗng (sau khi `Trim()`).
    3. Phải chọn loại sản phẩm trong ComboBox.
    4. Ngày nhập không lớn hơn ngày hiện tại (`NgayNhap.Date <= DateTime.Today`).
    5. Số lượng là số nguyên lớn hơn 0 (`SoLuong > 0`).
    6. Đơn giá là số lớn hơn 0 (`DonGia > 0`).
    7. Khi thêm: Kiểm tra không trùng mã sau khi đã `Trim()` khoảng trắng.
    8. Khi sửa: Giữ nguyên khóa chính (`MaSP`).
- **d) (0,5 điểm)**:
  - Trước khi xóa có hộp thoại xác nhận `Yes/No`. Chọn `No` sẽ hủy thao tác.
  - Xác nhận thoát được xử lý tập trung trong sự kiện `Form_FormClosing`. Bất kể người dùng bấm nút **Thoát**, nhấn phím **Esc**, hay bấm nút **[X]** góc phải cửa sổ, chương trình đều hiển thị hộp thoại xác nhận và **không bao giờ bị hỏi 2 lần**.
