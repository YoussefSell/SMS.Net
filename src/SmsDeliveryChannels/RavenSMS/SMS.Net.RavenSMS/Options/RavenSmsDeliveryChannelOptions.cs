namespace SMS.Net.Channel.RavenSMS
{
    using SMS.Net.Exceptions;

    /// <summary>
    /// the options for configuring the RavenSMS SMS delivery channel
    /// </summary>
    public class RavenSmsDeliveryChannelOptions
    {
        /// <summary>
        /// Get or Set your RavenSMS user name.
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// Get or Set your RavenSMS password.
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// Get or Set your RavenSMS password.
        /// </summary>
        public string? AccountSID { get; set; }

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Username))
                throw new RequiredOptionValueNotSpecifiedException<RavenSmsDeliveryChannelOptions>(
                    $"{nameof(Username)}", "the given RavenSMSSmsDeliveryChannelOptions.ApiKey value is null or empty.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new RequiredOptionValueNotSpecifiedException<RavenSmsDeliveryChannelOptions>(
                    $"{nameof(Password)}", "the given RavenSMSSmsDeliveryChannelOptions.ApiKey value is null or empty.");
        }
    }
}
