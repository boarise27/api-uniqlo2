using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepApi.Areas.Identity.Data;
using WepApi.ModelsDto;
using WepApi.Services;

namespace WepApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        private readonly WepApiIdentityDbContext _context;

        public UserController(UserManager<User> userManager, IUserService userService, WepApiIdentityDbContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.Select(u => new
            {
                user_id = u.Id,
                username = u.UserName,
                email = u.Email,
                first_name = u.FirstName,
                last_name = u.LastName,
                is_active = u.IsActive,
                is_admin = u.IsAdmin,
                full_name = $"{u.FirstName} {u.LastName}"
            }).ToListAsync();
            return Ok(new
            {
                data = users
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _context.Users.Select(u => new
            {
                user_id = u.Id,
                username = u.UserName,
                email = u.Email,
                first_name = u.FirstName,
                last_name = u.LastName,
                is_active = u.IsActive,
                is_admin = u.IsAdmin,
                full_name = $"{u.FirstName} {u.LastName}"
            }).FirstOrDefaultAsync(u => u.user_id == id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            return Ok(new
            {
                data = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDto model)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto model)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.UserName = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsActive = model.IsActive;
            user.IsAdmin = model.IsAdmin;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest(new { Message = "Password update failed.", Errors = passwordResult.Errors });
                }
            }

            await _userManager.UpdateAsync(user);

            return Ok(new { Message = "User updated successfully." });
        }
    }
}