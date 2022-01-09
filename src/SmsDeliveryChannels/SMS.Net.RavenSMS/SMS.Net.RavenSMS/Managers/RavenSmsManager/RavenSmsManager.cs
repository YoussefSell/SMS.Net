namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS manager, used to manage all messages sent with RavenSMS. 
/// </summary>
public partial class RavenSmsManager : IRavenSmsManager
{
    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message)
    {
        // queue the message for future processing
        message.JobQueueId = await _queueManager.QueueMessageAsync(message);
        message.Status = RavenSmsMessageStatus.Queued;

        // save the message
        var saveResult = await _messagesRepository.SaveAsync(message);
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
        var saveResult = await _messagesRepository.SaveAsync(message);
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
    private readonly IRavenSmsMessagesStore _messagesRepository;

    public RavenSmsManager(
        IQueueManager queueManager,
        IRavenSmsMessagesStore messagesRepository)
    {
        _queueManager = queueManager;
        this._messagesRepository = messagesRepository;
    }
}
