using System;
using Backend.Data;
using Backend.Models;

namespace Backend.Repositories;

public class UserNotificationRepository : GenericRepository<UserNotification>
{
    public UserNotificationRepository(AppDbContext context) : base(context)
    {
    }
}
