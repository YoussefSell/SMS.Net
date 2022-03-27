namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMs client manager implementation
/// </summary>
public partial class RavenSmsClientsManager
{
    /// <inheritdoc/>
    public Task<long> GetClientsCountAsync(CancellationToken cancellationToken = default)
        => _clientsStore.GetCountAsync(cancellationToken);
    
    /// <inheritdoc/>
    public Task<RavenSmsClient[]> GetAllClientsAsync(CancellationToken cancellationToken = default)
        => _clientsStore.GetAllAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<(RavenSmsClient[] clients, int rowsCount)> GetAllClientsAsync(RavenSmsClientsFilter filter, CancellationToken cancellationToken = default)
        => _clientsStore.GetAllAsync(filter, cancellationToken);
    
    /// <inheritdoc/>
    public async Task<RavenSmsClient[]> GetAllConnectedClientsAsync(CancellationToken cancellationToken = default)
    {
        var clients = await GetAllClientsAsync(new RavenSmsClientsFilter
        {
            IgnorePagination = true,
        }, cancellationToken);

        return clients.clients;
    }

    /// <inheritdoc/>
    public Task<bool> AnyClientAsync(string clientId, CancellationToken cancellationToken = default)
        => _clientsStore.AnyAsync(clientId, cancellationToken);

    /// <inheritdoc/>
    public Task<bool> AnyClientAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
        => _clientsStore.AnyAsync(phoneNumber, cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByIdAsync(string clientId, CancellationToken cancellationToken = default)
        => _clientsStore.FindByIdAsync(clientId, cancellationToken);

    /// <inheritdoc/>
    public Task<RavenSmsClient?> FindClientByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
        => _clientsStore.FindByPhoneNumberAsync(phoneNumber, cancellationToken);

    /// <inheritdoc/>
    public async Task<Result<RavenSmsClient>> SaveClientAsync(RavenSmsClient model, CancellationToken cancellationToken = default)
    {
        return await _clientsStore.AnyAsync(model.Id, cancellationToken)
            ? await _clientsStore.UpdateAsync(model, cancellationToken)
            : await _clientsStore.CreateAsync(model, cancellationToken);
    }
    
    /// <inheritdoc/>
    public Task<Result> DeleteClientAsync(RavenSmsClient client, CancellationToken cancellationToken = default)
        => _clientsStore.DeleteAsync(client, cancellationToken);

    /// <inheritdoc/>
    public async Task<Result> DeleteClientAsync(string clientId, CancellationToken cancellationToken = default)
    {
        var client = await _clientsStore.FindByIdAsync(clientId, cancellationToken).ConfigureAwait(false);
        if (client is null)
        {
            return Result.Failure()
                .WithMessage("there is no client with the given id")
                .WithCode(ResultCode.NotFound);
        }

        return await DeleteClientAsync(client, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Result> ClientDisconnectedAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var client = await _clientsStore.FindByConnectionIdAsync(connectionId, cancellationToken);
        if (client is null)
        {
            return Result.Failure()
                .WithMessage("there is no client with the given connection id")
                .WithCode(ResultCode.NotFound);
        }

        client.ConnectionId = string.Empty;
        client.Status = RavenSmsClientStatus.Disconnected;
        return await _clientsStore.UpdateAsync(client, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Result> ClientConnectedAsync(RavenSmsClient client, string connectionId, CancellationToken cancellationToken = default)
    {
        // set the client id
        client.ConnectionId = connectionId;
        client.Status = RavenSmsClientStatus.Connected;

        // attach the connection id to the client in database
        return await _clientsStore.UpdateAsync(client, cancellationToken);
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsManager"/>
/// </summary>
public partial class RavenSmsClientsManager : IRavenSmsClientsManager
{
    private readonly IRavenSmsClientsStore _clientsStore;

    public RavenSmsClientsManager(IRavenSmsClientsStore clientsStore) 
        => _clientsStore = clientsStore;
}