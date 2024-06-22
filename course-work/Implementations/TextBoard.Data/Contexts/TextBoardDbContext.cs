using TextBoard.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TextBoard.Data.Contexts;

public class TextBoardDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Message> Messages { get; set; }

    public TextBoardDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=ML;Database=TextB;Trusted_Connection=True;TrustServerCertificate=true;");
    }
}
