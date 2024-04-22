using Microsoft.AspNetCore.Identity;

namespace LOT_TASK.Models
{
    public class UserModel : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
