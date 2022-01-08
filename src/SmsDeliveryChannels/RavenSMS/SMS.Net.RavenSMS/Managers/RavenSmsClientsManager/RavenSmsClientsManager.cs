namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS clients manager, used to manage all clients used to send the SMS messages.
/// </summary>
public partial class RavenSmsClientsManager : IRavenSmsClientsManager
{
    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
        => _clientsRepository.GetAllAsync();

    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber from) 
        => _clientsRepository.AnyAsync(from);
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsManager"/>
/// </summary>
public partial class RavenSmsClientsManager
{
    private readonly IRavenSmsClientsRepository _clientsRepository;

    public RavenSmsClientsManager(IRavenSmsClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }
}