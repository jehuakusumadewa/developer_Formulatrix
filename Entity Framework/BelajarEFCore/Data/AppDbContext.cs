using Microsoft.EntityFrameworkCore;
using BelajarEFCore.Models;

namespace BelajarEFCore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Mahasiswa> Mahasiswa {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = kampus.db");
    }
}