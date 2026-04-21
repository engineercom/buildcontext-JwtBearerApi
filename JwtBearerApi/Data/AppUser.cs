using Microsoft.AspNetCore.Identity;

namespace JwtBearerApi.Data
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
       

    }
}
