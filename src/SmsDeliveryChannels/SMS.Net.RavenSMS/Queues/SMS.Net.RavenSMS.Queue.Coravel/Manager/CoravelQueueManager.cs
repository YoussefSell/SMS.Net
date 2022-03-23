namespace SMS.Net.Channel.RavenSMS.Queues.Coravel;

/// <summary>
/// the queue manager implementation using Coravel
/// </summary>
public partial class CoravelQueueManager : IQueueManager
{
    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var queueId = _queue
            .QueueInvocableWithPayload<RavenSmsProcessSmsMessageInvocable, InvocablePayload>(
                InvocablePayload.Create(message.Id));

        return Task.FromResult(queueId.ToString());
    }

    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var queueId = _queue
            .QueueInvocableWithPayload<RavenSmsProcessSmsMessageInvocable, InvocablePayload>(
                InvocablePayload.Create(message.Id, delay));

        return Task.FromResult(queueId.ToString());
    }
}

/// <summary>
/// partial part for <see cref="CoravelQueueManager"/>
/// </summary>
public partial class CoravelQueueManager
{
    private readonly IQueue _queue;

    public CoravelQueueManager(IQueue queue)
    {
        this._queue = queue;
    }
}
