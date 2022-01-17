namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS manager, used to manage all messages sent with RavenSMS. 
/// </summary>
public partial class RavenSmsManager : IRavenSmsManager
{
    /// <inheritdoc/>
    public async Task ProcessAsync(Guid messageId)
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

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllClientsAsync()
        => _clientsRepository.GetAllAsync();

    /// <inheritdoc/>
    public Task<bool> AnyClientAsync(PhoneNumber from)
        => _clientsRepository.AnyAsync(from);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByIdAsync(Guid clientId)
        => _clientsRepository.FindByIdAsync(clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _clientsRepository.FindByPhoneNumberAsync(phoneNumber);
}

/// <summary>
/// partial part for <see cref="RavenSmsManager"/>
/// </summary>
public partial class RavenSmsManager
{
    private readonly IQueueManager _queueManager;
    private readonly IRavenSmsMessagesStore _messagesStore;
    private readonly IRavenSmsClientsStore _clientsRepository;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsClientsStore clientsStore,
        IRavenSmsMessagesStore messagesRepository)
    {
        _queueManager = queueManager;
        _clientsRepository = clientsStore;
        _messagesStore = messagesRepository;
    }
}
