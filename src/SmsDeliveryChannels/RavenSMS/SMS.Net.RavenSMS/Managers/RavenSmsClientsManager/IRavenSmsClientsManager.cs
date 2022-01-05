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
}