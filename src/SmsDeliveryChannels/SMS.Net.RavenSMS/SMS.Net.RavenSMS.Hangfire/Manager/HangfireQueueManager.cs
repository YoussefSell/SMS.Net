namespace SMS.Net.Channel.RavenSMS.Hangfire;

/// <summary>
/// the queue manager implementation using Hangfire
/// </summary>
public partial class HangfireQueueManager : IQueueManager
{
    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message) 
        => Task.FromResult(BackgroundJob.Enqueue<IRavenSmsManager>(manager => manager.ProcessAsync(message.Id)));

    /// <inheritdoc/>
    public Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay)
        => Task.FromResult(BackgroundJob.Schedule<IRavenSmsManager>(manager => manager.ProcessAsync(message.Id), delay));
}

/// <summary>
/// partial part for <see cref="HangfireQueueManager"/>
/// </summary>
public partial class HangfireQueueManager
{
}
