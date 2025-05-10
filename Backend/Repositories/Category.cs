using System;
using Backend.Data;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// namespace Backend.Repositories;

public class Category:GenericRepository<Backend.Models.Category>
{

    private readonly AppDbContext _context;
    public Category(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
