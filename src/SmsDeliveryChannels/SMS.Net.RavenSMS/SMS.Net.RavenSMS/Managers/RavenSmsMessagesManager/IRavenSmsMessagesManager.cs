namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing messages
/// </summary>
public interface IRavenSmsMessagesManager
{
    /// <summary>
    /// get the messages count, returns the total of messages sent, failed, and in the queue
    /// </summary>
    /// <returns>the messages count</returns>
    Task<(long totalSent, long totalFailed, long totalInQueue)> MessagesCountsAsync();

    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <returns>the list of messages and total count of rows</returns>
    Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter);

    /// <summary>
    /// check if the message already exist
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>true if exist, false if not</returns>
    Task<bool> IsMessageExistAsync(string messageId);

    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <returns>list of all messages</returns>
    Task<RavenSmsMessage[]> GetAllAsync();

    /// <summary>
    /// find the message with the given id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>the message, or null if not found</returns>
    Task<RavenSmsMessage?> FindByIdAsync(string messageId);

    /// <summary>
    /// the message exist an update will be performed, if not the message will be added.
    /// </summary>
    /// <param name="message">the message to be saved</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message);
}
