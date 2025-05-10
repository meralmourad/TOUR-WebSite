using System;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class MessageRepositories: GenericRepository<Message>, IMessageRepositories
{  
    public MessageRepositories( AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageByResiverAndSenderIDs(int senderId, int receiverId)
    {
        var messages =await _context.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
            }).ToListAsync();
        return messages;
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageBySenderId(int senderId)
    {
        return await _context.Messages
            .Where(m => m.SenderId == senderId)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
            }).ToListAsync();
    }
}
