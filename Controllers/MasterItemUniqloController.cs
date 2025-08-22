using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepApi.Data;
using WepApi.Models;
using WepApi.ModelsDto;
using WepApi.Services;

namespace WepApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class MasterItemUniqloController : ControllerBase
    {
        private readonly MySQLDbContext _context;

        private readonly IUserService _userService;

        public MasterItemUniqloController(MySQLDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // Define your API endpoints here
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.MasterItemUniqlos.Select(
                x => new MasterItemUniqloResultDto
                {
                    Type = x.Type,
                    UniqloCode = x.UniqloCode,
                    UniqloColor = x.UniqloColor,
                    UniqloDesc = x.UniqloDesc,
                    GreigeCode = x.GreigeCode,
                    FabricCode = x.FabricCode,
                    FabricColor = x.FabricColor,
                    FabricDesc = x.FabricDesc,
                    IsActive = x.IsActive
                }
            ).ToListAsync();

            return Ok(new
            {
                message = "Items retrieved successfully",
                data = items
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetListItems()
        {
            var items = await _context.MasterItemUniqlos
                .Select(x => new
                {
                    label = x.UniqloCode,
                    value = x.UniqloCode,
                })
                .ToListAsync();

            return Ok(new
            {
                message = "Items retrieved successfully",
                data = items
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _context.MasterItemUniqlos
                .Where(x => x.Id == id)
                .Select(x => new MasterItemUniqloResultDto
                {
                    Type = x.Type,
                    UniqloCode = x.UniqloCode,
                    UniqloColor = x.UniqloColor,
                    UniqloDesc = x.UniqloDesc,
                    GreigeCode = x.GreigeCode,
                    FabricCode = x.FabricCode,
                    FabricColor = x.FabricColor,
                    FabricDesc = x.FabricDesc,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound(new
                {
                    message = "Item not found"
                });
            }

            return Ok(new
            {
                message = "Item retrieved successfully",
                data = item
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] MasterItemUniqloDto itemDto)
        {
            var query = _context.MasterItemUniqlos
                .AsNoTracking()
                .Where(x => x.UniqloCode == itemDto.UniqloCode && x.Type == itemDto.Type);

            if (itemDto.Type == AppConstants.FABRIC_TYPE_VALUE)
            {
                if (string.IsNullOrEmpty(itemDto.FabricCode) || string.IsNullOrEmpty(itemDto.FabricColor))
                {
                    return BadRequest(new
                    {
                        message = "Fabric code and color must be provided for fabric type"
                    });

                }
                query = query.Where(x => x.FabricCode == itemDto.FabricCode);
                query = query.Where(x => x.FabricColor == itemDto.FabricColor);
            }
            else if (itemDto.Type == AppConstants.GREIGE_TYPE_VALUE)
            {
                if (string.IsNullOrEmpty(itemDto.GreigeCode))
                {
                    return BadRequest(new
                    {
                        message = "Greige code must be provided for greige type"
                    });
                }
                query = query.Where(x => x.GreigeCode == itemDto.GreigeCode);
            }
            else
            {
                return BadRequest(new
                {
                    message = "Invalid type provided"
                });
            }

            query = query.Where(x => x.IsActive == true);
            var existingItem = await query.SingleOrDefaultAsync();

            if (existingItem != null)
            {
                return Conflict(new
                {
                    message = "Item with the same code and type already exists"
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newItem = new MasterItemUniqlo
                {
                    Type = itemDto.Type,
                    UniqloCode = itemDto.UniqloCode,
                    UniqloColor = string.IsNullOrEmpty(itemDto.UniqloColor) ? null : itemDto.UniqloColor,
                    UniqloDesc = string.IsNullOrEmpty(itemDto.UniqloDesc) ? null : itemDto.UniqloDesc,
                    GreigeCode = string.IsNullOrEmpty(itemDto.GreigeCode) ? null : itemDto.GreigeCode,
                    FabricCode = string.IsNullOrEmpty(itemDto.FabricCode) ? null : itemDto.FabricCode,
                    FabricColor = string.IsNullOrEmpty(itemDto.FabricColor) ? null : itemDto.FabricColor,
                    FabricDesc = string.IsNullOrEmpty(itemDto.FabricDesc) ? null : itemDto.FabricDesc,
                    CreatedBy = await _userService.GetCurrentUserAsync(User),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true // Assuming new items are active by default
                };

                 _context.MasterItemUniqlos.Add(newItem);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the item",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                message = "Item created successfully",
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] MasterItemUniqloDto itemDto)
        {
            var item = await _context.MasterItemUniqlos.FindAsync(id);
            if (item == null)
            {
                return NotFound(new
                {
                    message = "Item not found"
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                item.Type = itemDto.Type;
                item.UniqloCode = itemDto.UniqloCode;
                item.UniqloColor = string.IsNullOrEmpty(itemDto.UniqloColor) ? null : itemDto.UniqloColor;
                item.UniqloDesc = string.IsNullOrEmpty(itemDto.UniqloDesc) ? null : itemDto.UniqloDesc;
                item.GreigeCode = string.IsNullOrEmpty(itemDto.GreigeCode) ? null : itemDto.GreigeCode;
                item.FabricCode = string.IsNullOrEmpty(itemDto.FabricCode) ? null : itemDto.FabricCode;
                item.FabricColor = string.IsNullOrEmpty(itemDto.FabricColor) ? null : itemDto.FabricColor;
                item.FabricDesc = string.IsNullOrEmpty(itemDto.FabricDesc) ? null : itemDto.FabricDesc;
                item.UpdatedBy = await _userService.GetCurrentUserAsync(User);
                item.UpdatedAt = DateTime.UtcNow;
                item.IsActive = itemDto.IsActive;

                _context.MasterItemUniqlos.Update(item);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the item",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                message = "Item updated successfully",
                data = item
            });
        }
    }
}