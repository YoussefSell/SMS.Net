namespace SMS.Net;

/// <summary>
/// the Configurations class
/// </summary>
public static class SmsNetBuilderExtensions
{
    /// <summary>
    /// add the RavenSMS channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the SMS.Net builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseRavenSMS(this SmsNetBuilder builder, Action<RavenSmsBuilder> config)
    {
        builder.Services.AddRavenSMS(config);
        builder.Services.AddScoped<ISmsDeliveryChannel, RavenSmsDeliveryChannel>();
        builder.Services.AddScoped<IRavenSmsDeliveryChannel, RavenSmsDeliveryChannel>();
        return builder;
    }
}