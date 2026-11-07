using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySanPham.DAL.Entities
{
    [Table("LoaiSanPham")]
    public class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        [StringLength(10)]
        public string MaLoai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; }

        // Navigation property: Một loại sản phẩm có nhiều sản phẩm (Quan hệ 1 - N)
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
