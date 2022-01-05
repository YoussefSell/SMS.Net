namespace SMS.Net.Channel.RavenSMS.Enums;

/// <summary>
/// the status of the message.
/// </summary>
public enum RavenSmsMessageStatus
{
    /// <summary>
    /// the message has been created.
    /// </summary>
    Created,

    /// <summary>
    /// the message has been Queued.
    /// </summary>
    Queued,

    /// <summary>
    /// failed to send the message.
    /// </summary>
    Failed,

    /// <summary>
    /// the message has been sent successfully.
    /// </summary>
    Sent,
}
