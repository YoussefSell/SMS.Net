namespace SMS.Net
{
    using SMS.Net.Channel.Twilio;
    using SMS.Net.Factories;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// the extensions methods over the <see cref="SmsMessageComposer"/> factory.
    /// </summary>
    public static class TwilioMessageComposerExtensions
    {
        /// <summary>
        /// pass a custom channel data to configure a custom userName to be used when initializing the Twilio client instead of using the one set in the <see cref="TwilioSmsDeliveryChannelOptions.Username"/>
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="userName">the userName to be used.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer UseUserName(this SmsMessageComposer messageComposer, string userName)
            => messageComposer.WithCustomData(CustomChannelData.Username, userName);

        /// <summary>
        /// pass a custom channel data to configure a custom password to be used when initializing the Twilio client instead of using the one set in the <see cref="TwilioSmsDeliveryChannelOptions.Password"/>
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="password">the password to be used.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer UsePassword(this SmsMessageComposer messageComposer, string password)
            => messageComposer.WithCustomData(CustomChannelData.Password, password);

        /// <summary>
        /// pass a custom channel data to configure a custom accountSID to be used when initializing the Twilio client instead of using the one set in the <see cref="TwilioSmsDeliveryChannelOptions.AccountSID"/>
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="accountSID">the accountSID to be used.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer UseAccountSID(this SmsMessageComposer messageComposer, string accountSID)
            => messageComposer.WithCustomData(CustomChannelData.AccountSID, accountSID);

        /// <summary>
        /// set The SID of the Account that will create the resource.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the PathAccountSid value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetPathAccountSid(this SmsMessageComposer messageComposer, string value)
            => messageComposer.WithCustomData(CustomChannelData.PathAccountSid, value);

        /// <summary>
        /// set The SID of the Messaging Service you want to associate with the message.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the MessagingServiceSid value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetMessagingServiceSid(this SmsMessageComposer messageComposer, string value)
            => messageComposer.WithCustomData(CustomChannelData.MessagingServiceSid, value);

        /// <summary>
        /// set The URL of the media to send with the message.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the MediaUrl value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetMediaUrl(this SmsMessageComposer messageComposer, List<Uri> value)
            => messageComposer.WithCustomData(CustomChannelData.MediaUrl, value);

        /// <summary>
        /// set The URL we should call to send status information to your application.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the StatusCallback value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetStatusCallback(this SmsMessageComposer messageComposer, Uri value)
            => messageComposer.WithCustomData(CustomChannelData.StatusCallback, value);

        /// <summary>
        /// The application to use for callbacks.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the ApplicationSid value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetApplicationSid(this SmsMessageComposer messageComposer, string value)
            => messageComposer.WithCustomData(CustomChannelData.ApplicationSid, value);

        /// <summary>
        /// The total maximum price up to 4 decimal places in US dollars acceptable for the message to be delivered.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the MaxPrice value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetMaxPrice(this SmsMessageComposer messageComposer, decimal value)
            => messageComposer.WithCustomData(CustomChannelData.MaxPrice, value);

        /// <summary>
        /// Whether to confirm delivery of the message.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the ProvideFeedback value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetProvideFeedback(this SmsMessageComposer messageComposer, bool value)
            => messageComposer.WithCustomData(CustomChannelData.ProvideFeedback, value);

        /// <summary>
        /// Total number of attempts made , this inclusive to send out the message.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the Attempt value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetAttempt(this SmsMessageComposer messageComposer, int value)
            => messageComposer.WithCustomData(CustomChannelData.Attempt, value);

        /// <summary>
        /// The number of seconds that the message can remain in our outgoing queue.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the ValidityPeriod value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetValidityPeriod(this SmsMessageComposer messageComposer, int value)
            => messageComposer.WithCustomData(CustomChannelData.ValidityPeriod, value);

        /// <summary>
        /// Reserved.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the ForceDelivery value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetForceDelivery(this SmsMessageComposer messageComposer, bool value)
            => messageComposer.WithCustomData(CustomChannelData.ForceDelivery, value);

        /// <summary>
        /// Whether to detect Unicode characters that have a similar GSM-7 character and replace them.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the SmartEncoded value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetSmartEncoded(this SmsMessageComposer messageComposer, bool value)
            => messageComposer.WithCustomData(CustomChannelData.SmartEncoded, value);

        /// <summary>
        /// Rich actions for Channels Messages.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the PersistentAction value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetPersistentAction(this SmsMessageComposer messageComposer, List<string> value)
            => messageComposer.WithCustomData(CustomChannelData.PersistentAction, value);

        /// <summary>
        /// If set to True, Twilio will deliver the message as a single MMS message, regardless of the presence of media.
        /// </summary>
        /// <param name="messageComposer">the message composer instance.</param>
        /// <param name="value">the SendAsMms value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public static SmsMessageComposer SetSendAsMms(this SmsMessageComposer messageComposer, bool value)
            => messageComposer.WithCustomData(CustomChannelData.SendAsMms, value);
    }
}
