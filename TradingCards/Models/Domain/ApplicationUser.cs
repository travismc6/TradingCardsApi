using Microsoft.AspNetCore.Identity;

namespace TradingCards.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
