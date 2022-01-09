namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// Set RavenSMS to use EF core stores to persist the data.
    /// </summary>
    /// <param name="builder">the RavenSMS builder instance.</param>
    /// <returns>instance of <see cref="RavenSmsBuilder"/> to enable methods chaining.</returns>
    /// <remarks>
    /// we assume that you already configured EF core in your project, this package only provide an implementation of stores that uses EF core to store the data.
    /// </remarks>
    public static RavenSmsBuilder UseEntityFrameworkStores(this RavenSmsBuilder builder)
    {
        builder.ServiceCollection.AddScoped<IRavenSmsClientsStore, RavenSmsClientsStore>();
        builder.ServiceCollection.AddScoped<IRavenSmsMessagesStore, RavenSmsMessagesStore>();

        return builder;
    }
}
