using Microsoft.AspNetCore.Identity;

namespace JwtBearerApi.Data
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }

    }
}
