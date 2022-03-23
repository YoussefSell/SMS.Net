namespace SMS.Net.Channel.RavenSMS.Queues.Coravel;

/// <summary>
/// the invocable payload
/// </summary>
public class InvocablePayload
{
    public InvocablePayload(string messageId)
        => MessageId = messageId;

    public InvocablePayload(string messageId, TimeSpan delay)
        : this(messageId) => Delay = delay;

    /// <summary>
    /// the message id
    /// </summary>
    public string MessageId { get; }

    /// <summary>
    /// the delay to wait before sending the message
    /// </summary>
    public TimeSpan? Delay { get; }

    internal static InvocablePayload Create(string messageId)
        => new(messageId);
    internal static InvocablePayload Create(string messageId, TimeSpan delay)
        => new(messageId, delay);
}