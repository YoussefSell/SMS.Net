namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the SMS Queue manager
/// </summary>
public interface IQueueManager
{
    /// <summary>
    /// queue the message for processing.
    /// </summary>
    /// <param name="message">the message to queue</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the if the background job Id generated the by the underlying queue manager.
    ///  </returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<string> QueueMessageAsync(RavenSmsMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// queue the message for processing with a delay
    /// </summary>
    /// <param name="message">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the if the background job Id generated the by the underlying queue manager.
    ///  </returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay, CancellationToken cancellationToken = default);
}