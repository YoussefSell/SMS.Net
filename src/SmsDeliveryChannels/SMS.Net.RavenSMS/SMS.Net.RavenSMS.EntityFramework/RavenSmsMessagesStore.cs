namespace SMS.Net.Channel.RavenSMS.Hangfire;

/// <summary>
/// the store implementation for <see cref="IRavenSmsMessagesStore"/>
/// </summary>
public partial class RavenSmsMessagesStore : IRavenSmsMessagesStore
{
    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(Guid messageId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SaveAsync(RavenSmsMessage message)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(RavenSmsMessage message)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsMessagesStore
{
    public RavenSmsMessagesStore()
    {

    }
}
