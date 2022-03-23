namespace SMS.Net.Channel.RavenSMS.Stores;

/// <summary>
/// the store for managing RavenSMS clients data.
/// </summary>
public interface IRavenSmsClientsStore
{
    /// <summary>
    /// get the total count of the clients.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>total count of the clients</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of all clients.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>a list of all clients.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient[]> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// get the list of clients using a filter with total count of all entities that matches the filter for pagination.
    /// </summary>
    /// <param name="filter">the filter used to retrieve the clients.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the list of messages and total count of rows</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    /// <exception cref="ArgumentNullException">If the <see cref="RavenSmsClientsFilter"/> is null.</exception>
    Task<(RavenSmsClient[] data, int rowsCount)> GetAllAsync(RavenSmsClientsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// check if there is any client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number instance.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    /// <exception cref="ArgumentNullException">if the given phone number instance is null.</exception>
    Task<bool> AnyAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// check if there is any client with the given id.
    /// </summary>
    /// <param name="clientId">the client id.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<bool> AnyAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client to find.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient?> FindByIdAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the clients with the given Ids.
    /// </summary>
    /// <param name="clientsIds">the ids of the clients to find.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>array of <see cref="RavenSmsClient"/> found.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient[]> FindByIdAsync(string[] clientsIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// find a client by connection Id.
    /// </summary>
    /// <param name="connectionId">the connection Id</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the client associated with the given connection Id</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient?> FindByConnectionIdAsync(string connectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// find the client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number value.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// save the given client to the underlying store.
    /// </summary>
    /// <param name="client">the client instance to be saved</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsClient>> CreateAsync(RavenSmsClient client, CancellationToken cancellationToken = default);

    /// <summary>
    /// update the given client in the store.
    /// </summary>
    /// <param name="client">the client instance to be updated</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result<RavenSmsClient>> UpdateAsync(RavenSmsClient client, CancellationToken cancellationToken = default);

    /// <summary>
    /// delete the client from the store.
    /// </summary>
    /// <param name="client">the client instance to be delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>the operation result</returns>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    Task<Result> DeleteAsync(RavenSmsClient client, CancellationToken cancellationToken = default);
}
