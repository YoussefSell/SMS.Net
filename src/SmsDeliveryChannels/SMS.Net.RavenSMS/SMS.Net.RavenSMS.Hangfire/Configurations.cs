namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// Set RavenSMS to use Hangfire queue for managing messages queue.
    /// </summary>
    /// <param name="builder">the RavenSMS builder instance.</param>
    /// <returns>instance of <see cref="RavenSmsBuilder"/> to enable methods chaining.</returns>
    /// <remarks>
    /// we assume that you already configured hangfire in your project, this package only provide an implementation of <see cref="ISmsChannel"/> that uses Hangfire background jobs for queuing the messages.
    /// </remarks>
    public static RavenSmsBuilder UseHangfireQueue(this RavenSmsBuilder builder)
    {
        builder.ServiceCollection.AddScoped<IQueueManager, HangfireQueueManager>();

        return builder;
    }
}
