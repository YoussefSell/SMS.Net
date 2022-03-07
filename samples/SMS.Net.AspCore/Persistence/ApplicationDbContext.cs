namespace SMS.Net.Persistence;

public class ApplicationDbContext : DbContext, IRavenSmsDbContext
{
    public DbSet<RavenSmsClient> RavenSmsClients { get; set; } = default!;

    public DbSet<RavenSmsMessage> RavenSmsMessages { get; set; } = default!;

    public string ConnectionString { get; }

    public ApplicationDbContext()
    {
        ConnectionString = "Server=localhost;Database=sms.net;User=root;Password=root; Port=3306";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, serverVersion: ServerVersion.AutoDetect(ConnectionString))
            .LogTo(Console.WriteLine);

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyRavenSmsEntityConfiguration();
}
