namespace SMS.Net.Persistence;

public class ApplicationDbContext : DbContext, IRavenSmsDbContext
{
    public DbSet<RavenSmsClient> Clients { get; set; } = default!;

    public DbSet<RavenSmsMessage> Messages { get; set; } = default!;

    public string DatabasePath { get; }

    public ApplicationDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DatabasePath = Path.Join(path, "SmsNet.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
}
