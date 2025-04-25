using System;
using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class MessageRepositories: GenericRepository<Message>, IMessageRepositories
{  
    public MessageRepositories( AppDbContext context) : base(context)
    {
    }

}
