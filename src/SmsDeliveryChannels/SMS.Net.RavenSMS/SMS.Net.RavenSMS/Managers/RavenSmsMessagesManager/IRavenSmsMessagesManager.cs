namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing messages
/// </summary>
public interface IRavenSmsMessagesManager
{
    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <returns>the list of messages and total count of rows</returns>
    Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter);

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
    /// save the given message.
    /// </summary>
    /// <param name="message">the message to be saved</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message);

    /// <summary>
    /// update the given message.
    /// </summary>
    /// <param name="message">the message to be updated</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message);
}
