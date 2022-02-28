namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// extension for EF core configuration
/// </summary>
public static class EntityFrameworkExtensions
{
    /// <summary>
    /// Apply the Commune Configuration to all entities that derives from <see cref="Entity{Tkey}"/>
    /// </summary>
    /// <param name="modelBuilder">the module builder instance</param>
    /// <returns>the module builder instance</returns>
    public static ModelBuilder ApplyRavenSmsEntityConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RavenSmsClientEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RavenSmsMessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RavenSmsMessageSendAttemptEntityConfiguration());

        return modelBuilder;
    }
}

