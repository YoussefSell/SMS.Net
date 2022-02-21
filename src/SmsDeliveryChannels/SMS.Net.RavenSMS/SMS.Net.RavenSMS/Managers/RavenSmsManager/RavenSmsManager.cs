namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS manager, used to manage all messages sent with RavenSMS. 
/// </summary>
public partial class RavenSmsManager : IRavenSmsManager
{
    /// <inheritdoc/>
    public async Task ProcessAsync(string messageId)
    {
        var message = await _messagesStore.FindByIdAsync(messageId);
        if (message is null)
            throw new RavenSmsMessageNotFoundException($"there is no message with the given Id {messageId}");

        // get the client associated with the given from number
        var client = await _clientsManagers.FindClientByPhoneNumberAsync(message.From);
        if (client is null)
        {
            // no client so we can do anything in here
            return;
        }

        if (client.Status != RavenSmsClientStatus.Connected)
        {
            // client is not connected
            return;
        }

        await _clientConnector.SendSmsMessageAsync(client, message);

        // add the logic for sending the message with the client
    }

    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message)
    {
        // queue the message for future processing
        message.JobQueueId = await _queueManager.QueueMessageAsync(message);
        message.Status = RavenSmsMessageStatus.Queued;

        // save the message
        var saveResult = await _messagesStore.SaveAsync(message);
        if (saveResult.IsFailure())
        {
            return Result.Failure()
                .WithMessage("failed to persist the message")
                .WithErrors(saveResult.Errors.ToArray());
        }

        // all done
        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay)
    {
        // queue the message for future processing
        message.JobQueueId = await _queueManager.QueueMessageAsync(message, delay);
        message.Status = RavenSmsMessageStatus.Queued;

        // save the message
        var saveResult = await _messagesStore.SaveAsync(message);
        if (saveResult.IsFailure())
        {
            return Result.Failure()
                .WithMessage("failed to persist the message")
                .WithErrors(saveResult.Errors.ToArray());
        }

        // all done
        return Result.Success();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsManager"/>
/// </summary>
public partial class RavenSmsManager
{
    private readonly IQueueManager _queueManager;
    private readonly IRavenSmsClientsStore _clientsStore;
    private readonly IRavenSmsMessagesStore _messagesStore;
    private readonly IRavenSmsClientConnector _clientConnector;
    private readonly IRavenSmsClientsManager _clientsManagers;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsClientsStore clientsStore,
        IRavenSmsClientsManager clientsManagers,
        IRavenSmsClientConnector clientConnector,
        IRavenSmsMessagesStore messagesRepository)
    {
        _queueManager = queueManager;
        _clientsStore = clientsStore;
        _clientsManagers = clientsManagers;
        this._clientConnector = clientConnector;
        _messagesStore = messagesRepository;
    }
}
