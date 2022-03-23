namespace SMS.Net.Channel.RavenSMS.Domain.Models;

/// <summary>
/// defines a sending attempt error
/// </summary>
public struct RavenSmsMessageSendAttemptError
{
    /// <summary>
    /// the error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// the code associated with the exception
    /// </summary>
    public string Code { get; set; }
}
