namespace SMS.Net.Channel.RavenSMS.Queues.InMemory;

/// <summary>
/// the in memory queue implementation of the <see cref="IQueueManager"/>
/// </summary>
public partial class InMemoryQueueManager : IQueueManager
{
    private readonly IInMemoryQueue _queue;

    public InMemoryQueueManager(IInMemoryQueue queue)
        => _queue = queue;

    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, CancellationToken cancellationToken = default) =>
        Task.FromResult(_queue.Enqueue(message.Id));

    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay, CancellationToken cancellationToken = default)
        => Task.FromResult(_queue.Enqueue(message.Id, delay));
}
