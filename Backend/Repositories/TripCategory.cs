using System;
using Backend.Data;

namespace Backend.Repositories;

public class TripCategory : GenericRepository<Backend.Models.TripCategory>
{
    public TripCategory(AppDbContext context) : base(context)
    {
    }
}
