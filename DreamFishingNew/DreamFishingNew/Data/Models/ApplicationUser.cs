using Microsoft.AspNetCore.Identity;

namespace DreamFishingNew.Data.Models
{
    public class ApplicationUser: IdentityUser
    {
        public int ProductCartId { get; set; }
        public ProductCart ProductCart { get; set; } = new ProductCart();

      
    }
}
