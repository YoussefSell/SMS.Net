namespace SMS.Net.Channel.Avochato
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// the Avochato client SMS delivery channel
    /// </summary>
    public partial class AvochatoSmsDeliveryChannel : IAvochatoSmsDeliveryChannel
    {
        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<SmsSendingResult> SendAsync(SmsMessage message)
        {
            try
            {
                // init the Avochato client
                var client = CreateClient();

                // build http request
                using var request = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(_options.BaseUrl) { Path = "v1/messages" }.Uri);

                // create the message & build the request json content
                using var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(CreateMessage(message)),
                    Encoding.UTF8,
                    "application/json");

                // attach the content to the request
                request.Content = jsonContent;

                // send the message
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

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
        string ISmsChannel.Name => Name;

        /// <summary>
        /// the name of the SMS delivery channel
        /// </summary>
        public const string Name = "Avochato";

        /// <summary>
        /// create an instance of <see cref="AvochatoSmsDeliveryChannel"/>
        /// </summary>
        /// <param name="options">the edp options instance</param>
        /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
        public AvochatoSmsDeliveryChannel(HttpClient httpClient, AvochatoSmsDeliveryChannelOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // validate if the options are valid
            options.Validate();
            _options = options;

            // if the http client is null
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

            var tagsChannelData = message.ChannelData.GetData(CustomChannelData.Tags);
            var authIdChannelData = message.ChannelData.GetData(CustomChannelData.AuthKey);
            var mediaUrlChannelData = message.ChannelData.GetData(CustomChannelData.MediaUrl);
            var authSecretChannelData = message.ChannelData.GetData(CustomChannelData.AuthSecret);
            var markAddressedChannelData = message.ChannelData.GetData(CustomChannelData.MarkAddressed);
            var statusCallbackChannelData = message.ChannelData.GetData(CustomChannelData.StatusCallback);

            if (!authIdChannelData.IsEmpty())
                option.AuthId = authIdChannelData.GetValue<string>();

            if (!authSecretChannelData.IsEmpty())
                option.AuthSecret = authSecretChannelData.GetValue<string>();

            if (!markAddressedChannelData.IsEmpty())
                option.MarkAddressed = markAddressedChannelData.GetValue<bool>();

            if (!tagsChannelData.IsEmpty())
                option.Tags = string.Join(",", tagsChannelData.GetValue<List<string>>());

            if (!mediaUrlChannelData.IsEmpty())
                option.MediaUrl = mediaUrlChannelData.GetValue<Uri>();

            if (!statusCallbackChannelData.IsEmpty())
                option.StatusCallback = statusCallbackChannelData.GetValue<Uri>();

            return option;
        }
    }
}