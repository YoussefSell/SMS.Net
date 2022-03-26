namespace SMS.Net.Channel.RavenSMS;

/// <summary>
/// the RavenSMS SMS delivery channel.
/// </summary>
public partial class RavenSmsDeliveryChannel : IRavenSmsDeliveryChannel
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
        => SendAsync(message).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            // if the message is null return failure
            if (message is null)
            {
                return SmsSendingResult.Failure(Name)
                    .AddError(new SmsSendingError(
                        code: "message_not_defined",
                        message: "the given message instance is null"));
            }

            // check if there is any client registered with the given "FROM" phone number
            var client = await _clientsManager.FindClientByPhoneNumberAsync(message.From, cancellationToken);
            if (client is null)
            {
                return SmsSendingResult.Failure(Name)
                    .AddError(new SmsSendingError(
                        code: "invalid_from",
                        message: "the given sender 'FROM' phone number is not registered with any client app"));
            }

            // create the raven SMS message
            var ravenSmsMessage = CreateMessage(message, client.Id);

            // get the delay data if any
            var delayData = message.ChannelData.GetData(CustomChannelData.Delay);
            if (delayData.IsEmpty())
            {
                // queue the message for delivery without delay
                var queuingResult = await _ravenSmsManager.QueueMessageAsync(ravenSmsMessage, cancellationToken);
                if (queuingResult.IsSuccess())
                {
                    // all done return success result
                    return SmsSendingResult.Success(Name);
                }

                // return the failure error
                return SmsSendingResult.Failure(Name)
                    .AddError(queuingResult);
            }

            // var get the delay value
            var delay = delayData.GetValue<TimeSpan>();

            // queue the message for delivery with a delay
            var queuingWithDelayResult = await _ravenSmsManager.QueueMessageAsync(ravenSmsMessage, delay, cancellationToken);
            if (queuingWithDelayResult.IsSuccess())
            {
                // all done return success result
                return SmsSendingResult.Success(Name);
            }

            // return the failure error
            return SmsSendingResult.Failure(Name)
                .AddError(queuingWithDelayResult);
        }
        catch (Exception ex)
        {
            return SmsSendingResult.Failure(Name)
                .AddError(ex);
        }
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsDeliveryChannel"/>
/// </summary>
public partial class RavenSmsDeliveryChannel
{
    /// <summary>
    /// the name of the SMS delivery channel
    /// </summary>
    public const string Name = "ravensms";

    /// <inheritdoc/>
    string ISmsDeliveryChannel.Name => Name;

    private readonly IRavenSmsManager _ravenSmsManager;
    private readonly IRavenSmsClientsManager _clientsManager;
    private readonly RavenSmsDeliveryChannelOptions _options;

    /// <summary>
    /// create an instance of <see cref="RavenSmsDeliveryChannel"/>
    /// </summary>
    /// <param name="ravenSmsManager">the <see cref="IRavenSmsManager"/> instance</param>
    /// <param name="clientsManager">the <see cref="IRavenSmsClientsManager"/> instance</param>
    /// <param name="options">the channel options instance</param>
    /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
    public RavenSmsDeliveryChannel(
        IRavenSmsManager ravenSmsManager,
        IRavenSmsClientsManager clientsManager,
        RavenSmsDeliveryChannelOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // validate if the options are valid
        options.Validate();
        _options = options;
        _clientsManager = clientsManager;
        _ravenSmsManager = ravenSmsManager;
    }

    /// <summary>
    /// create a <see cref="RavenSmsMessage"/> instance from the given <see cref="SmsMessage"/>.
    /// </summary>
    /// <param name="message">the <see cref="SmsMessage"/> instance</param>
    /// <param name="clientId">the clientId this message will be sent with.</param>
    /// <returns>an instance of <see cref="RavenSmsMessage"/> class</returns>
    public static RavenSmsMessage CreateMessage(SmsMessage message, string clientId)
        => new()
        {
            To = message.To,
            ClientId = clientId,
            Body = message.Body,
            Priority = message.Priority,
        };
}
