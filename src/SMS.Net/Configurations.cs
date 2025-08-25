namespace SMS.Net;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// add the SMS.Net services and configuration. SmsNetBuilderExtensions
    /// </summary>
    /// <param name="serviceCollection">the service collection instant</param>
    /// <param name="defaultChannel">name of the default delivery channel to be used.</param>
    public static SmsNetBuilder AddSMSNet(this IServiceCollection serviceCollection, string defaultChannel)
        => AddSMSNet(serviceCollection, options => options.DefaultDeliveryChannel = defaultChannel);

    /// <summary>
    /// add the SMS.Net services and configuration.
    /// </summary>
    /// <param name="serviceCollection">the service collection instant</param>
    /// <param name="config">the configuration initializer.</param>
    public static SmsNetBuilder AddSMSNet(this IServiceCollection serviceCollection, Action<SmsServiceOptions> config)
    {
        if (config is null)
            throw new ArgumentNullException(nameof(config));

        // load the configuration
        var configuration = new SmsServiceOptions();
        config(configuration);

        serviceCollection.AddSingleton((s) => configuration);
        serviceCollection.AddScoped<ISmsService, SmsService>();

        // register the countries service
        return new SmsNetBuilder(serviceCollection, configuration);
    }
}
