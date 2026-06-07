using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Repositories.IProductRepository _productRepo;
        private readonly Repositories.ICategoryRepository _categoryRepo;
        private readonly Repositories.IOrderRepository _orderRepo;

        public AdminController(Repositories.IProductRepository productRepo, Repositories.ICategoryRepository categoryRepo, Repositories.IOrderRepository orderRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _orderRepo = orderRepo;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllAsync();

            ViewBag.TotalProducts = products.Count();
            ViewBag.TotalCategories = categories.Count();
            ViewBag.TotalOrders = orders.Count();
            ViewBag.TotalRevenue = orders.Sum(o => o.TotalPrice);

            return View();
        }
    }
}
