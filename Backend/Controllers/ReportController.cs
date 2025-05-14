using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportServices _reportService;

        public ReportController(ReportServices reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
                return NotFound("Report not found.");

            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDto reportDto)
        {
            if (reportDto == null)
                return BadRequest("Invalid report data.");

            var result = await _reportService.CreateReportAsync(reportDto);
            if (result)
                return Ok("Report created successfully.");
            else
                return StatusCode(500, "An error occurred while creating the report.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            if (reports == null || !reports.Any())
                return NotFound("No reports found.");

            return Ok(reports);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _reportService.DeleteReportAsync(id);
            if (result)
                return Ok("Report deleted successfully.");
            else
                return NotFound("Report not found.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ReportDto reportDto)
        {
            if (reportDto == null)
                return BadRequest("Invalid report data.");

            var result = await _reportService.UpdateReportAsync(id, reportDto);
            if (result)
                return Ok("Report updated successfully.");
            else
                return NotFound("Report not found.");
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReportsByUserId(int userId)
        {
            var reports = await _reportService.GetReportsByUserIdAsync(userId);
            if (reports == null || !reports.Any())
                return NotFound("No reports found for this user.");

            return Ok(reports);
        }
        [HttpGet("trip/{tripId}")]
        public async Task<IActionResult> GetReportsByTripId(int tripId)
        {
            var reports = await _reportService.GetReportsByTripIdAsync(tripId);
            if (reports == null || !reports.Any())
                return NotFound("No reports found for this trip.");

            return Ok(reports);
        }
    }
}
