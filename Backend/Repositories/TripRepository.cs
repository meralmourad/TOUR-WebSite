using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class TripRepository : GenericRepository<Trip>, ITripRepository
{
    public TripRepository(AppDbContext context) : base(context)
    {
    }

}
