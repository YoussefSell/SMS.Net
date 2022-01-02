namespace SMS.Net.Channel.RavenSMS
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// the RavenSMS client SMS delivery channel
    /// </summary>
    public partial class RavenSmsDeliveryChannel : IRavenSmsDeliveryChannel
    {
        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message)
        {
            var ravenSmsMessage = CreateMessage(message);



            throw new NotImplementedException();
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
        string ISmsChannel.Name => Name;

        private readonly RavenSmsDeliveryChannelOptions _options;

        /// <summary>
        /// create an instance of <see cref="RavenSmsDeliveryChannel"/>
        /// </summary>
        /// <param name="options">the edp options instance</param>
        /// <exception cref="ArgumentNullException">if the given provider options is null</exception>
        public RavenSmsDeliveryChannel(RavenSmsDeliveryChannelOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // validate if the options are valid
            options.Validate();
            _options = options;
        }

        /// <summary>
        /// create a <see cref="RavenSmsMessage"/> instance from the given <see cref="SmsMessage"/>.
        /// </summary>
        /// <param name="message">the <see cref="SmsMessage"/> instance</param>
        /// <returns>an instance of <see cref="RavenSmsMessage"/> class</returns>
        public RavenSmsMessage CreateMessage(SmsMessage message)
        {
            var ravenSmsMessage = new RavenSmsMessage
            {
                To = message.To,
                Body = message.Body,
                Priority = message.Priority,
            };

            return ravenSmsMessage;
        }
    }
}