namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager implementation for <see cref="IRavenSmsMessagesManager"/>
/// </summary>
public partial class RavenSmsMessagesManager
{
    /// <inheritdoc/>
    public Task<(long totalSent, long totalFailed, long totalInQueue)> MessagesCountsAsync()
        => _messagesStore.MessagesCountsAsync();

    /// <inheritdoc/>
    public Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter)
        => _messagesStore.GetAllAsync(filter);

    /// <summary>
    /// check if the message already exist
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>true if exist, false if not</returns>
    public Task<bool> IsMessageExistAsync(string messageId)
        => _messagesStore.IsMessageExistAsync(messageId);

    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(string messageId)
        => _messagesStore.FindByIdAsync(messageId);

    /// <inheritdoc/>
    public Task<RavenSmsMessage[]> GetAllAsync()
        => _messagesStore.GetAllAsync();

    /// <inheritdoc/>
    public async Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message)
    {
        if (await _messagesStore.IsMessageExistAsync(message.Id))
            return await _messagesStore.UpdateAsync(message);

        return await _messagesStore.AddAsync(message);
    }

    /// <inheritdoc/>
    public Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message)
        => _messagesStore.UpdateAsync(message);
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