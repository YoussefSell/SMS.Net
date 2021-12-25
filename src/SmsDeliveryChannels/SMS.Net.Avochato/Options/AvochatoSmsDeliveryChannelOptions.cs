namespace SMS.Net.Channel.Avochato
{
    using SMS.Net.Exceptions;

    /// <summary>
    /// the options for configuring the Avochato SMS delivery channel
    /// </summary>
    public class AvochatoSmsDeliveryChannelOptions
    {
        /// <summary>
        /// Get or Set your Avochato authentication key.
        /// </summary>
        public string AuthId { get; set; }
        
        /// <summary>
        /// Get or Set your Avochato authentication secret.
        /// </summary>
        public string AuthSecret { get; set; }

        /// <summary>
        /// the base url to the Avochato service, default value is: 'https://www.avochato.com'
        /// </summary>
        public string BaseUrl { get; set; } = "https://www.avochato.com";

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(AuthId))
                throw new RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>(
                    $"{nameof(AuthId)}", "the given AvochatoSmsDeliveryChannelOptions.AuthId value is null or empty.");

            if (string.IsNullOrWhiteSpace(AuthSecret))
                throw new RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>(
                    $"{nameof(AuthSecret)}", "the given AvochatoSmsDeliveryChannelOptions.AuthSecret value is null or empty.");
        }
    }
}
