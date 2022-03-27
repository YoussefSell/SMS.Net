namespace SMS.Net.Channel.RavenSMS.Domain.Enums;

/// <summary>
/// the send attempt status
/// </summary>
public enum SendAttemptStatus
{
    /// <summary>
    /// failed to send the message.
    /// </summary>
    Failed = 0,

    /// <summary>
    /// the message has been sent successfully.
    /// </summary>
    Sent = 1,
}