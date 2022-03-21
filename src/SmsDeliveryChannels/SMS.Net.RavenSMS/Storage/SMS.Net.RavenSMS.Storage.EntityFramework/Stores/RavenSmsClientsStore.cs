namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsClientsStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => await _clients.CountAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync(CancellationToken cancellationToken = default)
        => _clients.ToArrayAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<(RavenSmsClient[] data, int rowsCount)> GetAllAsync(RavenSmsClientsFilter filter, CancellationToken cancellationToken = default)
    {
        if (filter is null)
            throw new ArgumentNullException(nameof(filter));

        var query = _clients.AsQueryable();

        // apply the filter & the orderBy
        query = SetFilter(query, filter);
        query = query.DynamicOrderBy(filter.OrderBy, filter.SortDirection);

        var rowsCount = 0;

        if (!filter.IgnorePagination)
        {
            rowsCount = await query.Select(e => e.Id)
                .Distinct()
                .CountAsync(cancellationToken: cancellationToken);

            query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        var data = await query.ToArrayAsync(cancellationToken: cancellationToken);

        rowsCount = filter.IgnorePagination
            ? data.Length : rowsCount;

        return (data, rowsCount);
    }

    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default) 
        => _clients.AsNoTracking().AnyAsync(q => q.PhoneNumber == phoneNumber.ToString(), cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task<bool> AnyAsync(string clientId, CancellationToken cancellationToken = default)
        => _clients.AsNoTracking().AnyAsync(q => q.Id == clientId, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(string clientId, CancellationToken cancellationToken = default)
        => _clients.FirstOrDefaultAsync(client => client.Id == clientId, cancellationToken: cancellationToken);
    
    /// <inheritdoc/>
    public Task<RavenSmsClient[]> FindByIdAsync(string[] clientsIds, CancellationToken cancellationToken = default)
        => _clients.Where(client => clientsIds.Contains(client.Id)).ToArrayAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByConnectionIdAsync(string connectionId, CancellationToken cancellationToken = default)
        => _clients.FirstOrDefaultAsync(client => client.ConnectionId == connectionId, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
        => _clients.Where(q => q.PhoneNumber == phoneNumber.ToString())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> CreateAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = _clients.Add(client);
            await _context.SaveChangesAsync(cancellationToken);
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
    public async Task<Result<RavenSmsClient>> UpdateAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = _clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
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
    public async Task<Result> DeleteAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        try
        {
            _ = _clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
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
