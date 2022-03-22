namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing SMS messages.
/// </summary>
public interface IRavenSmsManager
{
    /// <summary>
    /// Process the sending of the message.
    /// </summary>
    /// <param name="messageId">the id of the message to process.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>a Task instance</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task ProcessAsync(string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> QueueMessageAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay, CancellationToken cancellationToken = default);
}
