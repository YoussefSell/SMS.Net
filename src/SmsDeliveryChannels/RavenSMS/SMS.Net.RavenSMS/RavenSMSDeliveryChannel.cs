namespace SMS.Net.Channel.RavenSMS
{
    using SMS.Net.RavenSMS.Managers;
    using System;
    using System.Threading.Tasks;

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
                if (!await _clientsManager.AnyAsync(message.From))
                {
                    return SmsSendingResult.Failure(Name)
                        .AddError(new SmsSendingError(
                            code: "invalid_from",
                            message: "the given sender 'FROM' phone number is not registered with any client app"));
                }

                // create the raven SMS message
                var ravenSmsMessage = CreateMessage(message);

                // queue the message for delivery
                await _ravenSmsManager.QueueMessageAsync(ravenSmsMessage);

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
        string ISmsChannel.Name => Name;

        private readonly IRavenSmsManager _ravenSmsManager;
        private readonly IRavenSmsClientsManager _clientsManager;
        private readonly RavenSmsDeliveryChannelOptions _options;

        /// <summary>
        /// create an instance of <see cref="RavenSmsDeliveryChannel"/>
        /// </summary>
        /// <param name="options">the edp options instance</param>
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
            _ravenSmsManager = ravenSmsManager;
            _clientsManager = clientsManager;
        }

        /// <summary>
        /// create a <see cref="RavenSmsMessage"/> instance from the given <see cref="SmsMessage"/>.
        /// </summary>
        /// <param name="message">the <see cref="SmsMessage"/> instance</param>
        /// <returns>an instance of <see cref="RavenSmsMessage"/> class</returns>
        public static RavenSmsMessage CreateMessage(SmsMessage message)
        {
            var ravenSmsMessage = new RavenSmsMessage
            {
                To = message.To,
                Body = message.Body,
                From = message.From,
                Priority = message.Priority,
            };

            return ravenSmsMessage;
        }
    }
}