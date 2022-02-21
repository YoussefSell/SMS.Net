namespace SMS.Net.Channel.RavenSMS;

/// <summary>
/// the RavenSMS client SMS delivery channel
/// </summary>
public partial class RavenSmsDeliveryChannel : IRavenSmsDeliveryChannel
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
        => SendAsync(message).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public async Task<SmsSendingResult> SendAsync(SmsMessage message)
    {
        try
        {
            // check if there is any client registered with the given "FROM" phone number
            var client = await _clientsManager.FindClientByPhoneNumberAsync(message.From);
            if (client is not null)
            {
                return SmsSendingResult.Failure(Name)
                    .AddError(new SmsSendingError(
                        code: "invalid_from",
                        message: "the given sender 'FROM' phone number is not registered with any client app"));
            }

            // create the raven SMS message
            var ravenSmsMessage = CreateMessage(message);

            // get the delay data if any
            var delayData = message.ChannelData.GetData(CustomChannelData.Delay);
            if (delayData.IsEmpty())
            {
                // queue the message for delivery without delay
                await _ravenSmsManager.QueueMessageAsync(ravenSmsMessage);

                // all done return success result
                return SmsSendingResult.Success(Name);
            }

            // var get the delay value
            var delay = delayData.GetValue<TimeSpan>();

            // queue the message for delivery with a delay
            await _ravenSmsManager.QueueMessageAsync(ravenSmsMessage, delay);

            // all done return success result
            return SmsSendingResult.Success(Name);
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
    /// <param name="options">the EDP options instance</param>
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
    /// <returns>an instance of <see cref="RavenSmsMessage"/> class</returns>
    public static RavenSmsMessage CreateMessage(SmsMessage message) 
        => new()
        {
            To = message.To,
            Body = message.Body,
            From = message.From,
            Priority = message.Priority,
        };
}
