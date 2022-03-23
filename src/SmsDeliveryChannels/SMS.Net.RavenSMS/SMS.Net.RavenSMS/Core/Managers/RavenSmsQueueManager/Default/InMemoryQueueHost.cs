namespace SMS.Net.Channel.RavenSMS.Queues.InMemory;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

/// <summary>
/// the hosted service for managing the in memory queue execution
/// </summary>
internal partial class InMemoryQueueHost : IHostedService, IDisposable
{
    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        InitTimer();
        Consume();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _shutdown.Cancel();
        _timer?.Change(Timeout.Infinite, 0);

        await _queue.ConsumeQueueOnShutdown();

        if (_queue.IsRunning)
        {
            Console.WriteLine(@"
                the Queuing service is attempting to close but the queue is still running.
                App closing (in background) will be prevented until all tasks are completed.
            ");
        }

        while (_queue.IsRunning)
            await Task.Delay(50, cancellationToken);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _timer?.Dispose();

            _disposed = true;
        }
    }
}

/// <summary>
/// partial part for <see cref="InMemoryQueueHost"/>
/// </summary>
internal partial class InMemoryQueueHost : IHostedService, IDisposable
{
    private bool _disposed;

    private Timer? _timer;
    private readonly InMemoryQueue _queue;
    private readonly SemaphoreSlim _signal = new(0);
    private readonly CancellationTokenSource _shutdown = new();

    public InMemoryQueueHost(IInMemoryQueue queue)
    {
        _queue = queue as InMemoryQueue
            ?? throw new RavenSmsException("the inMemory Queue is not registered, call UseInMemoryQueue() on RavenSmsBuilder");
    }


    private void Consume() => Task.Run(ConsumeQueueAsync);

    private async Task ConsumeQueueAsync()
    {
        while (!_shutdown.IsCancellationRequested)
        {
            await _signal.WaitAsync(_shutdown.Token);
            await _queue.ConsumeQueueAsync();
        }
    }

    private void InitTimer()
    {
        _timer = new Timer(
            callback: (state) => _signal.Release(),
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromSeconds(10));
    }
}