using Microsoft.AspNetCore.Mvc;

namespace WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        // Your action methods go here
        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            var types = AppConstants.TypesList
                .Select(t => new { t.Value, t.Label })
                .ToList();
            return Ok(new
            {
                data = types
            });
        }

        [HttpGet("record-types")]
        public IActionResult GetRecordTypes()
        {
            var recordTypes = AppConstants.RecordTypes
                .Select(t => new { t.Value, t.Label })
                .ToList();
            return Ok(new
            {
                data = recordTypes
            });
        }

        [HttpGet("status")]
        public IActionResult GetStatusList()
        {
            var statusList = AppConstants.StatusList
                .Select(t => new { t.Value, t.Label })
                .ToList();
            return Ok(new
            {
                data = statusList
            });
        }
    }
}