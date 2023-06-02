using Microsoft.AspNetCore.Identity;

namespace OPizza.Models
{
    public class OrderViewModel
    {
        public List<OrderModel>? Orders { get; set; }
        public List<IdentityUser>? Users { get; set; }

        public OrderModel? Order { get; set; }
        public IdentityUser? User { get; set; }
    }
}
