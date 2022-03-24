namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing messages
/// </summary>
public interface IRavenSmsMessagesManager
{
    /// <summary>
    /// get the messages count, including the total of messages sent, failed, and in the queue.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the messages count</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<(long totalSent, long totalFailed, long totalInQueue)> MessagesCountsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of all messages.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>list of all messages</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsMessage[]> GetAllMessagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of messages sent by the client with the given id
    /// </summary>
    /// <param name="clientId">the id of the client</param>
    /// <param name="excludeMessagesInQueue">true to exclude the messages in the queue, by default is set to true</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an array of <see cref="RavenSmsMessage"/></returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsMessage[]> GetAllMessagesAsync(string clientId, bool excludeMessagesInQueue = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of messages using a filter with total count of all entities that matches the filter for pagination.
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the list of messages</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// check if the message exist by id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>true if exist, false if not</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<bool> AnyAsync(string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the message with the given id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the message, or null if not found</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsMessage?> FindByIdAsync(string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// save the given message to the underlying store. if the message exist it will be update, if not, it will be inserted.
    /// </summary>
    /// <param name="message">the message instance to be saved</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// delete the message with the given id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> DeleteAsync(string messageId, CancellationToken cancellationToken = default);
}
