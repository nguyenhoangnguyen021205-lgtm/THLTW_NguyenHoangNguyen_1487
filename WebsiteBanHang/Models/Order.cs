using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Đang Xử Lý";

        [Required, StringLength(100)]
        public string ShippingAddress { get; set; }

        public string Notes { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
