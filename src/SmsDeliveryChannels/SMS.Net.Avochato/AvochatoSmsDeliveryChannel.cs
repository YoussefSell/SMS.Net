namespace SMS.Net.Channel.Avochato;

/// <summary>
/// the Avochato client SMS delivery channel
/// </summary>
public partial class AvochatoSmsDeliveryChannel : IAvochatoSmsDeliveryChannel
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
        => SendAsync(message).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            // init the Avochato client
            var client = CreateClient();

            // build HTTP request
            using var request = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(_options.BaseUrl) { Path = "v1/messages" }.Uri);

            // create the message & build the request json content
            using var jsonContent = new StringContent(
                JsonConvert.SerializeObject(CreateMessage(message)),
                Encoding.UTF8,
                "application/json");

            // attach the content to the request
            request.Content = jsonContent;

            // send the message
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            // create the client
            return BuildResultObject(response);
        }
        catch (Exception ex)
        {
            return SmsSendingResult.Failure(Name).AddError(ex);
        }
    }
}

/// <summary>
/// partial part for <see cref="AvochatoSmsDeliveryChannel"/>
/// </summary>
public partial class AvochatoSmsDeliveryChannel
{
    private readonly HttpClient _httpClient;
    private readonly AvochatoSmsDeliveryChannelOptions _options;

    /// <inheritdoc/>
    string ISmsDeliveryChannel.Name => Name;

    /// <summary>
    /// the name of the SMS delivery channel
    /// </summary>
    public const string Name = "Avochato";

    /// <summary>
    /// create an instance of <see cref="AvochatoSmsDeliveryChannel"/>
    /// </summary>
    /// <param name="httpClient">the httpClient instance</param>
    /// <param name="options">the options instance</param>
    /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
    public AvochatoSmsDeliveryChannel(HttpClient? httpClient, AvochatoSmsDeliveryChannelOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // validate if the options are valid
        options.Validate();
        _options = options;

        // if the HTTP client is null
        _httpClient = httpClient ?? new HttpClient();
    }

    private HttpClient CreateClient() => _httpClient;

    private static SmsSendingResult BuildResultObject(HttpResponseMessage result)
    {
        // there is no status of the delivery result, because they throw exception when there is a failure.
        return SmsSendingResult.Success(Name)
            .AddMetaData("Avochato_response", result);
    }

    /// <summary>
    /// create an instance of <see cref="AvochatoMessage"/> from the given <see cref="SmsMessage"/>.
    /// </summary>
    /// <param name="message">the message instance</param>
    /// <returns>instance of <see cref="AvochatoMessage"/></returns>
    public AvochatoMessage CreateMessage(SmsMessage message)
    {
        var option = new AvochatoMessage()
        {
            Phone = message.To,
            From = message.From,
            Message = message.Body,
            AuthId = _options.AuthId,
            AuthSecret = _options.AuthSecret,
        };

        SetCustomData(message, option);

        return option;

        static void SetCustomData(SmsMessage message, AvochatoMessage option)
        {
            if (message.ChannelData.TryGetData<string>(CustomChannelData.AuthId, out var authId))
                option.AuthId = authId;

            if (message.ChannelData.TryGetData<Uri>(CustomChannelData.MediaUrl, out var mediaUrl))
                option.MediaUrl = mediaUrl;

            if (message.ChannelData.TryGetData<List<string>>(CustomChannelData.Tags, out var tags))
                option.Tags = string.Join(",", tags);

            if (message.ChannelData.TryGetData<string>(CustomChannelData.AuthSecret, out var authSecret))
                option.AuthSecret = authSecret;

            if (message.ChannelData.TryGetData<bool>(CustomChannelData.MarkAddressed, out var markAddressed))
                option.MarkAddressed = markAddressed;

            if (message.ChannelData.TryGetData<Uri>(CustomChannelData.StatusCallback, out var statusCallback))
                option.StatusCallback = statusCallback;
        }
    }
}