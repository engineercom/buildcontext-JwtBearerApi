using Microsoft.AspNetCore.Identity;

namespace JwtBearerApi.Data
{
    public static class SeedData
    {
        public static async Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new AppRole { Name="Admin"});
            
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new AppRole { Name="User"});
            }
        
        }
    }
}
