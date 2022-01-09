namespace SMS.Net.Channel.RavenSMS.Persistence;

/// <summary>
/// the repository for managing clients apps.
/// </summary>
public interface IRavenSmsClientsStore
{
    /// <summary>
    /// check if there is any client with the given phone number.
    /// </summary>
    /// <param name="from">the phone number instance.</param>
    /// <returns>true if exist, false if not.</returns>
    /// <exception cref="ArgumentNullException">if the given phone number instance is null.</exception>
    Task<bool> AnyAsync(PhoneNumber from);
    
    /// <summary>
    /// get the list of all registered clients.
    /// </summary>
    /// <returns>the list of clients.</returns>
    Task<RavenSmsClient[]> GetAllAsync();
}
