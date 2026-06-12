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

            // Thêm danh mục và 10 sản phẩm mới nếu chưa có
            if (!context.Categories.Any(c => c.Name == "Guitar Acoustic"))
            {
                var catAcoustic = new Category { Name = "Guitar Acoustic" };
                var catPhuKien = new Category { Name = "Phụ Kiện Âm Nhạc" };
                var catAmp = new Category { Name = "Ampli & Loa" };

                context.Categories.AddRange(catAcoustic, catPhuKien, catAmp);
                await context.SaveChangesAsync();

                var newProducts = new List<Product>
                {
                    new Product { Name = "Taylor 114ce", Price = 22500000M, Description = "Guitar Acoustic cao cấp từ thương hiệu Taylor danh tiếng.", ImageUrl = "", Stock = 10, CategoryId = catAcoustic.Id, IsFeatured = true, Rating = 5.0, SalesCount = 0 },
                    new Product { Name = "Yamaha F310", Price = 3500000M, Description = "Sự lựa chọn hoàn hảo cho người mới bắt đầu học Acoustic.", ImageUrl = "", Stock = 25, CategoryId = catAcoustic.Id, IsFeatured = false, Rating = 4.8, SalesCount = 15 },
                    new Product { Name = "Cordoba C5 CE", Price = 9800000M, Description = "Guitar Classic tích hợp EQ, âm thanh mộc mạc và chuẩn xác.", ImageUrl = "", Stock = 5, CategoryId = catAcoustic.Id, IsFeatured = false, Rating = 4.9, SalesCount = 3 },
                    new Product { Name = "Martin D-28", Price = 85000000M, Description = "Huyền thoại Guitar Acoustic, chất âm vang dội và cân bằng.", ImageUrl = "", Stock = 2, CategoryId = catAcoustic.Id, IsFeatured = true, Rating = 5.0, SalesCount = 1 },
                    new Product { Name = "Dây đàn Elixir Phosphor Bronze", Price = 450000M, Description = "Dây đàn cao cấp có lớp phủ chống gỉ sét Nanoweb.", ImageUrl = "", Stock = 100, CategoryId = catPhuKien.Id, IsFeatured = false, Rating = 5.0, SalesCount = 50 },
                    new Product { Name = "Capo D'Addario NS Pro", Price = 320000M, Description = "Capo nhôm siêu nhẹ, kẹp chắc chắn không làm rè dây.", ImageUrl = "", Stock = 50, CategoryId = catPhuKien.Id, IsFeatured = false, Rating = 4.7, SalesCount = 20 },
                    new Product { Name = "Chân để đàn (Guitar Stand) Aroma", Price = 180000M, Description = "Khung kim loại vững chắc, có bọc xốp bảo vệ đàn.", ImageUrl = "", Stock = 30, CategoryId = catPhuKien.Id, IsFeatured = false, Rating = 4.5, SalesCount = 10 },
                    new Product { Name = "Boss Katana-50 MkII", Price = 6500000M, Description = "Ampli Guitar 50W lý tưởng cho tập luyện và biểu diễn nhỏ.", ImageUrl = "", Stock = 8, CategoryId = catAmp.Id, IsFeatured = true, Rating = 4.9, SalesCount = 5 },
                    new Product { Name = "Fender Champion 20", Price = 3800000M, Description = "Ampli 20W nhỏ gọn với nhiều hiệu ứng âm thanh tích hợp.", ImageUrl = "", Stock = 15, CategoryId = catAmp.Id, IsFeatured = false, Rating = 4.6, SalesCount = 12 },
                    new Product { Name = "Dây tín hiệu Fender (Cable) 3m", Price = 350000M, Description = "Dây kết nối nhạc cụ chất lượng cao, chống nhiễu tuyệt đối.", ImageUrl = "", Stock = 40, CategoryId = catPhuKien.Id, IsFeatured = false, Rating = 4.8, SalesCount = 25 }
                };

                context.Products.AddRange(newProducts);
                await context.SaveChangesAsync();
            }
        }
    }
}
