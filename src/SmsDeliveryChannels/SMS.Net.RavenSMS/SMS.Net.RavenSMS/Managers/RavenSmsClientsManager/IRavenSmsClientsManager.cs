namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS client manager
/// </summary>
public interface IRavenSmsClientsManager
{
    /// <summary>
    /// get the total count of the clients.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>total count of the clients</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<long> GetClientsCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of all clients.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>a list of all clients.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient[]> GetAllClientsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of clients using a filter with total count of all entities that matches the filter for pagination.
    /// </summary>
    /// <param name="filter">the filter used to retrieve the clients.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the list of messages and total count of rows</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    /// <exception cref="ArgumentNullException">If the <see cref="RavenSmsClientsFilter"/> is null.</exception>
    Task<(RavenSmsClient[] clients, int rowsCount)> GetAllClientsAsync(RavenSmsClientsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of all connected clients.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the list of clients.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient[]> GetAllConnectedClientsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// check if there is any client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number instance.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    /// <exception cref="ArgumentNullException">if the given phone number instance is null.</exception>
    Task<bool> AnyClientAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// check if there is any client with the given id.
    /// </summary>
    /// <param name="clientId">the client id.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<bool> AnyClientAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client to find.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient?> FindClientByIdAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number value.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient?> FindClientByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// set the client as Connected, and associated the given connection id with it.
    /// </summary>
    /// <param name="client">the connected client</param>
    /// <param name="connectionId">the connection id</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> ClientConnectedAsync(RavenSmsClient client, string connectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// set the client associated with the given connectionId as disconnected.
    /// </summary>
    /// <param name="connectionId">the connection Id</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> ClientDisconnectedAsync(string connectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// create a new client if not exist, if exist the client will be updated.
    /// </summary>
    /// <param name="model">client model instance</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsClient>> SaveClientAsync(RavenSmsClient model, CancellationToken cancellationToken = default);

    /// <summary>
    /// delete the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> DeleteClientAsync(string clientId, CancellationToken cancellationToken = default);
}