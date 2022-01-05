namespace SMS.Net.RavenSMS.Managers
{
    using System;
    using System.Threading.Tasks;

    public class RavenSmsClientsManager : IRavenSmsClientsManager
    {
        public Task<bool> AnyAsync(PhoneNumber from)
        {
            throw new NotImplementedException();
        }
    }
}