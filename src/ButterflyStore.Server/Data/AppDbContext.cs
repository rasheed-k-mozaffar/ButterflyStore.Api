namespace ButterflyStore.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Cloth> Clothes { get; set; } = null!;
}