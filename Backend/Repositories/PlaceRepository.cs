using System;
using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class PlaceRepository:GenericRepository<Place>,IplacePepoitory
{
    private readonly AppDbContext _context;
    public PlaceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public bool PlaceExists(string name)
    {
        return _context.Places.Any(p => p.Name == name);
    }
}
