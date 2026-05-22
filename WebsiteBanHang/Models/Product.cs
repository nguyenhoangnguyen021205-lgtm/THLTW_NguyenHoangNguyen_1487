using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm âm nhạc không được để trống")]
        [StringLength(150, ErrorMessage = "Tên sản phẩm không được vượt quá 150 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(1000, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 1,000đ")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục phù hợp")]
        public int CategoryId { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả ngắn không quá 500 ký tự")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsFeatured { get; set; }

        // --- CÁC THUỘC TÍNH NÂNG CẤP MỚI ---
        public int Stock { get; set; } = 10;          // Số lượng tồn kho (Mặc định là 10)
        public double Rating { get; set; } = 5.0;      // Điểm đánh giá sao (Mặc định 5.0)
        public int SalesCount { get; set; } = 0;       // Số lượng đã bán (Mặc định 0)
    }
}