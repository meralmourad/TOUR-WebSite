using System;
using Backend.IServices;
using Backend.Repositories;
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
    TripPlaceRepository TripPlace { get; }
    
    ImageRepository image { get; }
    Backend.Repositories.TripCategory tripCategory{get;}
    Category category{get;}
    
    Task<int> CompleteAsync();
}
