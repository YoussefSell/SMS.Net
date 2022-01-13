namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing the RavenSMS client apps
/// </summary>
public interface IRavenSmsClientsManager
{
    /// <summary>
    /// check if there is any Client app registered with the given phone number
    /// </summary>
    /// <param name="from">the phone number instance</param>
    /// <returns>true if exist, false if not</returns>
    /// <exception cref="ArgumentNullException">if the <paramref name="from"/> value is null</exception>
    Task<bool> AnyAsync(PhoneNumber from);

    /// <summary>
    /// find the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient> FindByIdAsync(Guid clientId);

    /// <summary>
    /// find the client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number associated with the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient> FindByPhoneNumberAsync(PhoneNumber phoneNumber);

    /// <summary>
    /// get the list of all registered clients.
    /// </summary>
    /// <returns>the list of clients.</returns>
    Task<RavenSmsClient[]> GetAllAsync();
}