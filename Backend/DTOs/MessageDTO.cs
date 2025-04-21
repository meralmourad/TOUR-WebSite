using Backend.Models;

namespace Backend.DTOs;

public class MessageDTO
{
    public required int Id { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    //mappers
    public static MessageDTO FromMessage(Message message)
    {
        return new MessageDTO
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }
    public static Message ToMessage(MessageDTO messageDTO)
    {
        return new Message
        {
            Id = messageDTO.Id,
            SenderId = messageDTO.SenderId,
            ReceiverId = messageDTO.ReceiverId,
            Content = messageDTO.Content,
            CreatedAt = messageDTO.CreatedAt
        };
    }
}