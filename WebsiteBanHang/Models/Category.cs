using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục nhạc cụ không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không quá 100 ký tự")]
        public string Name { get; set; } = string.Empty; // Gán mặc định rỗng

        public string? IconClass { get; set; } // Thêm ? cho phép null
    }
}