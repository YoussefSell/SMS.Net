namespace SMS.Net.Channel
{
    using System.Threading.Tasks;

    /// <summary>
    /// defines the SMS delivery channel for sending the SMS messages.
    /// </summary>
    public interface ISmsChannel
    {
        /// <summary>
        /// a unique name of the SMS delivery channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sends the specified SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        Task<SmsSendingResult> SendAsync(SmsMessage message);
    }
}
