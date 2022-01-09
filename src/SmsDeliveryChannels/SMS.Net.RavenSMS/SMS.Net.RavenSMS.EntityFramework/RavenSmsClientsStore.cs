namespace SMS.Net.Channel.RavenSMS.Hangfire;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber from)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore
{
    public RavenSmsClientsStore()
    {

    }
}
