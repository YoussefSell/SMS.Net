namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// this class defines an in memory job definition
/// </summary>
internal class InMemoryJob
{
    private readonly Action? _job;
    private readonly Func<Task>? _asyncJob;

    /// <summary>
    /// the job Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// create an instance of the <see cref="InMemoryJob"/> from the given action
    /// </summary>
    /// <param name="action">the action instance</param>
    public InMemoryJob(Action action)
    {
        _job = action;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// create an instance of the <see cref="InMemoryJob"/> from the given function
    /// </summary>
    /// <param name="func">the function instance</param>
    public InMemoryJob(Func<Task> func)
    {
        _asyncJob = func;
        Id = Guid.NewGuid();
    }

    public async Task Invoke()
    {
        // check if it an Async task
        if (_asyncJob is not null)
        {
            await _asyncJob();
            return;
        }

        _job();
    }
}