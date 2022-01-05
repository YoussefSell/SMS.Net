namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing SMS messages.
/// </summary>
public interface IRavenSmsManager
{
    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="ravenSmsMessage">the message to queue</param>
    /// <returns>a Task instance</returns>
    Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="ravenSmsMessage">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <returns>a Task instance</returns>
    Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage, TimeSpan delay);
}