namespace SMS.Net.Channel.RavenSMS.Entities;

/// <summary>
/// this class defines the phone number associated with a client
/// </summary>
public class RavenSmsClientPhoneNumber
{
    /// <summary>
    /// the phone numbers associated with this client
    /// </summary>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// the id of the client that owns this phone number
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// the client that owns this phone number
    /// </summary>
    public RavenSmsClient Client { get; set; } = default!;
}
