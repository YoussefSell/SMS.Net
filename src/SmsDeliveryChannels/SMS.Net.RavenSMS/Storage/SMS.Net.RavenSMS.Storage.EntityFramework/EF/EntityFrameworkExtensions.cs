namespace SMS.Net.Channel.RavenSMS.Stores.EntityFramework;

/// <summary>
/// extension for EF core configuration
/// </summary>
public static class EntityFrameworkExtensions
{
    /// <summary>
    /// Apply the RavenSMS entities configuration. 
    /// which include configuration for <see cref="RavenSmsClient"/>, <see cref="RavenSmsMessage"/>, and <see cref="RavenSmsMessageSendAttempt"/> entities
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

