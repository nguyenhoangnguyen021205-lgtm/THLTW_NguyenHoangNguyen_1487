using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

public class MockProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product {
            Id = 1, Name = "Fender Player Stratocaster Guitar", Price = 18500000, CategoryId = 1,
            Description = "Âm thanh Fender cổ điển, hoàn hảo cho mọi thể loại âm nhạc.",
            ImageUrl = "https://images.unsplash.com/photo-1550291652-6ea9114a47b1?w=500", IsFeatured = true
        },
        new Product {
            Id = 2, Name = "Yamaha P-125 Digital Piano", Price = 15900000, CategoryId = 2,
            Description = "Piano điện nhỏ gọn với âm thanh chân thực từ concert grand piano.",
            ImageUrl = "https://images.unsplash.com/photo-1524230572899-a752b3835840?w=500", IsFeatured = true
        },
        new Product {
            Id = 3, Name = "Tai Nghe Kiểm Âm Audio-Technica ATH-M50x", Price = 4200000, CategoryId = 3,
            Description = "Sự lựa chọn hàng đầu của các kỹ sư âm thanh và nghệ sĩ studio chuyên nghiệp.",
            ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500", IsFeatured = false
        },
        new Product {
            Id = 4, Name = "Đĩa Than Pink Floyd - The Dark Side of the Moon", Price = 1250000, CategoryId = 4,
            Description = "Album huyền thoại dưới định dạng đĩa vinyl chất lượng cao.",
            ImageUrl = "https://images.unsplash.com/photo-1603048588665-791ca8aea617?w=500", IsFeatured = true
        }
    };

    public IEnumerable<Product> GetAll() => _products;
    public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public void Add(Product product)
    {
        product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
        if (string.IsNullOrEmpty(product.ImageUrl)) product.ImageUrl = "https://images.unsplash.com/photo-1511671782779-c97d3d27a1d4?w=500";
        _products.Add(product);
    }

    public void Update(Product product)
    {
        var existing = GetById(product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.CategoryId = product.CategoryId;
            existing.Description = product.Description;
            existing.ImageUrl = string.IsNullOrEmpty(product.ImageUrl) ? existing.ImageUrl : product.ImageUrl;
            existing.IsFeatured = product.IsFeatured;
        }
    }

    public void Delete(int id)
    {
        var product = GetById(id);
        if (product != null) _products.Remove(product);
    }
}