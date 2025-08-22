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
    public class UqImportViewController : ControllerBase
    {
        private readonly MySQLDbContext _context;
        private readonly IUserService _userService;

        public UqImportViewController(MySQLDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // Define your API endpoints here
        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] string? opt = null, [FromQuery] string? rt = null, [FromQuery] string? w = null, [FromQuery] string? s = null)
        {

            var query = _context.UqImportViews.AsQueryable();

            if (!string.IsNullOrEmpty(opt))
            {
                query = query.Where(x => x.OutputType == opt);
            }

            if (!string.IsNullOrEmpty(rt))
            {
                query = query.Where(x => x.RecordType == rt);
            }

            if (!string.IsNullOrEmpty(w))
            {
                if (!DateTime.TryParse(w, out var weekSysDate))
                {
                    return BadRequest(new
                    {
                        message = "Invalid date format for WeekSys. Expected format: yyyy-MM-dd"
                    });
                }
                query = query.Where(x => x.WeekSys == weekSysDate);
            }

            if (!string.IsNullOrEmpty(s))
            {
                query = query.Where(x => x.Status == s);
            }
            else
            {
                query = query.Where(x => x.Status == "D" || x.Status == "C");
            }

            query = query.OrderBy(x => x.Status == "D" ? 0 : x.Status == "C" ? 1 : 2);

            var datas = await query.Select(x => new UqImportViewResultDto
            {
                Id = x.Id,
                OutputType = x.OutputType,
                OutputItem = x.OutputItem == "TEMP" ? "" : x.OutputItem,
                OutputColor = x.OutputColor,
                RecordType = x.RecordType == "T" ? null : x.RecordType,
                InputItem = x.InputItem,
                InputColor = x.InputColor,
                WeekSys = x.WeekSys.ToString("yyyy-MM-dd"),
                QtySys = x.QtySys,
                QtyConfirm = x.QtyConfirm,
                Status = x.Status,
                CreatedAt = x.CreatedAt != null ? x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                UpdatedAt = x.UpdatedAt != null ? x.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
            }).ToListAsync();

            return Ok(new
            {
                message = "Items retrieved successfully",
                data = datas
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _context.UqImportViews.FindAsync(id);
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
        public async Task<IActionResult> CreateItem([FromBody] UqImportViewAddRowDto itemDto)
        {
            if (itemDto.Type != AppConstants.FABRIC_TYPE_VALUE && itemDto.Type != AppConstants.GREIGE_TYPE_VALUE)
            {
                return BadRequest(new
                {
                    message = "Invalid type specified"
                });
            }
            var userFullName = await _userService.GetCurrentUserAsync(User);
            // ดึงวันจันทร์ของสัปดาห์ปัจจุบัน
            var today = DateTime.Today;
            int diff = today.DayOfWeek == DayOfWeek.Sunday ? -6 : DayOfWeek.Monday - today.DayOfWeek;
            var monday = today.AddDays(diff);

            var newItems = Enumerable.Range(0, itemDto.Amount).Select(_ => new UqImportView
            {
                OutputType = itemDto.Type,
                OutputItem = "TEMP",
                RecordType = "T",
                WeekSys = monday,
                Status = "D",
                OutputColor = itemDto.Type == AppConstants.GREIGE_TYPE_VALUE ? "Greige" : null,
                InputItem = itemDto.Type == AppConstants.GREIGE_TYPE_VALUE ? "Greige" : null,
                InputColor = itemDto.Type == AppConstants.GREIGE_TYPE_VALUE ? "Greige" : null,
                CreatedBy = userFullName,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.UqImportViews.AddRange((IEnumerable<UqImportView>)newItems);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the items",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                message = "Items created successfully",
                count = newItems.Count
            });
        }

        [HttpPut("update-column/{id}")]
        public async Task<IActionResult> UpdateByColumn(int id, [FromBody] UqImportViewUpdateByColumn itemDto)
        {
            var item = await _context.UqImportViews.FindAsync(id);
            if (item == null)
            {
                return NotFound(new
                {
                    message = "Item not found"
                });
            }

            var columnToProperty = new Dictionary<string, string>
                {
                    { "output_item", "OutputItem" },
                    { "output_color", "OutputColor" },
                    { "input_item", "InputItem" },
                    { "input_color", "InputColor" },
                    { "record_type", "RecordType" },
                    { "week_sys", "WeekSys" },
                    { "output_type", "OutputType" },
                    { "qty_sys", "QtySys" },
                    { "qty_confirm", "QtyConfirm" },
                    { "status", "Status" },
                    // ...อื่น ๆ
            };
            var propertyName = columnToProperty.TryGetValue(itemDto.ColumnName!, out var prop) ? prop : itemDto.ColumnName!;
            var property = typeof(UqImportView).GetProperty(propertyName);
            if (property == null)
            {
                return BadRequest(new
                {
                    message = $"Column '{itemDto.ColumnName}' does not exist in the model"
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (itemDto.ColumnName!.Equals("week_sys", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParse(itemDto.NewValue?.ToString(), out var parsedDate))
                    {
                        property.SetValue(item, parsedDate.Date);
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            message = "Invalid date format for WeekSys. Expected format: yyyy-MM-dd"
                        });
                    }
                }
                else
                {
                    property.SetValue(item, itemDto.NewValue);
                }
                property.SetValue(item, itemDto.NewValue);
                item.UpdatedBy = await _userService.GetCurrentUserAsync(User);
                item.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new
                {
                    message = $"Failed to update column '{itemDto.ColumnName}': {ex.Message}"
                });
            }

            return Ok(new
            {
                message = "Item updated successfully"
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UqImportViewUpdateRowDto itemDto)
        {
            var item = await _context.UqImportViews.FindAsync(id);
            if (item == null)
            {
                return NotFound(new
                {
                    message = "Item not found"
                });
            }

            item.OutputType = itemDto.OutputType!;
            item.OutputItem = itemDto.OutputItem!;
            item.OutputColor = string.IsNullOrEmpty(itemDto.OutputColor) ? null : itemDto.OutputColor!;
            item.RecordType = itemDto.RecordType!;
            item.InputItem = string.IsNullOrEmpty(itemDto.InputItem) ? null : itemDto.InputItem!;
            item.InputColor = string.IsNullOrEmpty(itemDto.InputColor) ? null : itemDto.InputColor!;
            if (!DateTime.TryParse(itemDto.WeekSys, out var parsedWeekSys))
            {
                return BadRequest(new
                {
                    message = "Invalid date format for WeekSys. Expected format: yyyy-MM-dd"
                });
            }
            item.WeekSys = parsedWeekSys.Date;
            if (!string.IsNullOrEmpty(itemDto.QtySys) && int.TryParse(itemDto.QtySys, out var qtySysValue))
            {
                item.QtySys = qtySysValue;
            }
            else
            {
                item.QtySys = null;
            }
            if (!string.IsNullOrEmpty(itemDto.QtyConfirm) && int.TryParse(itemDto.QtyConfirm, out var qtyConfirmValue))
            {
                item.QtyConfirm = qtyConfirmValue;
            }
            else
            {
                item.QtyConfirm = null;
            }
            item.UpdatedBy = await _userService.GetCurrentUserAsync(User);
            item.UpdatedAt = DateTime.UtcNow;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new
                {
                    message = $"Failed to update item: {ex.Message}"
                });
            }

            return Ok(new
            {
                message = "Item updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.UqImportViews.FindAsync(id);
            if (item == null)
            {
                return NotFound(new
                {
                    message = "Item not found"
                });
            }

            _context.UqImportViews.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Item deleted successfully"
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userService.GetCurrentUserAsync(User);
                var items = await _context.UqImportViews.Where(x => x.OutputItem == "TEMP" && x.CreatedBy == user).ToListAsync();
                if (items == null || items.Count == 0)
                {
                    return NotFound(new
                    {
                        message = "Item not found"
                    });
                }

                _context.UqImportViews.RemoveRange(items);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new
                {
                    message = $"Failed to delete item: {ex.Message}"
                });
            }

            return Ok(new
            {
                message = "Item deleted successfully"
            });
        }
    }
}