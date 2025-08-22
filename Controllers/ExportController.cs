using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepApi.Data;

namespace WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly MySQLDbContext _dbContext;

        public ExportController(MySQLDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("export-data")]
        public async Task<IActionResult> ExportData()
        {
            var result = await _dbContext.UqImportViews
                .Where(x => x.Status == "C" && (x.OutputItem == "TEMP" || x.RecordType != "T"))
                .OrderBy(x => x.RecordType)
                .Select(x => new
                {
                    data_type_code = x.RecordType,
                    prod_mgmt_factory = "NAN YANG TEXTILE GROUP_0N33",
                    production_facility_uid = x.RecordType == "3" ? 9999 : (int?)null,
                    splr_mtrl_cd_output = x.OutputItem,
                    splr_color_cd_output = x.OutputType == "F" ? x.OutputColor : "Greige",
                    production_lot_id = x.RecordType == "3" ? "Dummy" : null,
                    production_start_date = x.RecordType == "3" ? "9999-12-31" : null,
                    splr_mtrl_cd_input = x.RecordType == "2" ? x.InputItem : null,
                    splr_color_cd_input = (x.RecordType == "2" && x.OutputType == "G") ? "Greige"
                        : (x.RecordType == "2" ? x.InputColor : null),
                    po_number = x.RecordType == "3" ? "Dummy" : null,
                    consumed_material_production_lot_id = "",
                    week = x.WeekSys,
                    input_act = x.RecordType == "2" ? x.QtyConfirm : 0,
                    output_plan = x.RecordType == "1" ? x.QtyConfirm : 0,
                    output_act = x.RecordType == "3" ? x.QtyConfirm : 0
                })
                .ToListAsync();
            if (result.Count == 0)
            {
                return NotFound("No data found to export.");
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Data Type Code,Prod Mgmt Factory,Production Facility UID,Splr Mtrl Cd (Output),Splr Color Cd(Output),Production Lot ID,Production Start Date,Splr Mtrl Cd(Input),Splr Color Cd(Input),PO Number,Consumed Material Production Lot ID,Week,[Input] Act,[Output] Plan,[Output] Act");
            foreach (var item in result)
            {
                sb.AppendLine($"{item.data_type_code},{item.prod_mgmt_factory},{item.production_facility_uid},{item.splr_mtrl_cd_output},{item.splr_color_cd_output},{item.production_lot_id},{item.production_start_date},{item.splr_mtrl_cd_input},{item.splr_color_cd_input},{item.po_number},{item.consumed_material_production_lot_id},{item.week},{item.input_act},{item.output_plan},{item.output_act}");
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            var now = DateTime.Now;
            var formattedDate = now.ToString("yyyyMMddHHmmssfff");
            var fileName = $"MaterialProductionInfo_{formattedDate}.csv";
            return File(bytes, "text/csv", fileName);
        }
    }
}
