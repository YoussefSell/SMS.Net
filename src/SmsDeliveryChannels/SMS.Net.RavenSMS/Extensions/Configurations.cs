namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// add the RavenSMS channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the SMS.Net builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseRavenSMS(this SmsNetBuilder builder, Action<RavenSmsBuilder> config)
    {
        builder.ServiceCollection.AddRavenSMS(config);
        return builder;
    }
}