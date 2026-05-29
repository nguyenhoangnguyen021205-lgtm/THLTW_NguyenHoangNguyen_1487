using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhạc cụ không được để trống")]
        [StringLength(100, ErrorMessage = "Tên nhạc cụ không quá 100 ký tự")]
        public string? Name { get; set; }

        // 🌟 FIX LỖI OVERFLOW: Chuyển đổi khoảng kiểm tra sang dạng double để không bị tràn Int32
        [Range(1000.0, 1000000000.0, ErrorMessage = "Giá bán phải nằm trong khoảng từ 1,000 đ đến 1 tỷ đ")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        public double Rating { get; set; } = 5.0;
        public int SalesCount { get; set; } = 0;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<ProductImage>? Images { get; set; }
    }
}