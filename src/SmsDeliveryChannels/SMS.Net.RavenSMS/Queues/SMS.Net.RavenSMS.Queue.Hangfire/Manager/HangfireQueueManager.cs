namespace SMS.Net.Channel.RavenSMS.Hangfire;

/// <summary>
/// the queue manager implementation using Hangfire
/// </summary>
public partial class HangfireQueueManager : IQueueManager
{
    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(BackgroundJob.Enqueue<IRavenSmsManager>(manager => manager.ProcessAsync(message.Id, default)));
    }

    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(BackgroundJob.Schedule<IRavenSmsManager>(manager => manager.ProcessAsync(message.Id, default), delay));
    }
}

/// <summary>
/// partial part for <see cref="HangfireQueueManager"/>
/// </summary>
public partial class HangfireQueueManager
{
}
