namespace SMS.Net;

/// <summary>
/// the options for configuring the SMS service
/// </summary>
public class SmsServiceOptions
{
    /// <summary>
    /// Get or set the name of the default SMS delivery channel.
    /// </summary>
    public string? DefaultDeliveryChannel { get; set; }

    /// <summary>
    /// Get or set the default phone number to be used as the "From" value.
    /// </summary>
    public PhoneNumber? DefaultFrom { get; set; }

    /// <summary>
    /// Get or set if the SMS sending is paused, if set to true no SMS message will be sent. by default is set to false.
    /// </summary>
    public bool PauseSending { get; set; }

    /// <summary>
    /// validate if the options are all set correctly
    /// </summary>
    /// <exception cref="RequiredOptionValueNotSpecifiedException{TOptions}">if the required options are not specified</exception>
    public void Validate()
    {
        if (!DefaultDeliveryChannel!.IsValid())
            throw new RequiredOptionValueNotSpecifiedException<SmsServiceOptions>(
                nameof(DefaultDeliveryChannel),
                "you must specify a valid delivery channel to be used as the default sending channel");
    }
}
