using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vendorReports : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        
        public vendorReports(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("trip-report/{agencyId}")]
        public async Task<IActionResult> GetTripReport(int agencyId)
        {
            try
            {
                string pythonScriptPath = Path.Combine(_environment.ContentRootPath, "Scripts", "generate_report.py");
                
                string scriptDirectory = Path.GetDirectoryName(pythonScriptPath);
                if (!Directory.Exists(scriptDirectory))
                {
                    Directory.CreateDirectory(scriptDirectory);
                }

                if (!System.IO.File.Exists(pythonScriptPath))
                {
                    return StatusCode(500, $"Python script not found at: {pythonScriptPath}");
                }

                string pdfFileName = $"trip_report_agency_{agencyId}.pdf";
                string outputPath = Path.Combine(_environment.ContentRootPath, "Reports", pdfFileName);
                
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Users\lenovo\AppData\Local\Microsoft\WindowsApps\PythonSoftwareFoundation.Python.3.11_qbz5n2kfra8p0\python.exe",
                        Arguments = $"\"{pythonScriptPath}\" {agencyId} \"{outputPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = scriptDirectory
                    };

                    process.Start();
                    
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        return StatusCode(500, $"Python script execution failed: {error}\nOutput: {output}");
                    }
                }

                if (!System.IO.File.Exists(outputPath))
                {
                    return NotFound($"Report file was not generated at path: {outputPath}");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(outputPath);
                return File(fileBytes, "application/pdf", pdfFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}