using System;

namespace Backend.Models;

public class UserNotification
{
    //many to many between user and notification
    public int ReceiverId { get; set; }
    public User Receiver { get; set; }
    public int NotificationId { get; set; }
    public Notification Notification { get; set; }
    public bool IsRead { get; set; } = false;


}
