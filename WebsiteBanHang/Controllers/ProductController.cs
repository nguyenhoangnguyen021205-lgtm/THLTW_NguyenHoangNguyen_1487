using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        // TRANG CHỦ SHOWROOM (Hiển thị ảnh lưu trực tiếp trong DB)
        public async Task<IActionResult> Index(int? categoryId, string searchString)
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();

            if (categoryId.HasValue) products = products.Where(p => p.CategoryId == categoryId.Value);
            if (!string.IsNullOrEmpty(searchString)) products = products.Where(p => p.Name != null && p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            return View(products);
        }

        // TRANG QUẢN TRỊ ADMIN (Hiển thị ảnh lưu trực tiếp trong DB)
        public async Task<IActionResult> Manage()
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // XỬ LÝ LƯU THÊM MỚI (Linh hoạt File hoặc Link URL)
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageFile)
        {
            // 1. Nếu người dùng chọn file từ máy tính, tiến hành lưu file
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageFile);
            }
            // 2. Nếu không chọn file mà ô dán link ImageUrl trống, gán ảnh mặc định
            else if (string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = "https://images.unsplash.com/photo-1511192336575-5a79af67a629?w=500&q=80";
            }

            product.Rating = 5.0;
            product.SalesCount = 0;

            await _productRepo.AddAsync(product);
            return RedirectToAction(nameof(Manage));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // XỬ LÝ LƯU CẬP NHẬT CHỈNH SỬA (Linh hoạt File hoặc Link URL)
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageFile)
        {
            if (id != product.Id) return NotFound();

            var existingProduct = await _productRepo.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;

            // 1. Nếu người dùng upload file mới, ghi đè đường dẫn file
            if (imageFile != null && imageFile.Length > 0)
            {
                existingProduct.ImageUrl = await SaveImageAsync(imageFile);
            }
            // 2. Nếu không upload file, lấy chính xác chuỗi link ảnh người dùng vừa dán/sửa ở Form
            else
            {
                existingProduct.ImageUrl = product.ImageUrl;
            }

            await _productRepo.UpdateAsync(existingProduct);
            return RedirectToAction(nameof(Manage));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Manage));
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create)) { await image.CopyToAsync(fileStream); }
            return "/images/" + fileName;
        }
    }
}