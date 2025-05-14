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
                // Path to Python script - adjust as needed
                string pythonScriptPath = Path.Combine(_environment.ContentRootPath, "Scripts", "generate_report.py");
                
                // Ensure the script directory exists
                string scriptDirectory = Path.GetDirectoryName(pythonScriptPath);
                if (!Directory.Exists(scriptDirectory))
                {
                    Directory.CreateDirectory(scriptDirectory);
                }

                // Check if script exists
                if (!System.IO.File.Exists(pythonScriptPath))
                {
                    return StatusCode(500, $"Python script not found at: {pythonScriptPath}");
                }

                // Expected PDF output path
                string pdfFileName = $"trip_report_agency_{agencyId}.pdf";
                string outputPath = Path.Combine(_environment.ContentRootPath, "Reports", pdfFileName);
                
                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Run the Python script as a process
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
                    
                    // Capture any output/errors
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        return StatusCode(500, $"Python script execution failed: {error}\nOutput: {output}");
                    }
                }

                // Check if PDF was generated
                if (!System.IO.File.Exists(outputPath))
                {
                    return NotFound($"Report file was not generated at path: {outputPath}");
                }

                // Return the PDF file
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