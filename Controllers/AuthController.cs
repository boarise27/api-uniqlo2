using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WepApi.ModelsDto;
using WepApi.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using WepApi.Services;
namespace WepApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        private readonly IUserService _userService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env, IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid login attempt." });
            }

            return await CreateToken(model.Username);
        }


        private async Task<IActionResult> CreateToken(String username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(configuration["Jwt:ExpiryMinutes"] != null ? Convert.ToInt32(configuration["Jwt:ExpiryMinutes"]) : 30),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token), message = "Token created successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Username already exists." });
            }
            var existingEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmail != null)
            {
                return BadRequest(new { Message = "Email already exists." });
            }

            var MyUser = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(MyUser, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "User registration failed.", Errors = result.Errors });
            }

            return Ok(new { Message = "User registered successfully." });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID not found in claims." });
            }
            var userProfile = await _userManager.FindByIdAsync(userId);
            if (userProfile == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var _fullName = await _userService.GetCurrentUserAsync(User);

            return Ok(new
            {
                message = "User profile retrieved successfully.",
                fullName = _fullName
            });
        }
    }
}