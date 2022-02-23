namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager implementation for <see cref="IRavenSmsMessagesManager"/>
/// </summary>
public partial class RavenSmsMessagesManager
{
    /// <inheritdoc/>
    public Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter)
        => _messagesStore.GetAllAsync(filter);

    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(string messageId)
        => _messagesStore.FindByIdAsync(messageId);

    /// <inheritdoc/>
    public Task<RavenSmsMessage[]> GetAllAsync()
        => _messagesStore.GetAllAsync();

    /// <inheritdoc/>
    public Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message)
        => _messagesStore.SaveAsync(message);

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