namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the options for building the RavenSMS integration.
/// </summary>
public class RavenSmsBuilder
{
    /// <summary>
    /// create an instance of <see cref="RavenSmsBuilder"/>
    /// </summary>
    /// <param name="builder"></param>
    internal RavenSmsBuilder(SmsNetBuilder builder) => Builder = builder;

    /// <summary>
    /// the SMS builder instance
    /// </summary>
    internal SmsNetBuilder Builder { get; }

    /// <summary>
    /// Get the service collection.
    /// </summary>
    public IServiceCollection ServiceCollection => Builder.ServiceCollection;

    /// <summary>
    /// set the options values
    /// </summary>
    internal RavenSmsDeliveryChannelOptions InitOptions()
    {
        return new RavenSmsDeliveryChannelOptions
        {
            
        };
    }
}

