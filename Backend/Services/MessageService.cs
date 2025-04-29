using System;
using Backend.Data;
using Backend.DTOs;
using Backend.IServices;
using Backend.Models;

namespace Backend.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    public MessageService(IUnitOfWork unitOfWork)
    {
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
            CreatedAt = m.CreatedAt
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
            CreatedAt = message.CreatedAt
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
            CreatedAt = m.CreatedAt
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
            CreatedAt = m.CreatedAt
        }).ToList();
    }

    public async Task<bool> SendMessageAsync(MessageDTO messageDto)
    {
        var message = new Message
        {
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Message.AddAsync(message);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
