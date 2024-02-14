namespace SMS.Net;

/// <summary>
/// the sms service used to abstract the sms sending
/// </summary>
public partial class SmsService
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
        => Send(message, _defaultProvider);

    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message, string channel_name)
    {
        // check if the provider name is valid
        if (channel_name is null)
            throw new ArgumentNullException(nameof(channel_name));

        // check if the provider exist
        if (!_providers.TryGetValue(channel_name, out ISmsDeliveryChannel provider))
            throw new SmsDeliveryChannelNotFoundException(channel_name);

        // send the sms message
        return Send(message, provider);
    }

    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message, ISmsDeliveryChannel channel)
    {
        // check if given params are not null.
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        if (channel is null)
            throw new ArgumentNullException(nameof(channel));

        // check if the from is null
        CheckMessageFromValue(message);

        // check if the sending is paused
        if (Options.PauseSending)
        {
            return SmsSendingResult.Success(channel.Name)
                .AddMetaData(SmsSendingResult.MetaDataKeys.SendingPaused, true);
        }

        // send the sms message
        return channel.Send(message);
    }

    /// <inheritdoc/>
    public Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        => SendAsync(message, _defaultProvider, cancellationToken);

    /// <inheritdoc/>
    public Task<SmsSendingResult> SendAsync(SmsMessage message, string channel_name, CancellationToken cancellationToken = default)
    {
        // check if the provider name is valid
        if (channel_name is null)
            throw new ArgumentNullException(nameof(channel_name));

        // check if the provider exist
        if (!_providers.TryGetValue(channel_name, out ISmsDeliveryChannel provider))
            throw new SmsDeliveryChannelNotFoundException(channel_name);

        // send the sms message
        return SendAsync(message, provider, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<SmsSendingResult> SendAsync(SmsMessage message, ISmsDeliveryChannel channel, CancellationToken cancellationToken = default)
    {
        // check if given params are not null.
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        if (channel is null)
            throw new ArgumentNullException(nameof(channel));

        // check if the from is null
        CheckMessageFromValue(message);

        // check if the sending is paused
        if (Options.PauseSending)
        {
            return Task.FromResult(SmsSendingResult.Success(channel.Name)
                .AddMetaData("sending_paused", true));
        }

        // send the sms message
        return channel.SendAsync(message, cancellationToken);
    }
}

/// <summary>
/// partial part for <see cref="SmsService"/>
/// </summary>
public partial class SmsService : ISmsService
{
    private readonly IDictionary<string, ISmsDeliveryChannel> _providers;
    private readonly ISmsDeliveryChannel _defaultProvider;

    /// <summary>
    /// create an instance of <see cref="SmsService"/>.
    /// </summary>
    /// <param name="channels">the list of supported SMS delivery channels.</param>
    /// <param name="options">the sms service options.</param>
    /// <exception cref="ArgumentNullException">if channels or options are null.</exception>
    /// <exception cref="ArgumentException">if channels list is empty.</exception>
    /// <exception cref="SmsDeliveryChannelNotFoundException">if the default SMS delivery channel cannot be found.</exception>
    public SmsService(IEnumerable<ISmsDeliveryChannel> channels, SmsServiceOptions options)
    {
        if (channels is null)
            throw new ArgumentNullException(nameof(channels));

        if (!channels.Any())
            throw new ArgumentException("you must specify at least one sms delivery channel, the list is empty.", nameof(channels));

        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // validate if the options are valid
        options.Validate();

        Options = options;

        // init the providers dictionary
        _providers = channels.ToDictionary(provider => provider.Name);

        // check if the default sms delivery channel exist
        if (!_providers.ContainsKey(options.DefaultDeliveryChannel!))
            throw new SmsDeliveryChannelNotFoundException(options.DefaultDeliveryChannel!);

        // set the default provider
        _defaultProvider = _providers[options.DefaultDeliveryChannel!];
    }

    /// <summary>
    /// Get the sms service options instance
    /// </summary>
    public SmsServiceOptions Options { get; }

    /// <summary>
    /// Get the list of sms delivery channels attached to this sms service.
    /// </summary>
    public IEnumerable<ISmsDeliveryChannel> Channels => _providers.Values;

    /// <summary>
    /// Get the default sms delivery channel attached to this sms service.
    /// </summary>
    public ISmsDeliveryChannel DefaultChannel => _defaultProvider;

    /// <summary>
    /// check if the message from value is supplied
    /// </summary>
    /// <param name="message">the message instance</param>
    private void CheckMessageFromValue(SmsMessage message)
    {
        if (message.From is null)
        {
            if (Options.DefaultFrom is null)
                throw new ArgumentException($"the {typeof(SmsMessage).FullName} [From] value is null, either supply a from value in the message, or set a default [From] value in {typeof(SmsServiceOptions).FullName}");

            message.SetFrom(Options.DefaultFrom);
        }
    }
}
