namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS clients manager, used to manage all clients used to send the SMS messages.
/// </summary>
public partial class RavenSmsClientsManager : IRavenSmsClientsManager
{
    public Task<bool> AnyAsync(PhoneNumber from)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsManager"/>
/// </summary>
public partial class RavenSmsClientsManager
{

}