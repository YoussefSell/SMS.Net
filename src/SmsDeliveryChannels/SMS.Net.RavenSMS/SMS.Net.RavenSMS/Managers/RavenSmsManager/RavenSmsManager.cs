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
        _ = await FindClientByPhoneNumberAsync(message.From);

        // add the logic for sending the message with the client
    }

    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message)
    {
        // queue the message for future processing
        // message.JobQueueId = await _queueManager.QueueMessageAsync(message);
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
        // message.JobQueueId = await _queueManager.QueueMessageAsync(message, delay);
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

    #region Clients management

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllClientsAsync()
        => _clientsStore.GetAllAsync();

    /// <inheritdoc/>
    public Task<(RavenSmsClient[] clients, int rowsCount)> GetAllClientsAsync(RavenSmsClientsFilter filter)
        => _clientsStore.GetAllAsync(filter);

    /// <inheritdoc/>
    public Task<bool> AnyClientAsync(PhoneNumber phoneNumber)
        => _clientsStore.AnyAsync(phoneNumber);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByIdAsync(string clientId)
        => _clientsStore.FindByIdAsync(clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _clientsStore.FindByPhoneNumberAsync(phoneNumber);

    /// <inheritdoc/>
    public Task<Result<RavenSmsClient>> CreateClientAsync(RavenSmsClient model)
        => _clientsStore.SaveAsync(model);

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> ClientConnectedAsync(RavenSmsClient client, string connectionId)
    {
        // set the client id
        client.ConnectionId = connectionId;
        client.Status = RavenSmsClientStatus.Connected;

        // attach the connection id to the client in database
        return await _clientsStore.UpdateAsync(client);
    }

    #endregion

    #region Messages management

    /// <inheritdoc/>
    public Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter) 
        => _messagesStore.GetAllAsync(filter);

    #endregion
}

/// <summary>
/// partial part for <see cref="RavenSmsManager"/>
/// </summary>
public partial class RavenSmsManager
{
    private readonly IQueueManager _queueManager;
    private readonly IRavenSmsClientsStore _clientsStore;
    private readonly IRavenSmsMessagesStore _messagesStore;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsClientsStore clientsStore,
        IRavenSmsMessagesStore messagesRepository)
    {
        _queueManager = queueManager;
        _clientsStore = clientsStore;
        _messagesStore = messagesRepository;
    }
}
