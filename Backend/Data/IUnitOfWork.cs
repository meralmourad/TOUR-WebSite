using System;
using Backend.IServices;
using Backend.Repositories.Interfaces;

namespace Backend.Data;

public interface IUnitOfWork : IDisposable
{
    ITripRepository Trip { get; }
    IUserRepository User { get; }
    IBookingRepository BookingRepository { get; }
    INotificationRepository Notification { get; }
    IMessageRepositories Message{ get; }
    IReportRepository Report { get; }
    IplacePepoitory Place { get; }
    Task<int> CompleteAsync();
}
