namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsNetBuilder"/>.
/// </summary>
public static class SmsNetBuilderExtensions
{
    /// <summary>
    /// add the D7Networks channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsNetBuilder"/> instance.</param>
    /// <param name="apiKey">Set your D7Networks api-key.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseD7Networks(this SmsNetBuilder builder, string apiKey)
       => builder.UseD7Networks(op => { op.ApiKey = apiKey; });

    /// <summary>
    /// add the D7Networks channel to be used with your sms service.
    /// </summary>
    /// <param name="builder">the SmsNet builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseD7Networks(this SmsNetBuilder builder, Action<D7NetworksSmsDeliveryChannelOptions> config)
    {
        // load the configuration
        var configuration = new D7NetworksSmsDeliveryChannelOptions();
        config(configuration);

        // validate the configuration
        configuration.Validate();

        builder.Services.AddSingleton((s) => configuration);
        builder.Services.AddHttpClient<ISmsDeliveryChannel, D7NetworksSmsDeliveryChannel>();
        builder.Services.AddHttpClient<ID7NetworksSmsDeliveryChannel, D7NetworksSmsDeliveryChannel>();

        return builder;
    }
}
