namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber from) 
        => _context.Clients.AnyAsync(client => client.PhoneNumbers.Contains(from.ToString()));

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(Guid clientId)
        => _context.Clients.FirstOrDefaultAsync(client => client.Id == clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _context.Clients.FirstOrDefaultAsync(client => client.PhoneNumbers.Contains(phoneNumber.ToString()));

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
        => _context.Clients.ToArrayAsync();
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore
{
    private readonly IRavenSmsDbContext _context;

    public RavenSmsClientsStore(IRavenSmsDbContext context)
    {
        _context = context;
    }
}
