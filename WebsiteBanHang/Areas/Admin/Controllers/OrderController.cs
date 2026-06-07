using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly Repositories.IOrderRepository _orderRepo;

        public OrderController(Repositories.IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllAsync();
            return View(orders);
        }
    }
}
