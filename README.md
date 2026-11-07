# BÀI KIỂM TRA LẬP TRÌNH NÂNG CAO C#
**Mã đề 02: Quản lý sản phẩm**

## 1. Thông tin sinh viên
- **Họ và tên**: Vũ Mạnh Hùng
- **Mã sinh viên**: 24110231
- **Lớp**: Lập trình nâng cao 

---

## 2. Giới thiệu bài làm
Bài làm được xây dựng bằng **C# WinForms (.NET Framework 4.8)** kết hợp **Entity Framework 6 Code First** và **SQL Server**, tổ chức theo chuẩn **mô hình 3 tầng (Three-Layer Architecture)**:

- **QuanLySanPham.GUI (Giao diện)**:
  - Thiết kế Form quản lý sản phẩm gồm các ô nhập thông tin, các nút chức năng (Mới, Nhập, Sửa, Xóa, Thoát) và bảng DataGridView hiển thị danh sách.
  - Phím `Enter` để Nhập, `Esc` để Thoát.
  - Xử lý xác nhận trước khi xóa và xác nhận thoát (không bị hỏi 2 lần).
- **QuanLySanPham.BUS (Nghiệp vụ)**:
  - Tiếp nhận dữ liệu từ GUI để kiểm tra tính hợp lệ trước khi gửi xuống DAL.
  - Bắt lỗi: không để trống mã/tên, ngày nhập không quá ngày hiện tại, số lượng và đơn giá phải lớn hơn 0.
  - Kiểm tra trùng mã sản phẩm khi thêm mới, giữ nguyên mã khi sửa.
- **QuanLySanPham.DAL (Dữ liệu)**:
  - `Entities`: Chứa 2 model `LoaiSanPham` (10 ký tự) và `SanPham` (20 ký tự, decimal 18,2, khóa ngoại tới LoaiSanPham).
  - `QuanLySanPhamDbContext`: Kế thừa `DbContext`, cấu hình quan hệ 1-N và nạp dữ liệu mẫu ban đầu (3 loại sản phẩm, 5 sản phẩm).
  - `LoaiSanPhamDAL`, `SanPhamDAL`: Thao tác truy vấn, thêm, sửa, xóa trực tiếp với cơ sở dữ liệu qua EF.

---

## 3. Cấu trúc thư mục
```text
24110231-VuManhHung/
├── QuanLySanPham.sln             # File mở solution bằng Visual Studio
├── README.md                     # File giới thiệu bài làm
├── QuanLySanPham.DAL/            # Tầng truy cập dữ liệu (DbContext, Entity, DAL)
├── QuanLySanPham.BUS/            # Tầng xử lý nghiệp vụ (Validation, BUS)
└── QuanLySanPham.GUI/            # Tầng giao diện WinForms (Form, App.config)
```

---