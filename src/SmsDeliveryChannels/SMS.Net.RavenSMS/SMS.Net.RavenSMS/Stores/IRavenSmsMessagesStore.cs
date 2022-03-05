namespace SMS.Net.Channel.RavenSMS.Persistence;

/// <summary>
/// the repository for managing SMS messages data
/// </summary>
public interface IRavenSmsMessagesStore
{
    /// <summary>
    /// get the messages count, returns the total of messages sent, failed, and in the queue
    /// </summary>
    /// <returns>the messages count</returns>
    Task<(long totalSent, long totalFailed, long totalInQueue)> MessagesCountsAsync();

    /// <summary>
    /// get the list of all messages that match the given filter.
    /// </summary>
    /// <param name="filter">the ravenSMS message filter</param>
    /// <returns>the list of messages</returns>
    Task<(RavenSmsMessage[] data, int rowsCount)> GetAllAsync(RavenSmsMessageFilter filter);

    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <returns>list of all messages</returns>
    Task<RavenSmsMessage[]> GetAllAsync();

    /// <summary>
    /// check if the message exist by id
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>true if exist, false if not</returns>
    Task<bool> IsMessageExistAsync(string messageId);

    /// <summary>
    /// find the message with the given id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>the message, or null if not found</returns>
    Task<RavenSmsMessage?> FindByIdAsync(string messageId);

    /// <summary>
    /// save the given message.
    /// </summary>
    /// <param name="message">the message to be saved</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsMessage>> AddAsync(RavenSmsMessage message);

    /// <summary>
    /// update the given message.
    /// </summary>
    /// <param name="message">the message to be updated</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message);
}
