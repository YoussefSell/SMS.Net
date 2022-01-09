namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing SMS messages.
/// </summary>
public interface IRavenSmsManager
{
    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue</param>
    /// <returns>a Task instance</returns>
    Task<Result> QueueMessageAsync(RavenSmsMessage message);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <returns>a Task instance</returns>
    Task<Result> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay);
}