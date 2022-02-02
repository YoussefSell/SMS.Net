﻿namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
        => _clients.Include(e => e.PhoneNumbers).ToArrayAsync();

    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber phoneNumber) 
        => _clients.AsNoTracking().Join(_context.Set<RavenSmsClientPhoneNumber>(), e => e.Id, e => e.ClientId, (client, phoneNumberData) => new { client, phoneNumberData })
            .AnyAsync(q => q.phoneNumberData.PhoneNumber == phoneNumber.ToString());

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(string clientId)
        => _clients.FirstOrDefaultAsync(client => client.Id == clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _clients.Join(_context.Set<RavenSmsClientPhoneNumber>(), e => e.Id, e => e.ClientId, (client, phoneNumberData) => new { client, phoneNumberData })
            .Where(q => q.phoneNumberData.PhoneNumber == phoneNumber.ToString())
            .Select(e => e.client)
            .FirstOrDefaultAsync();

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> SaveAsync(RavenSmsClient client)
    {
        try
        {
            var entity = _clients.Add(client);
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
            var entity = _clients.Update(client);
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
    private readonly DbSet<RavenSmsClient> _clients;

    public RavenSmsClientsStore(IRavenSmsDbContext context)
    {
        _context = context;
        _clients = _context.Set<RavenSmsClient>();
    }
}
