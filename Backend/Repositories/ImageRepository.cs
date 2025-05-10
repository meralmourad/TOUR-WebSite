using System;
using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories;

public class ImageRepository:GenericRepository<Images>
{
        private readonly AppDbContext _context;
        public ImageRepository(AppDbContext context): base(context)
        {
            _context = context;
        }


}
