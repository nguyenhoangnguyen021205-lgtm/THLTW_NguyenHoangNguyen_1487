namespace WebsiteBanHang.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } // Đường dẫn ảnh phụ bổ sung
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}