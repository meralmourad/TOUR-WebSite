using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
