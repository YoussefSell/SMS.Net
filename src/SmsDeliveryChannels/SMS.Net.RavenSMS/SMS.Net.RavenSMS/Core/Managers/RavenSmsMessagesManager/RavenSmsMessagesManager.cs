namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager implementation for <see cref="IRavenSmsMessagesManager"/>
/// </summary>
public partial class RavenSmsMessagesManager
{
    /// <inheritdoc/>
    public Task<(long totalSent, long totalFailed, long totalInQueue)> MessagesCountsAsync(CancellationToken cancellationToken = default)
        => _messagesStore.GetCountsAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsMessage[]> GetAllMessagesAsync(CancellationToken cancellationToken = default)
        => _messagesStore.GetAllAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<RavenSmsMessage[]> GetAllMessagesAsync(string clientId, bool excludeMessagesInQueue = true, CancellationToken cancellationToken = default)
    {
        var (messages, _) = await GetAllMessagesAsync(new RavenSmsMessageFilter
        {
            IgnorePagination = true,
            Clients = new[] { clientId },
            ExcludeStatus = new[] { 
                RavenSmsMessageStatus.None, 
                RavenSmsMessageStatus.Queued, 
                RavenSmsMessageStatus.Created, 
            }
        },
        cancellationToken);

        return messages;
    }

    /// <inheritdoc/>
    public Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter, CancellationToken cancellationToken = default)
        => _messagesStore.GetAllAsync(filter, cancellationToken);

    /// <inheritdoc/>
    public Task<bool> AnyAsync(string messageId, CancellationToken cancellationToken = default)
        => _messagesStore.AnyAsync(messageId, cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(string messageId, CancellationToken cancellationToken = default)
        => _messagesStore.FindByIdAsync(messageId, cancellationToken);

    /// <inheritdoc/>
    public async Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        if (await _messagesStore.AnyAsync(message.Id, cancellationToken))
            return await _messagesStore.UpdateAsync(message, cancellationToken);

        return await _messagesStore.CreateAsync(message, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteAsync(string messageId, CancellationToken cancellationToken = default)
    {
        var message = await _messagesStore.FindByIdAsync(messageId, cancellationToken);
        if (message is null)
        {
            return Result.Failure()
                .WithMessage("Failed to delete the message, not found")
                .WithCode("message_not_found");
        }

        return await _messagesStore.DeleteAsync(message, cancellationToken);
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsMessagesManager"/>
/// </summary>
public partial class RavenSmsMessagesManager : IRavenSmsMessagesManager
{
    private readonly IRavenSmsMessagesStore _messagesStore;

    public RavenSmsMessagesManager(IRavenSmsMessagesStore messagesRepository)
    {
        _messagesStore = messagesRepository;
    }
}