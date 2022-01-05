namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the SMS Queue manager
/// </summary>
public interface IQueueManager
{
    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue</param>
    /// <returns>the id of the queue job</returns>
    Task<string> QueueMessageAsync(RavenSmsMessage message);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <returns>a Task instance</returns>
    Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay);
}
