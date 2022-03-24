namespace SMS.Net.Channel.RavenSMS.Stores;

/// <summary>
/// the store for managing RavenSMS messages data.
/// </summary>
public interface IRavenSmsMessagesStore
{
    /// <summary>
    /// get the messages count, including the total of messages sent, failed, and in the queue.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the messages count</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<(long totalSent, long totalFailed, long totalInQueue)> GetCountsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of all messages.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>list of all messages</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsMessage[]> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of messages using a filter with total count of all entities that matches the filter for pagination.
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the list of messages</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<(RavenSmsMessage[] data, int rowsCount)> GetAllAsync(RavenSmsMessageFilter filter, CancellationToken cancellationToken = default);

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
    /// save the given message to the underlying store.
    /// </summary>
    /// <param name="message">the message instance to be saved</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsMessage>> CreateAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// update the given message in the store.
    /// </summary>
    /// <param name="message">the message instance to be updated</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// delete the message with the given id.
    /// </summary>
    /// <param name="message">the message to delete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> DeleteAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);
}
