using System;
using Backend.Models;

namespace Backend.Repositories.Interfaces;

public interface IplacePepoitory:IGenericRepository<Place>
{
    bool PlaceExists(string name);
}
