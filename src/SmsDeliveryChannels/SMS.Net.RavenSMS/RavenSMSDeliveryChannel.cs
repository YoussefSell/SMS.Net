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

            // create the raven SMS message
            var ravenSmsMessage = CreateMessage(message);

            // get the delay data if any
            var delayData = message.ChannelData.GetData(CustomChannelData.Delay);
            var delay = delayData.IsEmpty() ? TimeSpan.Zero : delayData.GetValue<TimeSpan>();

            // queue the message for delivery with a delay
            var queuingWithDelayResult = await _service.SendAsync(ravenSmsMessage, delay, cancellationToken);
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

    private readonly IRavenSmsService _service;

    /// <summary>
    /// create an instance of <see cref="RavenSmsDeliveryChannel"/>
    /// </summary>
    /// <param name="service">the <see cref="IRavenSmsService"/> instance</param>
    /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
    public RavenSmsDeliveryChannel(IRavenSmsService service)
    {
        _service = service;
    }

    /// <summary>
    /// create a <see cref="RavenSmsMessage"/> instance from the given <see cref="SmsMessage"/>.
    /// </summary>
    /// <param name="message">the <see cref="SmsMessage"/> instance</param>
    /// <param name="clientId">the clientId this message will be sent with.</param>
    /// <returns>an instance of <see cref="RavenSmsMessage"/> class</returns>
    public static Message CreateMessage(SmsMessage message)
        => new(message.Body, message.From, message.To, (global::RavenSMS.Priority)((int)message.Priority));
}
