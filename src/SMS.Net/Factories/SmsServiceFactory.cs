namespace SMS.Net.Factories;

/// <summary>
/// the SMS service factory used to generate an instance of <see cref="SmsService"/> 
/// in case you not using DI, if you have access to DI use the Dependency injection integration instead.
/// </summary>
public partial class SmsServiceFactory
{
    private readonly SmsServiceOptions _options = new();
    private readonly HashSet<ISmsDeliveryChannel> _channels = [];

    private SmsServiceFactory() { }

    /// <summary>
    /// get an instance of the <see cref="SmsServiceFactory"/>
    /// </summary>
    public static readonly SmsServiceFactory Instance = new();

    /// <summary>
    /// set the options of the SMS service.
    /// </summary>
    /// <param name="options">the SMS option initializer.</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
    public SmsServiceFactory UseOptions(Action<SmsServiceOptions> options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // set the SMS options and validate it.
        options(_options);
        _options.Validate();

        return this;
    }

    /// <summary>
    /// set the <see cref="ISmsDeliveryChannel"/> to be used by the SMS service.
    /// </summary>
    /// <param name="channel">the <see cref="ISmsDeliveryChannel"/> instance</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
    public SmsServiceFactory UseChannel(ISmsDeliveryChannel channel)
    {
        if (channel is null)
            throw new ArgumentNullException(nameof(channel));

        _channels.Add(channel);

        return this;
    }

    /// <summary>
    /// create the SMS service instance.
    /// </summary>
    /// <returns>instance of <see cref="SmsService"/></returns>
    public ISmsService Create() => new SmsService(_channels, _options);
}
