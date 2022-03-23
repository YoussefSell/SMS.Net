namespace SMS.Net.Channel.RavenSMS.Domain.Models;

/// <summary>
/// a class that defines the RavenSMS Message
/// </summary>
public class RavenSmsMessage
{
    /// <summary>
    /// create an instance of <see cref="RavenSmsMessage"/>
    /// </summary>
    public RavenSmsMessage()
    {
        CreateOn = DateTimeOffset.Now;
        Status = RavenSmsMessageStatus.Created;
        Id = Generator.GenerateUniqueId("msg");
        SendAttempts = new List<RavenSmsMessageSendAttempt>();
    }

    /// <summary>
    /// Get or set the id of the message.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Get or set the date this message was create on.
    /// </summary>
    public DateTimeOffset CreateOn { get; set; }

    /// <summary>
    /// the date the message has been sent
    /// </summary>
    public DateTimeOffset? SentOn { get; set; }

    /// <summary>
    /// if the message has been queued, this will be the date & time the message will be sent
    /// </summary>
    public DateTimeOffset? DeliverAt { get; set; }

    /// <summary>
    /// Gets or sets the priority of this e-mail message.
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Get or set the message body.
    /// </summary>
    public string Body { get; set; } = default!;

    /// <summary>
    /// Get or set the phone numbers of recipients to send the SMS message to.
    /// </summary>
    public PhoneNumber To { get; set; } = default!;

    /// <summary>
    /// Get or set the id of the queue job associated with this message.
    /// </summary>
    public string? JobQueueId { get; set; }

    /// <summary>
    /// Get or set the status of the message
    /// </summary>
    public RavenSmsMessageStatus Status { get; set; }

    /// <summary>
    /// Get or set the id of the client used to send this message.
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Get or set the Client used to send this message.
    /// </summary>
    public RavenSmsClient? Client { get; set; }

    /// <summary>
    /// the list of sent attempts associated with this message
    /// </summary>
    public ICollection<RavenSmsMessageSendAttempt> SendAttempts { get; set; }

    /// <summary>
    /// create a new attempt with failed status, and added it to the list of attempts, also set the sentOn value, and set the message status to failed.
    /// </summary>
    /// <param name="code">the error code</param>
    /// <param name="message">the error message</param>
    internal void AddFailedAttempt(string code, string message)
    {
        var attempt = new RavenSmsMessageSendAttempt { Status = SendAttemptStatus.Failed };
        attempt.AddError(code, message);

        // update the message status
        SendAttempts.Add(attempt);
        SentOn = DateTimeOffset.UtcNow;
        Status = RavenSmsMessageStatus.Failed;
    }
}
