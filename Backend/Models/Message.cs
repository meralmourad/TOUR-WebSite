using System;

namespace Backend.Models;

public class Message
{
    public int Id { get; set; }
    public required string Content { get; set; }    
    public int ReceiverId { get; set; } 
    public User? Receiver { get; set; } 

    public int SenderId { get; set; }
    public User Sender { get; set; } 
}
