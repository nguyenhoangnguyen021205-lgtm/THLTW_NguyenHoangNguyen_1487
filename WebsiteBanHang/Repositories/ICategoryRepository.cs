using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll(); // Đổi từ GetAllCategories() thành GetAll() để đồng bộ với Controller
        Category? GetById(int id); // Thêm dấu ? để sửa cảnh báo Null reference
    }
}