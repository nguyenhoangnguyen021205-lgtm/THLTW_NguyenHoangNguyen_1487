using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteBanHang.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Tạo Role Admin nếu chưa có
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Tạo tài khoản Hoàng Nguyên làm Admin
            var adminEmail = "nguyenhoang@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Hoàng",
                    LastName = "Nguyên",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Kiểm tra xem database có bất kỳ Category nào chưa
            if (!context.Categories.Any())
            {
                var cat1 = new Category { Name = "Guitar Điện" };
                var cat2 = new Category { Name = "Piano Điện" };
                var cat3 = new Category { Name = "Trống Điện Tử" };

                context.Categories.AddRange(cat1, cat2, cat3);
                await context.SaveChangesAsync();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Fender Stratocaster Pro",
                        Price = 25000000M,
                        Description = "Guitar điện phong cách Cyberpunk với âm thanh cực chất.",
                        ImageUrl = "https://images.unsplash.com/photo-1550291652-6ea9114a47b1?w=500&q=80",
                        Stock = 10,
                        IsFeatured = true,
                        Rating = 5.0,
                        SalesCount = 42,
                        CategoryId = cat1.Id
                    },
                    new Product
                    {
                        Name = "Yamaha P-125 Digital Piano",
                        Price = 16000000M,
                        Description = "Piano điện nhỏ gọn dành cho người chơi hiện đại.",
                        ImageUrl = "https://images.unsplash.com/photo-1520523839897-bd0b52f945a0?w=500&q=80",
                        Stock = 15,
                        IsFeatured = false,
                        Rating = 4.8,
                        SalesCount = 20,
                        CategoryId = cat2.Id
                    },
                    new Product
                    {
                        Name = "Roland TD-17KVX",
                        Price = 38000000M,
                        Description = "Bộ trống điện tử cao cấp cho trải nghiệm đánh cực thật.",
                        ImageUrl = "https://images.unsplash.com/photo-1519892300165-cb5542fb47c7?w=500&q=80",
                        Stock = 5,
                        IsFeatured = true,
                        Rating = 4.9,
                        SalesCount = 10,
                        CategoryId = cat3.Id
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
