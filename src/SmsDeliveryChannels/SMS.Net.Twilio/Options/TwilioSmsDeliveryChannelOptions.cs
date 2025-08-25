namespace SMS.Net.Channel.Twilio;

/// <summary>
/// the options for configuring the Twilio SMS delivery channel
/// </summary>
public class TwilioSmsDeliveryChannelOptions
{
    /// <summary>
    /// Get or Set your Twilio account SID.
    /// </summary>
    public string? AccountSID { get; set; }

    /// <summary>
    /// Get or Set your Twilio auth token.
    /// </summary>
    public string? AuthToken { get; set; }

    /// <summary>
    /// validate if the options are all set correctly
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(AccountSID))
            throw new RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>(
                $"{nameof(AccountSID)}", "the given TwilioSmsDeliveryChannelOptions.AccountSID value is null or empty.");

        if (string.IsNullOrWhiteSpace(AuthToken))
            throw new RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>(
                $"{nameof(AuthToken)}", "the given TwilioSmsDeliveryChannelOptions.AuthToken value is null or empty.");
    }
}
