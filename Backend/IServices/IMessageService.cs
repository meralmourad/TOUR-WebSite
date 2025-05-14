using System;
using Backend.DTOs;
using Backend.Models;

namespace Backend.IServices;

public interface IMessageService
{  
    Task<MessageDTO> GetMessageByIdAsync(int id);
    Task<bool> SendMessageAsync(MessageDTO messageDto);
    Task<IEnumerable<MessageDTO>> GetMessagesByUserIdAsync(int userId);
    Task<IEnumerable<MessageDTO>> GetMessageByResiverAndSenderIDs(int senderId, int receiverId);
}
