using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebsiteBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(50, ErrorMessage = "Tên danh mục không quá 50 ký tự")]
        public string? Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}