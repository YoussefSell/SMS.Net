namespace SMS.Net.Channel.D7Networks;

/// <summary>
/// the D7Networks client SMS delivery channel
/// </summary>
public partial class D7NetworksSmsDeliveryChannel : ID7NetworksSmsDeliveryChannel
{
    /// <inheritdoc/>
    public SmsSendingResult Send(SmsMessage message)
        => SendAsync(message).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            // init the D7Networks client
            var client = CreateClient();

            // build HTTP request
            using var request = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(_options.BaseUrl) { Path = "messages/v1/send" }.Uri);

            // create the message & build the request json content
            using var jsonContent = new StringContent(
                JsonConvert.SerializeObject(CreateMessage(message)),
                Encoding.UTF8,
                "application/json");

            // attach the content to the request
            request.Content = jsonContent;

            // add the api-key
            request.Headers.Add("Authorization", $"Bearer {_options.ApiKey}");

            // send the message
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            // create the client
            return await BuildResultObjectAsync(response);
        }
        catch (Exception ex)
        {
            return SmsSendingResult.Failure(Name).AddError(ex);
        }
    }
}

/// <summary>
/// partial part for <see cref="D7NetworksSmsDeliveryChannel"/>
/// </summary>
public partial class D7NetworksSmsDeliveryChannel
{
    private readonly HttpClient _httpClient;
    private readonly D7NetworksSmsDeliveryChannelOptions _options;

    /// <inheritdoc/>
    string ISmsDeliveryChannel.Name => Name;

    /// <summary>
    /// the name of the SMS delivery channel
    /// </summary>
    public const string Name = "D7Networks";

    /// <summary>
    /// create an instance of <see cref="D7NetworksSmsDeliveryChannel"/>
    /// </summary>
    /// <param name="httpClient">the httpClient instance</param>
    /// <param name="options">the options instance</param>
    /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
    public D7NetworksSmsDeliveryChannel(HttpClient? httpClient, D7NetworksSmsDeliveryChannelOptions options)
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

    private static async Task<SmsSendingResult> BuildResultObjectAsync(HttpResponseMessage result)
    {
        var responseContent = await result.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<D7NetworksSendMessageResponse>(responseContent);

        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return SmsSendingResult.Success(Name)
                .AddMetaData("D7Networks_response", response!);
        }

        return SmsSendingResult.Failure(Name)
            .AddMetaData("D7Networks_response", response!);
    }

    /// <summary>
    /// create an instance of <see cref="D7NetworksMessage"/> from the given <see cref="SmsMessage"/>.
    /// </summary>
    /// <param name="message">the message instance</param>
    /// <returns>instance of <see cref="D7NetworksMessage"/></returns>
    public D7NetworksMessage CreateMessage(SmsMessage message)
    {
        var option = new D7NetworksMessage()
        {
            Messages = 
            [
                new Message
                {
                    Recipients = [message.To],
                    Content = message.Body,
                }
            ],
            MessageGlobals = new MessageGlobals(),
        };

        SetCustomData(message, option.Messages[0], option.MessageGlobals);

        return option;

        static void SetCustomData(SmsMessage message, Message option, MessageGlobals globalOptions)
        {
            if (message.ChannelData.TryGetData<Uri>(CustomChannelData.ReportUrl, out var reportUrl))
                globalOptions.ReportUrl = reportUrl;

            if (message.ChannelData.TryGetData<string>(CustomChannelData.Originator, out var originator))
                globalOptions.Originator = originator;
            
            if (message.ChannelData.TryGetData<DataCoding>(CustomChannelData.DataCoding, out var dataCoding))
            {
                option.DataCoding = dataCoding;
                globalOptions.DataCoding = dataCoding;
            }

            if (message.ChannelData.TryGetData<DateTime>(CustomChannelData.ScheduleTime, out var scheduleTime))
                globalOptions.ScheduleTime = scheduleTime.ToString("yyyy-MM-ddTHH:mmzzz");

            if (message.ChannelData.TryGetData<MessageType>(CustomChannelData.MessageType, out var messageType))
                option.MessageType = messageType;
        }
    }
}