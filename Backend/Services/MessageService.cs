using System;
using Backend.Data;
using Backend.DTOs;
using Backend.IServices;
using Backend.Models;
using Backend.WebSockets;

namespace Backend.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MyWebSocketManager _ws;
    private readonly notificationSocket _nws;
    private readonly Notificationservices _notificationService;

    public MessageService(IUnitOfWork unitOfWork, MyWebSocketManager ws, notificationSocket notificationSocket,
        Notificationservices notificationService)
    {
        _notificationService = notificationService;
        _nws = notificationSocket;
        _ws = ws;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MessageDTO>> GetAllMessagesAsync()
    {
        var messages = await _unitOfWork.Message.GetAllAsync();
        if (messages == null)
            return null;
        return messages.Select(m => new MessageDTO
        {
            Id = m.Id,
            SenderId = m.SenderId,
            ReceiverId = m.ReceiverId,
            Content = m.Content,
        }).ToList();
    }

    public async Task<MessageDTO?> GetMessageByIdAsync(int id)
    {
        var message =await _unitOfWork.Message.GetByIdAsync(id);
        if (message == null)
            return null;
        return new MessageDTO
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
        };
        
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageByResiverAndSenderIDs(int senderId, int receiverId)
    {
        var messages = await _unitOfWork.Message.GetMessageByResiverAndSenderIDs(senderId, receiverId);
        if (messages == null)
            return null;
        
        return messages.Select(m => new MessageDTO
        {
            Id = m.Id,
            SenderId = m.SenderId,
            ReceiverId = m.ReceiverId,
            
            Content = m.Content,
        }).ToList();
    }

    public async Task<IEnumerable<MessageDTO>> GetMessagesByUserIdAsync(int userId)
    {
        var messages = await _unitOfWork.Message.GetMessageBySenderId(userId);
        if (messages == null)
            return null;
        return messages.Select(m => new MessageDTO
        {
            Id = m.Id,
            SenderId = m.SenderId,
            ReceiverId = m.ReceiverId,
            Content = m.Content,
        }).ToList();
    }

    public async Task<bool> SendMessageAsync(MessageDTO messageDto)
    {
        
        var user = await _unitOfWork.User.GetByIdAsync(messageDto.SenderId);
        if (user == null)
            return false;
        var messageToSend = new
        {
            SenderId = messageDto.SenderId,
            Content = messageDto.Content
        };
        var messageJson = System.Text.Json.JsonSerializer.Serialize(messageToSend);
        await _ws.SendMessageToUserAsync(messageDto.ReceiverId, messageJson);
        var userName = user.Name;
        var messageToSend2 = new
        {
            senderId = messageDto.SenderId,
            content = "You have a new message from " + userName,
        };
        
        var messageJson2 = System.Text.Json.JsonSerializer.Serialize(messageToSend2);
        await _nws.SendMessageToUserAsync(messageDto.ReceiverId, messageJson2);
        
        var message = new Message
        {
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
        };
        var notifi = new NotificationDto
        {
            Context = messageDto.Content,
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId 
        };
        await _notificationService.SendNotificationAsync(notifi, new List<int> {messageDto.ReceiverId},true);
        await _unitOfWork.Message.AddAsync(message);
        // insert into the database

        await _unitOfWork.CompleteAsync();
        return true;
    }
}
