namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber from) 
        => _context.RavenSmsClients.AnyAsync(client => client.PhoneNumbers.Contains(from.ToString()));

    /// <inheritdoc/>
    public Task<bool> ClientPhoneNumberExistAsync(string phoneNumber)
        => _context.RavenSmsClients.AnyAsync(client => client.PhoneNumbers.Contains(phoneNumber.ToString()));

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(string clientId)
        => _context.RavenSmsClients.FirstOrDefaultAsync(client => client.Id == clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _context.RavenSmsClients.FirstOrDefaultAsync(client => client.PhoneNumbers.Contains(phoneNumber.ToString()));

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
        => _context.RavenSmsClients.ToArrayAsync();

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> SaveAsync(RavenSmsClient client)
    {
        try
        {
            var entity = _context.RavenSmsClients.Add(client);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
        catch (Exception ex)
        {
            return Result.Failure<RavenSmsClient>()
                .WithMessage("Failed to save the client, an exception has been accrued")
                .WithErrors(ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> UpdateAsync(RavenSmsClient client)
    {
        try
        {
            var entity = _context.RavenSmsClients.Update(client);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
        catch (Exception ex)
        {
            return Result.Failure<RavenSmsClient>()
                .WithMessage("Failed to update the client, an exception has been accrued")
                .WithErrors(ex);
        }
    }
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
