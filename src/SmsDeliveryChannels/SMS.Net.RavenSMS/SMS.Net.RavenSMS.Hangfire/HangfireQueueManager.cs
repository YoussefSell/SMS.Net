namespace SMS.Net.Channel.RavenSMS.Hangfire;

/// <summary>
/// the queue manager implementation using Hangfire
/// </summary>
public partial class HangfireQueueManager : IQueueManager
{
    public Task<string> QueueMessageAsync(RavenSmsMessage message)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }

    public Task<string> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }
}

/// <summary>
/// partial part for <see cref="HangfireQueueManager"/>
/// </summary>
public partial class HangfireQueueManager
{
}
