using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Threading.Tasks;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile? imageFile)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Images");
            ModelState.Remove("Rating");
            ModelState.Remove("SalesCount");
            ModelState.Remove("IsFeatured");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
                return View(product);
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageFile);
            }
            else if (string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = "https://images.unsplash.com/photo-1511192336575-5a79af67a629?w=500&q=80";
            }

            product.Rating = 5.0;
            product.SalesCount = 0;

            await _productRepo.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.Id) return NotFound();

            ModelState.Remove("Category");
            ModelState.Remove("Images");
            ModelState.Remove("Rating");
            ModelState.Remove("SalesCount");
            ModelState.Remove("IsFeatured");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                ViewBag.CategoryList = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            var existingProduct = await _productRepo.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;

            if (imageFile != null && imageFile.Length > 0)
            {
                existingProduct.ImageUrl = await SaveImageAsync(imageFile);
            }
            else
            {
                existingProduct.ImageUrl = product.ImageUrl;
            }

            await _productRepo.UpdateAsync(existingProduct);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            try
            {
                await _productRepo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                // Bắt lỗi nếu sản phẩm đang nằm trong đơn hàng (lỗi khóa ngoại)
                TempData["ErrorMessage"] = "Không thể xóa nhạc cụ này vì nó đã có trong đơn hàng!";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Đã xóa nhạc cụ thành công!";
            return RedirectToAction(nameof(Index));
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
