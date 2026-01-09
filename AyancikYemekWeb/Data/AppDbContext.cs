using Microsoft.EntityFrameworkCore;
using AyancikYemekWeb.Models; 

namespace AyancikYemekWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<YemekModel> YemekMenuleri { get; set; }

        public DbSet<Abone> Aboneler { get; set; }
    }
}