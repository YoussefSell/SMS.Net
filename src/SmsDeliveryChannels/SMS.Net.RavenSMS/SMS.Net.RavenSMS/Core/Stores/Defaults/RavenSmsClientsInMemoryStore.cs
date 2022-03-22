namespace SMS.Net.Channel.RavenSMS.Stores.InMemory;

/// <summary>
/// the default implementation for <see cref="IRavenSmsClientsStore"/> with an in memory store
/// </summary>
public partial class RavenSmsClientsInMemoryStore : IRavenSmsClientsStore
{
    /// <inheritdoc/>
    public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.LongCount());
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.ToArray());
    }

    /// <inheritdoc/>
    public async Task<(RavenSmsClient[] data, int rowsCount)> GetAllAsync(RavenSmsClientsFilter filter, CancellationToken cancellationToken = default)
    {
        if (filter is null)
            throw new ArgumentNullException(nameof(filter));

        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        // apply the filter & the orderBy
        var query = SetFilter(_clients, filter);

        var rowsCount = 0;

        if (!filter.IgnorePagination)
        {
            rowsCount = query.Select(e => e.Id)
                .Distinct()
                .Count();

            query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        var data = query.ToArray();

        rowsCount = filter.IgnorePagination
            ? data.Length : rowsCount;

        return (data, rowsCount);
    }

    /// <inheritdoc/>
    public Task<bool> AnyAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.Any(q => q.PhoneNumber == phoneNumber.ToString()));
    }

    /// <inheritdoc/>
    public Task<bool> AnyAsync(string clientId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.Any(q => q.Id == clientId));
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.FirstOrDefault(client => client.Id == clientId));
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient[]> FindByIdAsync(string[] clientsIds, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.Where(client => clientsIds.Contains(client.Id)).ToArray());
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByConnectionIdAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(_clients.FirstOrDefault(client => client.ConnectionId == connectionId));
    }

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_clients.FirstOrDefault(q => q.PhoneNumber == phoneNumber.ToString()));
    }

    /// <inheritdoc/>
    public Task<Result<RavenSmsClient>> CreateAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        _clients.Add(client);

        return Task.FromResult(Result.Success(client));
    }

    /// <inheritdoc/>
    public Task<Result<RavenSmsClient>> UpdateAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var clientToUpdate = _clients.FirstOrDefault(c => c.Id == client.Id);
        if (clientToUpdate == null)
        {
            return Task.FromResult(Result.Failure<RavenSmsClient>()
                .WithMessage("Failed to update the client, not found")
                .WithCode("client_not_found"));
        }

        return Task.FromResult(Result.Success(clientToUpdate));
    }

    /// <inheritdoc/>
    public Task<Result> DeleteAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var clientToUpdate = _clients.FirstOrDefault(c => c.Id == client.Id);
        if (clientToUpdate == null)
        {
            return Task.FromResult(Result.Failure()
                .WithMessage("Failed to delete the client, not found")
                .WithCode("client_not_found"));
        }

        _clients.Remove(clientToUpdate);

        return Task.FromResult(Result.Success());
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsInMemoryStore"/>
/// </summary>
public partial class RavenSmsClientsInMemoryStore
{
    private readonly ICollection<RavenSmsClient> _clients;

    public RavenSmsClientsInMemoryStore()
    {
        _clients = new List<RavenSmsClient>
        {
            new RavenSmsClient
            {
                Id = "clt_defaultclient",
                Name = "Default Client",
                CreatedOn = DateTimeOffset.Now,
                PhoneNumber = "00212060606606",
                Status = RavenSmsClientStatus.RequireSetup,
                Description = "the default client created when the list of clients is initialized",
            }
        };
    }

    private static IEnumerable<RavenSmsClient> SetFilter(IEnumerable<RavenSmsClient> query, RavenSmsClientsFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchQuery))
            query = query.Where(e =>
                e.Description.Contains(filter.SearchQuery) ||
                e.Name.Contains(filter.SearchQuery)
            );

        if (filter.Status != RavenSmsClientStatus.None)
            query = query.Where(e => filter.Status == e.Status);

        if (filter.phoneNumbers is not null && filter.phoneNumbers.Any())
            query = query.Where(e => filter.phoneNumbers.Contains(e.PhoneNumber));

        return query;
    }
}
