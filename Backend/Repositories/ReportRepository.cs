using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    // Add any specific methods for Report if needed
}
