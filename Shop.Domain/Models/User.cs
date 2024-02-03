using Microsoft.AspNetCore.Identity;

namespace Shop.Domain.Models
{
    public class User : IdentityUser
    { 
        public IEnumerable<Order> Orders { get; set; }
    }
}