namespace SMS.Net.Channel.MessageBird
{
    using SMS.Net.Exceptions;

    /// <summary>
    /// the options for configuring the MessageBird SMS delivery channel
    /// </summary>
    public class MessageBirdSmsDeliveryChannelOptions
    {
        /// <summary>
        /// Get or Set your MessageBird access Key.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(AccessKey))
                throw new RequiredOptionValueNotSpecifiedException<MessageBirdSmsDeliveryChannelOptions>(
                    $"{nameof(AccessKey)}", "the given MessageBirdSmsDeliveryChannelOptions.AccessKey value is null or empty.");
        }
    }
}
