namespace SMS.Net.Channel.MessageBird
{
    using global::MessageBird;
    using global::MessageBird.Objects;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// the MessageBird client SMS delivery channel
    /// </summary>
    public partial class MessageBirdSmsDeliveryChannel : IMessageBirdSmsDeliveryChannel
    {
        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
        {
            try
            {
                // init the MessageBird client
                var client = CreateClient(message.ChannelData);

                // send the message
                var result = client.SendMessage(
                    message.From.ToString(),
                    message.Body,
                    new[] { long.Parse(message.To) }); 

                // create the client
                return BuildResultObject(result);
            }
            catch (Exception ex)
            {
                return SmsSendingResult.Failure(Name).AddError(ex);
            }
        }

        /// <inheritdoc/>
        public async Task<SmsSendingResult> SendAsync(SmsMessage message)
            => await Task.FromResult(Send(message));
    }

    /// <summary>
    /// partial part for <see cref="MessageBirdSmsDeliveryChannel"/>
    /// </summary>
    public partial class MessageBirdSmsDeliveryChannel
    {
        /// <summary>
        /// the name of the SMS delivery channel
        /// </summary>
        public const string Name = "MessageBird";

        /// <inheritdoc/>
        string ISmsChannel.Name => Name;

        private readonly MessageBirdSmsDeliveryChannelOptions _options;

        /// <summary>
        /// create an instance of <see cref="MessageBirdSmsDeliveryChannel"/>
        /// </summary>
        /// <param name="options">the edp options instance</param>
        /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
        public MessageBirdSmsDeliveryChannel(MessageBirdSmsDeliveryChannelOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // validate if the options are valid
            options.Validate();
            _options = options;
        }

        private Client CreateClient(IEnumerable<ChannelData> data)
        {
            // get the userName, password & accountSId from the data list if any.
            var accessKeyChannelData = data.GetData(CustomChannelData.AccessKey);

            // set the access key
            var accessKey = accessKeyChannelData.IsEmpty() ? _options.AccessKey : accessKeyChannelData.GetValue<string>();

            // create and return the client
            return Client.CreateDefault(accessKey);
        }

        private static SmsSendingResult BuildResultObject(Message result)
        {
            // there is no status of the delivery result, because they throw exception when there is a failure.
            return SmsSendingResult.Success(Name)
                .AddMetaData("message_id", result.Id)
                .AddMetaData("messageBird_response", result);
        }
    }
}