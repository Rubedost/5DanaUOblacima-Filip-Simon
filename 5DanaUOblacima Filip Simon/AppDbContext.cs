using _5DanaUOblacima_Filip_Simon;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
