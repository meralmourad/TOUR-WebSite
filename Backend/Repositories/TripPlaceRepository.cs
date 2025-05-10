using System;
using Backend.Data;
using Backend.Models;

namespace Backend.Repositories;

public class TripPlaceRepository: GenericRepository<TripPlace>
{
     public TripPlaceRepository(AppDbContext context) : base(context) { }

}
