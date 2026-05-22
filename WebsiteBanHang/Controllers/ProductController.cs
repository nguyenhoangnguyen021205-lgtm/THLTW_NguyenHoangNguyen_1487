using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

        // 1. TRANG CHỦ (Chỉ dành cho Khách hàng xem, không có nút Sửa/Xóa)
        public IActionResult Index()
        {
            var products = _productRepo.GetAll();
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(products);
        }

        // 2. TRANG QUẢN TRỊ ADMIN (Nơi duy nhất có nút Thêm, Sửa, Xóa dưới dạng bảng chuyên nghiệp)
        public IActionResult Manage()
        {
            var products = _productRepo.GetAll();
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(products);
        }

        // 3. TRANG CHI TIẾT SẢN PHẨM
        public IActionResult Display(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _categoryRepo.GetAll();

            var allProducts = _productRepo.GetAll();
            var relatedProducts = allProducts
                                    .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                                    .Take(4)
                                    .ToList();
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        // 4. ADD PRODUCT (Thêm mới nhạc cụ)
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = _categoryRepo.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Add(product);
                return RedirectToAction(nameof(Manage)); // Thêm xong chuyển về trang Quản trị
            }
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(product);
        }

        // 5. UPDATE PRODUCT (Cập nhật nhạc cụ)
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _categoryRepo.GetAll();
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Update(product);
                return RedirectToAction(nameof(Manage)); // Sửa xong chuyển về trang Quản trị
            }
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(product);
        }

        // 6. DELETE PRODUCT (Xóa nhạc cụ)
        public IActionResult Delete(int id)
        {
            _productRepo.Delete(id);
            return RedirectToAction(nameof(Manage)); // Xóa xong quay lại trang Quản trị
        }
    }
}