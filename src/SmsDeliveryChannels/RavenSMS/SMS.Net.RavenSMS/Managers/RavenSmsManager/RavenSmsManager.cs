namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS manager, used to manage all messages sent with RavenSMS. 
/// </summary>
public partial class RavenSmsManager : IRavenSmsManager
{
    public Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage)
    {



        throw new NotImplementedException();
    }

    public Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage, TimeSpan delay)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsManager"/>
/// </summary>
public partial class RavenSmsManager
{
    private readonly IQueueManager _queueManager;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsMessagesRepository messagesRepository)
    {
        _queueManager = queueManager;
    }
}
