using System;
using Backend.DTOs;
using Backend.Models;

namespace Backend.IServices;

public interface IMessageService
{  
    // Task<IEnumerable<MessageDTO>> GetAllMessagesAsync();
    Task<MessageDTO> GetMessageByIdAsync(int id);
    Task<bool> SendMessageAsync(MessageDTO messageDto);
    Task<IEnumerable<MessageDTO>> GetMessagesByUserIdAsync(int userId);
    //get messages by receiver id and sender id
    Task<IEnumerable<MessageDTO>> GetMessageByResiverAndSenderIDs(int senderId, int receiverId);
}
