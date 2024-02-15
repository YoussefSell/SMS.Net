namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsServiceFactory"/> factory.
/// </summary>
public static class D7NetworksSmsServiceFactoryExtensions
{
    /// <summary>
    /// add the D7Networks channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
    /// <param name="apiKey">Set your D7Networks api-key.</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
    public static SmsServiceFactory UseD7Networks(this SmsServiceFactory builder, string apiKey)
       => builder.UseD7Networks(op => { op.ApiKey = apiKey; });

    /// <summary>
    /// add the D7Networks channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
    public static SmsServiceFactory UseD7Networks(this SmsServiceFactory builder, Action<D7NetworksSmsDeliveryChannelOptions> config)
    {
        // load the configuration
        var configuration = new D7NetworksSmsDeliveryChannelOptions();
        config(configuration);

        // validate the configuration
        configuration.Validate();

        // add the channel to the SMSs service factory
        builder.UseChannel(new D7NetworksSmsDeliveryChannel(null, configuration));

        return builder;
    }
}
