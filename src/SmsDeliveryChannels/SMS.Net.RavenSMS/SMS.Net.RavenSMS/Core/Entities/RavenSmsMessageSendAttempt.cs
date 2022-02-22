namespace SMS.Net.Channel.RavenSMS.Entities;

/// <summary>
/// this class used to hold info about a send attempt of a sms message
/// </summary>
public class RavenSmsMessageSendAttempt
{
    public RavenSmsMessageSendAttempt()
    {
        Date = DateTimeOffset.UtcNow;
        Errors = new List<ResultError>();
        Id = Generator.GenerateUniqueId("msa");
    }
    
    /// <summary>
    /// Get or set the id of the message.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// the date of the attempt
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Get or set the status of the message
    /// </summary>
    public SendAttemptStatus Status { get; set; }

    /// <summary>
    /// the list of errors associated with this attempt if any
    /// </summary>
    public ICollection<ResultError> Errors { get; set; }

    /// <summary>
    /// the id of the message
    /// </summary>
    public string MessageId { get; set; } = default!;

    /// <summary>
    /// the message
    /// </summary>
    public RavenSmsMessage Message { get; set; } = default!;
}
