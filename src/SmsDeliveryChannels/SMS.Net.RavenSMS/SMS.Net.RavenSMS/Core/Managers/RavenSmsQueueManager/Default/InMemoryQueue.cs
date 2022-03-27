namespace SMS.Net.Channel.RavenSMS.Queues.InMemory;

/// <summary>
/// partial part for <see cref="InMemoryQueueManager"/>
/// </summary>
public partial class InMemoryQueue : IInMemoryQueue
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new();
    private readonly ConcurrentQueue<InMemoryJob> _tasks = new();

    private readonly IServiceScopeFactory _scopeFactory;
    private int _queueIsConsuming = 0;
    private int _tasksRunningCount = 0;

    public bool IsRunning => _queueIsConsuming > 0;

    public InMemoryQueue(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <inheritdoc/>
    public string Enqueue(string messageId, TimeSpan? delay = null)
    {
        // create the job
        var job = new InMemoryJob(async () =>
        {
            // if we have a delay we need to wait
            if (delay.HasValue)
                await Task.Delay(delay.Value);

            // create the service scope
            using var scope = _scopeFactory.CreateScope();

            // get the manager to process the message
            var manager = scope.ServiceProvider.GetService<IRavenSmsManager>();
            if (manager is null)
                throw new RavenSmsException($"try to register RavenSMS with AddSMSNet().UseRavenSMS().");

            // process the message
            await manager.ProcessAsync(messageId);
        });

        // queue the job
        _tasks.Enqueue(job);

        return job.Id.ToString();
    }

    public async Task ConsumeQueueAsync()
    {
        try
        {
            Interlocked.Increment(ref _queueIsConsuming);

            var dequeuedTasks = DequeueAllTasks();
            var dequeuedGuids = dequeuedTasks.Select(job => job.Id);

            await Task.WhenAll(
                dequeuedTasks
                    .Select(InvokeTask)
                    .ToArray()
            );

            CleanTokens(dequeuedGuids);
        }
        finally
        {
            Interlocked.Decrement(ref _queueIsConsuming);
        }
    }

    public async Task ConsumeQueueOnShutdown()
    {
        foreach (var cancellatinTokens in _tokens.Values)
        {
            if (!cancellatinTokens.IsCancellationRequested)
            {
                cancellatinTokens.Cancel();
            }
        }

        await ConsumeQueueAsync();
    }

    private void CleanTokens(IEnumerable<Guid> guidsForTokensToClean)
    {
        foreach (var guid in guidsForTokensToClean)
        {
            if (_tokens.TryRemove(guid, out var token))
            {
                token.Dispose();
            }
        }
    }

    private List<InMemoryJob> DequeueAllTasks()
    {
        List<InMemoryJob> dequeuedTasks = new(_tasks.Count);

        while (_tasks.TryPeek(out var _))
        {
            _tasks.TryDequeue(out var dequeuedTask);

            if (dequeuedTask is not null)
            {
                dequeuedTasks.Add(dequeuedTask);
            }
        }

        return dequeuedTasks;
    }

    private async Task InvokeTask(InMemoryJob task)
    {
        try
        {
            Interlocked.Increment(ref _tasksRunningCount);
            await task.Invoke();
        }
        finally
        {
            Interlocked.Decrement(ref _tasksRunningCount);
        }
    }
}