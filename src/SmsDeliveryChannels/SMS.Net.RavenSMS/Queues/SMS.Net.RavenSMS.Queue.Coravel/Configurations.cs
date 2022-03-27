namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// Set RavenSMS to use Coravel queue for managing messages queue.
    /// </summary>
    /// <param name="builder">the RavenSMS builder instance.</param>
    /// <returns>instance of <see cref="RavenSmsBuilder"/> to enable methods chaining.</returns>
    /// <remarks>
    /// we assume that you already configured Coravel in your project, 
    /// this package only provide an implementation of <see cref="IQueueManager"/> that uses Coravel background jobs for queuing the messages.
    /// </remarks>
    public static RavenSmsBuilder UseCoravelQueue(this RavenSmsBuilder builder)
    {
        builder.ServiceCollection.AddScoped<RavenSmsProcessSmsMessageInvocable>();
        builder.ServiceCollection.AddScoped<IQueueManager, CoravelQueueManager>();

        return builder;
    }
}
