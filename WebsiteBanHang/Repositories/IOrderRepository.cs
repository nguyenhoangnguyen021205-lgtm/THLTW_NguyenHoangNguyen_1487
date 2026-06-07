using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
