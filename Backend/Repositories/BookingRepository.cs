using System;
using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class BookingRepository:GenericRepository<Booking>,IBookingRepository 
{
    private readonly AppDbContext _context;
    public BookingRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
