namespace SMS.Net.Channel.Twilio;

using global::Twilio;
using global::Twilio.Rest.Api.V2010.Account;

/// <summary>
/// the Twilio client SMS delivery channel
/// </summary>
public partial class TwilioSmsDeliveryChannel : ITwilioSmsDeliveryChannel
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
    {
        try
        {
            // init the Twilio client
            CreateClient(message.ChannelData);

            // create the basic message
            var twilioMessage = CreateMessage(message);

            // send the message
            var result = MessageResource.Create(twilioMessage);

            // create the client
            return BuildResultObject(result);
        }
        catch (Exception ex)
        {
            return SmsSendingResult.Failure(Name).AddError(ex);
        }
    }

    /// <inheritdoc/>
    public async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        try
        {
            // init the Twilio client
            CreateClient(message.ChannelData);

            // create the basic message
            var twilioMessage = CreateMessage(message);

            // send the message
            var result = await MessageResource.CreateAsync(twilioMessage);

            // create the client
            return BuildResultObject(result);
        }
        catch (Exception ex)
        {
            return SmsSendingResult.Failure(Name).AddError(ex);
        }
    }
}

/// <summary>
/// partial part for <see cref="TwilioSmsDeliveryChannel"/>
/// </summary>
public partial class TwilioSmsDeliveryChannel
{
    /// <summary>
    /// the name of the SMS delivery channel
    /// </summary>
    public const string Name = "twilio";

    /// <inheritdoc/>
    string ISmsDeliveryChannel.Name => Name;

    private readonly TwilioSmsDeliveryChannelOptions _options;

    /// <summary>
    /// create an instance of <see cref="TwilioSmsDeliveryChannel"/>
    /// </summary>
    /// <param name="options">the options instance</param>
    /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
    public TwilioSmsDeliveryChannel(TwilioSmsDeliveryChannelOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // validate if the options are valid
        options.Validate();
        _options = options;
    }

    private void CreateClient(IEnumerable<ChannelData> data)
    {
        // get the userName, password & accountSId from the data list if any.
        var userName = data.GetData(CustomChannelData.Username, @default: _options.Username);
        var password = data.GetData(CustomChannelData.Password, @default: _options.Password);
        var accountSid = data.GetData(CustomChannelData.AccountSID, @default: _options.AccountSID);

        if (!string.IsNullOrEmpty(accountSid) && !string.IsNullOrWhiteSpace(accountSid))
        {
            TwilioClient.Init(userName, password, accountSid);
            return;
        }

        TwilioClient.Init(userName, password);
    }

    private static SmsSendingResult BuildResultObject(MessageResource result)
    {
        // we will assume if the status != 'Failed', then it success
        if (result.Status != MessageResource.StatusEnum.Failed)
        {
            // return the result
            return SmsSendingResult.Success(Name)
                .AddMetaData("message_id", result.Sid)
                .AddMetaData("twilio_response", result);
        }

        // create the failure result & return the result
        return SmsSendingResult.Failure(Name)
            .AddMetaData("twilio_response", result);
    }

    /// <summary>
    /// create an instance of <see cref="CreateMessageOptions"/> from the given <see cref="SmsMessage"/>.
    /// </summary>
    /// <param name="message">the message instance</param>
    /// <returns>instance of <see cref="CreateMessageOptions"/></returns>
    public CreateMessageOptions CreateMessage(SmsMessage message)
    {
        var option = new CreateMessageOptions(new global::Twilio.Types.PhoneNumber(message.To))
        {
            Body = message.Body,
            From = new global::Twilio.Types.PhoneNumber(message.From),
        };

        SetCustomData(message, option);

        return option;

        static void SetCustomData(SmsMessage message, CreateMessageOptions option)
        {
            if (message.ChannelData.TryGetData<int>(CustomChannelData.Attempt, out var attempt))
                option.Attempt = attempt;

            if (message.ChannelData.TryGetData<List<Uri>>(CustomChannelData.MediaUrl, out var mediaUrl))
                option.MediaUrl = mediaUrl;

            if (message.ChannelData.TryGetData<decimal>(CustomChannelData.MaxPrice, out var maxPrice))
                option.MaxPrice = maxPrice;

            if (message.ChannelData.TryGetData<bool>(CustomChannelData.SendAsMMS, out var sendAsMms))
                option.SendAsMms = sendAsMms;

            if (message.ChannelData.TryGetData<bool>(CustomChannelData.SmartEncoded, out var smartEncoded))
                option.SmartEncoded = smartEncoded;

            if (message.ChannelData.TryGetData<bool>(CustomChannelData.ForceDelivery, out var forceDelivery))
                option.ForceDelivery = forceDelivery;

            if (message.ChannelData.TryGetData<string>(CustomChannelData.ApplicationSid, out var applicationSid))
                option.ApplicationSid = applicationSid;

            if (message.ChannelData.TryGetData<Uri>(CustomChannelData.StatusCallback, out var statusCallback))
                option.StatusCallback = statusCallback;

            if (message.ChannelData.TryGetData<string>(CustomChannelData.PathAccountSid, out var pathAccountSid))
                option.PathAccountSid = pathAccountSid;

            if (message.ChannelData.TryGetData<int>(CustomChannelData.ValidityPeriod, out var validityPeriod))
                option.ValidityPeriod = validityPeriod;

            if (message.ChannelData.TryGetData<bool>(CustomChannelData.ProvideFeedback, out var provideFeedback))
                option.ProvideFeedback = provideFeedback;

            if (message.ChannelData.TryGetData<List<string>>(CustomChannelData.PersistentAction, out var persistentAction))
                option.PersistentAction = persistentAction;

            if (message.ChannelData.TryGetData<string>(CustomChannelData.MessagingServiceSid, out var messagingServiceSid))
                option.MessagingServiceSid = messagingServiceSid;
        }
    }
}