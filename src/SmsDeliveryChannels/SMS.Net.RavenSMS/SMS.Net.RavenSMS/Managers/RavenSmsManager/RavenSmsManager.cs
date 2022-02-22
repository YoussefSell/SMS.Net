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
            throw new RavenSmsMessageSendingFailedException("client_not_found_by_phone");

        if (client.Status != RavenSmsClientStatus.Connected)
            throw new RavenSmsMessageSendingFailedException("client_not_connected");

        // send the SMS message command to the client
        var sendResult = await _clientConnector.SendSmsMessageAsync(client, message);

        // create an attempt recored & update message status
        var attempt = new RavenSmsMessageSendAttempt { Status = SendAttemptStatus.Sent };
        message.Status = RavenSmsMessageStatus.Sent;
        message.SendAttempts.Add(attempt);

        if (sendResult.IsFailure())
        {
            attempt.Status = SendAttemptStatus.Failed;
            message.Status = RavenSmsMessageStatus.Failed;

            attempt.Errors = new List<ResultError>(sendResult.Errors)
            {
                new ResultError(sendResult.Message, sendResult.Code),
            };
        }

        await _messagesStore.UpdateAsync(message);

        if (sendResult.IsFailure())
            throw new RavenSmsMessageSendingFailedException(sendResult.Code);
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
    private readonly IHubContext<RavenSmsHub> _clientConnector;
    private readonly IRavenSmsClientsManager _clientsManagers;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsClientsStore clientsStore,
        IRavenSmsClientsManager clientsManagers,
        IHubContext<RavenSmsHub> clientConnector,
        IRavenSmsMessagesStore messagesRepository)
    {
        _queueManager = queueManager;
        _clientsStore = clientsStore;
        _clientsManagers = clientsManagers;
        this._clientConnector = clientConnector;
        _messagesStore = messagesRepository;
    }
}
