using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

public class MockCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories;

    public MockCategoryRepository()
    {
        _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Đàn Guitar & String", IconClass = "bi-music-note" },
            new Category { Id = 2, Name = "Piano & Keyboard", IconClass = "bi-music-node-nested" },
            new Category { Id = 3, Name = "Thiết bị Thu âm & Loa", IconClass = "bi-boombox" },
            new Category { Id = 4, Name = "Đĩa Nhạc Vinyl & Album", IconClass = "bi-disc" }
        };
    }

    public IEnumerable<Category> GetAll() => _categories;
    public Category GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);
}