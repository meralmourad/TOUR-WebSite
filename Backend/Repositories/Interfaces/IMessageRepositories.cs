using System;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Repositories.Interfaces;

public interface IMessageRepositories : IGenericRepository<Message>
{
    public Task<IEnumerable<MessageDTO>> GetMessageByResiverAndSenderIDs(int senderId, int receiverId);
    public Task<IEnumerable<MessageDTO>> GetMessageBySenderId(int senderId);
}
