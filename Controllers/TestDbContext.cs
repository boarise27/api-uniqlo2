using Microsoft.AspNetCore.Mvc;
using WepApi.Data;

namespace WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDbContext : ControllerBase
    {
        private readonly MySQLDbContext _context;

        public TestDbContext(MySQLDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestConnection()
        {
            // Use _context to access your database
            try
            {
                var testConnection = await _context.Database.CanConnectAsync();
                return Ok("Database connection is working!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}