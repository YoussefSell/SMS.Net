namespace SMS.Net.Channel.RavenSMS.Enums;

/// <summary>
/// the status of the client.
/// </summary>
public enum RavenSmsClientStatus
{
    /// <summary>
    /// default status.
    /// </summary>
    None = -1,

    /// <summary>
    /// the message has been created.
    /// </summary>
    UnActive = 0,

    /// <summary>
    /// the message has been Queued.
    /// </summary>
    Active = 1,

    /// <summary>
    /// failed to send the message.
    /// </summary>
    Disconnected = 2,

    /// <summary>
    /// the client app require setup
    /// </summary>
    RequireSetup,
}
