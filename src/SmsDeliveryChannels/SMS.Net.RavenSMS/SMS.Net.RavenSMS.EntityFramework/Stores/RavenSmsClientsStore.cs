﻿namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public async Task<long> ClientsCountAsync()
        => await _clients.CountAsync();

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync()
        => _clients.ToArrayAsync();

    /// <inheritdoc/>
    public async Task<(RavenSmsClient[] data, int rowsCount)> GetAllAsync(RavenSmsClientsFilter filter)
    {
        var query = _clients.AsQueryable();

        // apply the filter & the orderBy
        query = SetFilter(query, filter);
        query = query.DynamicOrderBy(filter.OrderBy, filter.SortDirection);

        var rowsCount = 0;

        if (!filter.IgnorePagination)
        {
            rowsCount = await query.Select(e => e.Id)
                .Distinct()
                .CountAsync();

            query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        var data = await query.ToArrayAsync();

        rowsCount = filter.IgnorePagination
            ? data.Length
            : rowsCount;

        return (data, rowsCount);
    }

    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber phoneNumber) 
        => _clients.AsNoTracking().AnyAsync(q => q.PhoneNumber == phoneNumber.ToString());
    
    /// <inheritdoc/>
    public Task<bool> IsExistClientAsync(string clientId)
        => _clients.AnyAsync(c => c.Id == clientId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(string clientId)
        => _clients.FirstOrDefaultAsync(client => client.Id == clientId);
    
    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByConnectionIdAsync(string connectionId)
        => _clients.FirstOrDefaultAsync(client => client.ConnectionId == connectionId);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber)
        => _clients.Where(q => q.PhoneNumber == phoneNumber.ToString())
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

    /// <inheritdoc/>
    public async Task<Result> DeleteClientAsync(RavenSmsClient client)
    {
        try
        {
            var entity = _clients.Remove(client);
            await _context.SaveChangesAsync();
            return Result.Success();
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

    private static IQueryable<RavenSmsClient> SetFilter(IQueryable<RavenSmsClient> query, RavenSmsClientsFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchQuery))
            query = query.Where(e => 
                EF.Functions.Like(e.Description, $"%{filter.SearchQuery}%") ||
                EF.Functions.Like(e.Name, $"%{filter.SearchQuery}%")
            );

        if (filter.Status != RavenSmsClientStatus.None)
            query = query.Where(e => filter.Status == e.Status);

        if (filter.phoneNumbers is not null && filter.phoneNumbers.Any())
            query = query.Where(e => filter.phoneNumbers.Contains(e.PhoneNumber));

        return query;
    }
}
