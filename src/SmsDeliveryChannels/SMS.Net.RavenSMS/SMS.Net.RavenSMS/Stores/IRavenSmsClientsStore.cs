﻿namespace SMS.Net.Channel.RavenSMS.Persistence;

/// <summary>
/// the repository for managing clients apps.
/// </summary>
public interface IRavenSmsClientsStore
{
    /// <summary>
    /// get the list of all registered clients.
    /// </summary>
    /// <returns>the list of clients.</returns>
    Task<RavenSmsClient[]> GetAllAsync();

    /// <summary>
    /// get the list of all clients
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <returns>the list of messages and total count of rows</returns>
    Task<(RavenSmsClient[] data, int rowsCount)> GetAllAsync(RavenSmsClientsFilter filter);

    /// <summary>
    /// check if there is any client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number instance.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="ArgumentNullException">if the given phone number instance is null.</exception>
    Task<bool> AnyAsync(PhoneNumber phoneNumber);

    /// <summary>
    /// find the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient?> FindByIdAsync(string clientId);

    /// <summary>
    /// find the client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number associated with the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient?> FindByPhoneNumberAsync(PhoneNumber phoneNumber);

    /// <summary>
    /// save the given client.
    /// </summary>
    /// <param name="client">the client to be saved</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsClient>> SaveAsync(RavenSmsClient client);

    /// <summary>
    /// update the given client.
    /// </summary>
    /// <param name="client">the client to be updated</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsClient>> UpdateAsync(RavenSmsClient client);
}
