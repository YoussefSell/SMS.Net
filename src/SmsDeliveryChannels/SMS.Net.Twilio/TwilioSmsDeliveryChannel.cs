namespace SMS.Net.Channel.Twilio
{
    using global::Twilio;
    using global::Twilio.Rest.Api.V2010.Account;
    using SMS.Net.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        public async Task<SmsSendingResult> SendAsync(SmsMessage message)
        {
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
        /// <param name="options">the edp options instance</param>
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
            var userNameEdpData = data.GetData(CustomChannelData.Username);
            var passwordEdpData = data.GetData(CustomChannelData.Password);
            var accountSIdEdpData = data.GetData(CustomChannelData.AccountSID);

            var userName = userNameEdpData.IsEmpty() ? _options.Username : userNameEdpData.GetValue<string>();
            var password = passwordEdpData.IsEmpty() ? _options.Password : passwordEdpData.GetValue<string>();
            var accountSId = accountSIdEdpData.IsEmpty() ? _options.AccountSID : accountSIdEdpData.GetValue<string>();

            if (accountSId.IsValid())
            {
                TwilioClient.Init(userName, password, accountSId);
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
        /// create an instance of <see cref="BasicMessage"/> from the given <see cref="SmsMessage"/>.
        /// </summary>
        /// <param name="message">the message instance</param>
        /// <returns>instance of <see cref="BasicMessage"/></returns>
        public CreateMessageOptions CreateMessage(SmsMessage message)
        {
            var attemptChannelData = message.ChannelData.GetData(CustomChannelData.Attempt);
            var mediaUrlChannelData = message.ChannelData.GetData(CustomChannelData.MediaUrl);
            var maxPriceChannelData = message.ChannelData.GetData(CustomChannelData.MaxPrice);
            var sendAsMmsChannelData = message.ChannelData.GetData(CustomChannelData.SendAsMms);
            var smartEncodedChannelData = message.ChannelData.GetData(CustomChannelData.SmartEncoded);
            var forceDeliveryChannelData = message.ChannelData.GetData(CustomChannelData.ForceDelivery);
            var applicationSidChannelData = message.ChannelData.GetData(CustomChannelData.ApplicationSid);
            var statusCallbackChannelData = message.ChannelData.GetData(CustomChannelData.StatusCallback);
            var pathAccountSidChannelData = message.ChannelData.GetData(CustomChannelData.PathAccountSid);
            var validityPeriodChannelData = message.ChannelData.GetData(CustomChannelData.ValidityPeriod);
            var provideFeedbackChannelData = message.ChannelData.GetData(CustomChannelData.ProvideFeedback);
            var persistentActionChannelData = message.ChannelData.GetData(CustomChannelData.PersistentAction);
            var messagingServiceSidChannelData = message.ChannelData.GetData(CustomChannelData.MessagingServiceSid);

            var option = new CreateMessageOptions(new global::Twilio.Types.PhoneNumber(message.To))
            {
                Body = message.Body,
                From = new global::Twilio.Types.PhoneNumber(message.From),
            };

            if (!pathAccountSidChannelData.IsEmpty())
                option.PathAccountSid = pathAccountSidChannelData.GetValue<string>();

            if (!messagingServiceSidChannelData.IsEmpty())
                option.MessagingServiceSid = messagingServiceSidChannelData.GetValue<string>();

            if (!mediaUrlChannelData.IsEmpty())
                option.MediaUrl = mediaUrlChannelData.GetValue<List<Uri>>();

            if (!statusCallbackChannelData.IsEmpty())
                option.StatusCallback = statusCallbackChannelData.GetValue<Uri>();

            if (!applicationSidChannelData.IsEmpty())
                option.ApplicationSid = applicationSidChannelData.GetValue<string>();

            if (!maxPriceChannelData.IsEmpty())
                option.MaxPrice = maxPriceChannelData.GetValue<decimal>();

            if (!provideFeedbackChannelData.IsEmpty())
                option.ProvideFeedback = provideFeedbackChannelData.GetValue<bool>();

            if (!attemptChannelData.IsEmpty())
                option.Attempt = attemptChannelData.GetValue<int>();

            if (!validityPeriodChannelData.IsEmpty())
                option.ValidityPeriod = validityPeriodChannelData.GetValue<int>();

            if (!forceDeliveryChannelData.IsEmpty())
                option.ForceDelivery = forceDeliveryChannelData.GetValue<bool>();

            if (!smartEncodedChannelData.IsEmpty())
                option.SmartEncoded = smartEncodedChannelData.GetValue<bool>();

            if (!persistentActionChannelData.IsEmpty())
                option.PersistentAction = persistentActionChannelData.GetValue<List<string>>();

            if (!sendAsMmsChannelData.IsEmpty())
                option.SendAsMms = sendAsMmsChannelData.GetValue<bool>();

            return option;
        }
    }
}