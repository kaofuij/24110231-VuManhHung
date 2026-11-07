using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySanPham.DAL.Entities
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        [StringLength(20)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(150)]
        public string TenSP { get; set; }

        public DateTime NgayNhap { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal")]
        public decimal DonGia { get; set; }

        [Required]
        [StringLength(10)]
        public string MaLoai { get; set; }

        // khoa ngoai toi LoaiSanPham
        [ForeignKey("MaLoai")]
        public virtual LoaiSanPham LoaiSanPham { get; set; }
    }
}
