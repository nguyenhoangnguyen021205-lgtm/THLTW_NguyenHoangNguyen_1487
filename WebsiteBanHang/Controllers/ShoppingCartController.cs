using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanHang.Helpers;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(IProductRepository productRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            var currentQuantity = cart.Items.FirstOrDefault(i => i.ProductId == productId)?.Quantity ?? 0;
            if (currentQuantity + quantity > product.Stock)
            {
                TempData["ErrorMessage"] = $"Chỉ còn {product.Stock} sản phẩm trong kho.";
                return RedirectToAction("Index");
            }

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            };
            
            cart.AddItem(cartItem);

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.RemoveItem(productId);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product != null)
                {
                    var currentItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                    if (currentItem != null && currentItem.Quantity < product.Stock)
                    {
                        cart.IncreaseItem(productId);
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Rất tiếc, sản phẩm này chỉ còn {product.Stock} cái trong kho.";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.DecreaseItem(productId);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            
            var user = await _userManager.GetUserAsync(User);
            var order = new Order();
            if (user != null)
            {
                order.CustomerName = $"{user.FirstName} {user.LastName}".Trim();
                order.CustomerPhone = user.PhoneNumber;
                order.ShippingAddress = user.Address;
            }

            return View(order);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = System.DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(c => c.Total);
            order.OrderDetails = new List<OrderDetail>();

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    TempData["ErrorMessage"] = $"Sản phẩm {item.Name} không đủ số lượng trong kho. Vui lòng kiểm tra lại giỏ hàng.";
                    return RedirectToAction("Index");
                }

                product.Stock -= item.Quantity;
                product.SalesCount += item.Quantity; // Increase sales count
                await _productRepository.UpdateAsync(product);

                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            order.Notes ??= "";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return View("OrderCompleted", order.Id);
        }
    }
}
