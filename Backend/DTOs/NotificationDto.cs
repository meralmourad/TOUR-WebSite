using Backend.Models;

namespace Backend.DTOs;

public class NotificationDto
{
    public required int Id { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required string Context { get; set; }
    public  required DateTime CreatedAt { get; set; }
    // mappers
    public static NotificationDto FromNotification(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            SenderId = notification.SenderId,
            ReceiverId = notification.ReceiverId,
            Context = notification.Content,
            CreatedAt = notification.CreatedAt
        };
    }
    public static Notification ToNotification(NotificationDto notificationDto)
    {
        return new Notification
        {
            Id = notificationDto.Id,
            SenderId = notificationDto.SenderId,
            ReceiverId = notificationDto.ReceiverId,
            Content = notificationDto.Context,
            CreatedAt = notificationDto.CreatedAt
        };
    }
}
