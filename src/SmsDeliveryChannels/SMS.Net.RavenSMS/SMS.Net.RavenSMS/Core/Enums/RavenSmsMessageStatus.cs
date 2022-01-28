namespace SMS.Net.Channel.RavenSMS.Enums;

/// <summary>
/// the status of the message.
/// </summary>
public enum RavenSmsMessageStatus
{
    /// <summary>
    /// default status.
    /// </summary>
    None = -1,

    /// <summary>
    /// the message has been created.
    /// </summary>
    Created = 0,

    /// <summary>
    /// the message has been Queued.
    /// </summary>
    Queued = 1,

    /// <summary>
    /// failed to send the message.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// the message has been sent successfully.
    /// </summary>
    Sent = 3,
}
