using System;
using Backend.Repositories;
using Backend.Repositories.Interfaces;

namespace Backend.Data;

public class UnitOfWork :IUnitOfWork
{
    private readonly AppDbContext _context;
    public ITripRepository Trip { get; private set; }
    public IUserRepository User { get; private set; }
    
    public UnitOfWork(AppDbContext context)
    {    
        _context = context;
        Trip = new TripRepository(_context);
        User = new UserRepository(_context);
        //if you want to use any other repository, you can initialize it here as well.
        // _repo = new _repo("_context");
    }
    public async Task<int> CompleteAsync() =>await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
