using System;
using Backend.Repositories;
using Backend.Repositories.Interfaces;

namespace Backend.Data;

public class UnitOfWork :IUnitOfWork
{
    private readonly AppDbContext _context;
    public ITripRepository Trip { get; private set; }
    public IUserRepository User { get; private set; }
    public IBookingRepository BookingRepository { get; private set; }
    public INotificationRepository Notification { get; private set; }
    public IMessageRepositories Message { get; private set; }
    public IReportRepository Report { get; private set; }
    public IplacePepoitory Place { get; private set; }
    
    public UnitOfWork(AppDbContext context)
    {    
        _context = context;
        Notification = new NotificationRepository(_context);
        Message = new MessageRepositories(_context);
        Report = new ReportRepository(_context);
        Trip = new TripRepository(_context);
        User = new UserRepository(_context);
        BookingRepository = new BookingRepository(_context);
        Place = new PlaceRepository(_context);

    }
    public async Task<int> CompleteAsync() =>await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
