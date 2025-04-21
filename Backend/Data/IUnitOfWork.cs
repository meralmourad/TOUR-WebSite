using System;
using Backend.Repositories.Interfaces;

namespace Backend.Data;

public interface IUnitOfWork : IDisposable
{
    ITripRepository Trip { get; }
    IUserRepository User { get; }

    Task<int> CompleteAsync();
}
