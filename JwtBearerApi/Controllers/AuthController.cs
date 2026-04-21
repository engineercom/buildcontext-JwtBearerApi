using JwtBearerApi.Data;
using JwtBearerApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtBearerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<AppUser> userManager, IConfiguration config, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminOnly()
        {

            return Ok("Sadece admin rolüne sahip olanlar görebilir.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var appUser = new AppUser
            {

                UserName = registerDto.UserName,
                FullName = registerDto.FullName,
                Email = registerDto.Email


            };


            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            var role = await _roleManager.FindByNameAsync(registerDto.RoleName);
            if (role != null)
            {

                var resultRole = await _userManager.AddToRoleAsync(appUser, registerDto.RoleName);
                return Ok("User ve rolü oluşturuldu");
            }


            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var hasUser = await _userManager.FindByNameAsync(loginDto.Username);
            if (hasUser is null) return Unauthorized("User not found");
            var result = await _userManager.CheckPasswordAsync(hasUser, loginDto.Password);
            if (!result) return Unauthorized("Wrong Password");
            var token = GenerateToken(hasUser);

            return Ok(new { token });
        }

        private async Task<string> GenerateToken(AppUser user)
        {
            var claims = new List<Claim> {

            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName!)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
