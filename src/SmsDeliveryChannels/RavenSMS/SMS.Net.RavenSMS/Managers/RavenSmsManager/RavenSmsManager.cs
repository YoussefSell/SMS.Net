namespace SMS.Net.RavenSMS.Managers
{
    using SMS.Net.Channel.RavenSMS;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// the ravenSMS manager, used to manage all messages sent with RavenSMS 
    /// </summary>
    public partial class RavenSmsManager : IRavenSmsManager
    {
        public Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage)
        {
            throw new NotImplementedException();
        }

        public Task QueueMessageAsync(RavenSmsMessage ravenSmsMessage, TimeSpan delay)
        {
            throw new NotImplementedException();
        }
    }
}
