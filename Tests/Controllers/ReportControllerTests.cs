using Backend.Controllers;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<ReportServices> _mockReportService;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _mockReportService = new Mock<ReportServices>(null!);
            _controller = new ReportController(_mockReportService.Object);
        }

        [Fact]
        public async Task GetReportById_ReturnsOk_WhenReportExists()
        {
            // Arrange
            var reportId = 1;
            var report = new ReportDto { Id = reportId, Content = "Test Content" };
            _mockReportService.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync(report);

            // Act
            var result = await _controller.GetReportById(reportId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, okResult.Value);
        }

        [Fact]
        public async Task GetReportById_ReturnsNotFound_WhenReportDoesNotExist()
        {
            // Arrange
            var reportId = 1;
            _mockReportService.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync((ReportDto?)null);

            // Act
            var result = await _controller.GetReportById(reportId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateReport_ReturnsOk_WhenReportIsCreated()
        {
            // Arrange
            var reportDto = new ReportDto { Content = "Test Content" };
            _mockReportService.Setup(s => s.CreateReportAsync(reportDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateReport(reportDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Report created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateReport_ReturnsBadRequest_WhenReportDtoIsNull()
        {
            // Act
            var result = await _controller.CreateReport(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteReport_ReturnsOk_WhenReportIsDeleted()
        {
            // Arrange
            var reportId = 1;
            _mockReportService.Setup(s => s.DeleteReportAsync(reportId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteReport(reportId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Report deleted successfully.", okResult.Value);
        }

        [Fact]
        public async Task DeleteReport_ReturnsNotFound_WhenReportDoesNotExist()
        {
            // Arrange
            var reportId = 1;
            _mockReportService.Setup(s => s.DeleteReportAsync(reportId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteReport(reportId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateReport_ReturnsOk_WhenReportIsUpdated()
        {
            // Arrange
            var reportId = 1;
            var reportDto = new ReportDto { Content = "Updated Content" };
            _mockReportService.Setup(s => s.UpdateReportAsync(reportId, reportDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateReport(reportId, reportDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Report updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateReport_ReturnsNotFound_WhenReportDoesNotExist()
        {
            // Arrange
            var reportId = 1;
            var reportDto = new ReportDto { Content = "Updated Content" };
            _mockReportService.Setup(s => s.UpdateReportAsync(reportId, reportDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateReport(reportId, reportDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
