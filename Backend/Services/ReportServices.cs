using System;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ReportServices 
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateReportAsync(ReportDto reportDto)
    {
        var report = new Report
        {
            SenderId = reportDto.SenderId,
            TripId = reportDto.TripId,
            Content = reportDto.Content
        };

        await _unitOfWork.Report.AddAsync(report);
        return await _unitOfWork.CompleteAsync() > 0;
    }
    //delete report
    public async Task<bool> DeleteReportAsync(int reportId)
    {
        var report = await _unitOfWork.Report.GetByIdAsync(reportId);
        if (report == null)
            return false;

        _unitOfWork.Report.Delete(report);
        return await _unitOfWork.CompleteAsync() > 0;
    }
    // get all reports
    public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
    {
        var reports = await _unitOfWork.Report.GetAllAsync();
        if (reports == null)
            return null;

        return reports.Select(r => new ReportDto
        {
            SenderId = r.SenderId,
            TripId = r.TripId,
            Content = r.Content
        }).ToList();
    }
    // get report by id
    public async Task<ReportDto?> GetReportByIdAsync(int id)
    {
        var report = await _unitOfWork.Report.GetByIdAsync(id);
        if (report == null)
            return null;

        return new ReportDto
        {
            SenderId = report.SenderId,
            TripId = report.TripId,
            Content = report.Content
        };
    }
    // get all reports by trip id
    public async Task<IEnumerable<ReportDto>> GetReportsByTripIdAsync(int tripId)
    {
        var reports = await _unitOfWork.Report.Query().Where(r => r.TripId == tripId).ToListAsync();
        if (reports == null)
            return null;

        return reports.Select(r => new ReportDto
        {
            SenderId = r.SenderId,
            TripId = r.TripId,
            Content = r.Content
        }).ToList();
    }
    // get all reports by sender id
    public async Task<IEnumerable<ReportDto>> GetReportsBySenderIdAsync(int senderId)
    {
        var reports = await _unitOfWork.Report.Query().Where(r => r.SenderId == senderId).ToListAsync();
        if (reports == null)
            return null;

        return reports.Select(r => new ReportDto
        {
            SenderId = r.SenderId,
            TripId = r.TripId,
            Content = r.Content
        }).ToList();
    }
    // get all reports by sender id and trip id
    public async Task<IEnumerable<ReportDto>> GetReportsBySenderIdAndTripIdAsync(int senderId, int tripId)
    {
        var reports = await _unitOfWork.Report.Query()
            .Where(r => r.SenderId == senderId && r.TripId == tripId)
            .ToListAsync();
        if (reports == null)
            return null;

        return reports.Select(r => new ReportDto
        {
            SenderId = r.SenderId,
            TripId = r.TripId,
            Content = r.Content
        }).ToList();
    }

    // Update an existing report
    public async Task<bool> UpdateReportAsync(int reportId, ReportDto reportDto)
    {
        var report = await _unitOfWork.Report.GetByIdAsync(reportId);
        if (report == null)
            return false;

        report.Content = reportDto.Content;
        report.TripId = reportDto.TripId;
        report.SenderId = reportDto.SenderId;

        _unitOfWork.Report.Update(report);
        return await _unitOfWork.CompleteAsync() > 0;
    }
    // Get all reports for a specific user
    public async Task<IEnumerable<ReportDto>> GetReportsByUserIdAsync(int userId)
    {
        var reports = await _unitOfWork.Report.Query()
            .Where(r => r.SenderId == userId)
            .ToListAsync();
        if (reports == null)
            return null;

        return reports.Select(r => new ReportDto
        {
            SenderId = r.SenderId,
            TripId = r.TripId,
            Content = r.Content
        }).ToList();
    }
    
}

