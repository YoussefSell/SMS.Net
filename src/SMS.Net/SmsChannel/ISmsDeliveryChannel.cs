namespace SMS.Net.Channel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// defines the SMS delivery channel for sending the SMS messages.
    /// </summary>
    public interface ISmsDeliveryChannel
    {
        /// <summary>
        /// a unique name of the SMS delivery channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);
    }
}
