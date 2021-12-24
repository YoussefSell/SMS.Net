namespace SMS.Net.Channel.Twilio
{
    /// <summary>
    /// custom channel data, this class holds the names of the keys
    /// </summary>
    internal static class CustomChannelData
    {
        public const string Username = "";
        public const string Password = "";
        public const string AccountSID = "";

        /// <summary>
        /// The SID of the Account that will create the resource
        /// </summary>
        public const string PathAccountSid = "path_account_sid";

        /// <summary>
        /// The SID of the Messaging Service you want to associate with the message.
        /// </summary>
        public const string MessagingServiceSid = "messaging_service_sid";

        /// <summary>
        /// The URL of the media to send with the message
        /// </summary>
        public const string MediaUrl = "media_url";

        /// <summary>
        /// The URL we should call to send status information to your application
        /// </summary>
        public const string StatusCallback = "status_callback";
        
        /// <summary>
        /// The application to use for callbacks
        /// </summary>
        public const string ApplicationSid = "application_sid";

        /// <summary>
        /// The total maximum price up to 4 decimal places in US dollars acceptable for the message to be delivered.
        /// </summary>
        public const string MaxPrice = "max_price";

        /// <summary>
        /// Whether to confirm delivery of the message
        /// </summary>
        public const string ProvideFeedback = "provide_feedback";

        /// <summary>
        /// Total number of attempts made , this inclusive to send out the message
        /// </summary>
        public const string Attempt = "attempt";

        /// <summary>
        /// The number of seconds that the message can remain in our outgoing queue.
        /// </summary>
        public const string ValidityPeriod = "validity_period";

        /// <summary>
        /// Reserved
        /// </summary>
        public const string ForceDelivery = "force_delivery";

        /// <summary>
        /// Whether to detect Unicode characters that have a similar GSM-7 character and replace them
        /// </summary>
        public const string SmartEncoded = "smart_encoded";

        /// <summary>
        /// Rich actions for Channels Messages.
        /// </summary>
        public const string PersistentAction = "persistent_action";

        /// <summary>
        /// If set to True, Twilio will deliver the message as a single MMS message, regardless of the presence of media
        /// </summary>
        public const string SendAsMms = "send_as_mms";
    }
}
