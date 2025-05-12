using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Backend.WebSockets; // Ensure this namespace is included

namespace Backend.Services;

public class Notificationservices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly WebSockets.WebSocketManager _webSocketManager; // Use the correct WebSocketManager

    public Notificationservices(IUnitOfWork unitOfWork, WebSockets.WebSocketManager webSocketManager)
    {
        _unitOfWork = unitOfWork;
        _webSocketManager = webSocketManager; // Initialize the correct WebSocketManager
    }

    public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
    {
        var notifications = await _unitOfWork.Notification.GetAllAsync();
        if (notifications == null)
            return null;

        var alluserNotifications = _unitOfWork.userNotification.Query();
        foreach (var notification in notifications)
        {
            var userNotification = alluserNotifications
                .Where(un => un.NotificationId == notification.Id)
                .Select(un => new UserNotification
                {
                    ReceiverId = un.ReceiverId,
                    NotificationId = un.NotificationId,
                    Notification = notification
                }).ToList();

            notification.UserNotifications = userNotification;
        }
        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            SenderId = n.SenderId,
            Context = n.Content,
            ReceiverId = n.UserNotifications.FirstOrDefault()?.ReceiverId ?? 0,
        }).ToList();
    }

    public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
    {
        var notification = await _unitOfWork.Notification.GetByIdAsync(id);
        if (notification == null)
            return null;
        var userNotification = _unitOfWork.userNotification.Query();
        var userNotifications = userNotification
            .Where(un => un.NotificationId == notification.Id)
            .Select(un => new UserNotification
            {
                ReceiverId = un.ReceiverId,
                NotificationId = un.NotificationId,
                Notification = notification
            }).ToList();
        notification.UserNotifications = userNotifications;
        return new NotificationDto
        {
            Id = notification.Id,
            SenderId = notification.SenderId,
            ReceiverId = notification.UserNotifications.FirstOrDefault()?.ReceiverId ?? 0,
            Context = notification.Content,
        };
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsByReceiverIdAsync(int receiverId)
    {
        var userNotifications = await _unitOfWork.userNotification.Query()
            .Include(un => un.Notification)
            .Where(un => un.ReceiverId == receiverId)
            .ToListAsync();
        if (userNotifications == null)
            return null;
        return userNotifications.Select(un => new NotificationDto
        {
            Id = un.Notification.Id,
            SenderId = un.Notification.SenderId,
            Context = un.Notification.Content,
            ReceiverId = un.ReceiverId,

        }).ToList();
    }
    //send notification to trip members
    public async Task<bool> SendNotificationToTripMembersAsync(NotificationDto notificationDto, int tripId)
    {
        // Log the input parameters
        Console.WriteLine($"[DEBUG] Sending notification to trip members. TripId: {tripId}, SenderId: {notificationDto.SenderId}");

        // Retrieve the trip
        var trip = await _unitOfWork.Trip.GetByIdAsync(tripId);
        if (trip == null)
        {
            Console.WriteLine("[ERROR] Trip not found.");
            return false;
        }
        Console.WriteLine($"[DEBUG] Trip found: {trip.Id}");

        // Retrieve bookings for the trip
        var bookings = _unitOfWork.BookingRepository.Query()
            // .Include(b => b.Tourist)
            .Where(b => b.TripId == tripId)
            .ToList();

        if (bookings == null || !bookings.Any())
        {
            Console.WriteLine("[ERROR] No bookings found for the trip.");
            return false;
        }
        Console.WriteLine($"[DEBUG] Bookings found: {bookings.Count}");

        // Create a new notification
        var notification = new Notification
        {
            SenderId = notificationDto.SenderId,
            Content = notificationDto.Context,
        };
        await _unitOfWork.Notification.AddAsync(notification);
        Console.WriteLine($"[DEBUG] Notification created with ID: {notification.Id}");

        // Create user notifications for each tourist in the trip
        foreach (var booking in bookings)
        {
            if (booking.TouristId == 0)
            {
                Console.WriteLine($"[WARNING] Invalid TouristId for booking: {booking.Id}");
                continue;
            }

            var userNotification = new UserNotification
            {
                NotificationId = notification.Id,
                ReceiverId = booking.TouristId,
                Notification = notification,
            };
            await _unitOfWork.userNotification.AddAsync(userNotification);
            //send notification via WebSocket
            var message = System.Text.Json.JsonSerializer.Serialize(new
            {
                NotificationId = notification.Id,
                SenderId = notification.SenderId,
                Content = notification.Content,
                ReceiverId = booking.TouristId
            });
            await _webSocketManager.SendMessageToUserAsync(booking.TouristId, message);
            Console.WriteLine($"[DEBUG] UserNotification created for TouristId: {booking.TouristId}");
        }

        // Save changes
        await _unitOfWork.CompleteAsync();
        Console.WriteLine("[DEBUG] Notifications sent successfully.");
        return true;
    }
    public async Task<bool> SendNotificationAsync(NotificationDto notificationDto, List<int> receiverIds)
    {
        var notification = new Notification
        {
            SenderId = notificationDto.SenderId,
            Content = notificationDto.Context,
        };

        await _unitOfWork.Notification.AddAsync(notification);

        foreach (var receiverId in receiverIds)
        {
            var userNotification = new UserNotification
            {
                ReceiverId = receiverId,
                Notification = notification,
            };
            await _unitOfWork.userNotification.AddAsync(userNotification);

            // Broadcast notification via WebSocket
            var message = System.Text.Json.JsonSerializer.Serialize(new
            {
                NotificationId = notification.Id,
                SenderId = notification.SenderId,
                Content = notification.Content,
                ReceiverId = receiverId
            });
            await _webSocketManager.SendMessageToUserAsync(receiverId, message);
        }


        await _unitOfWork.CompleteAsync();
        return true;
    }
}
