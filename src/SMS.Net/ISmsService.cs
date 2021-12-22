namespace SMS.Net
{
    using SMS.Net.Channel;
    using SMS.Net.Exceptions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// the SMS service used to abstract the SMS sending
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Sends the specified SMS message using the default <see cref="ISmsChannel"/>.
        /// </summary>
        /// <param name="message">the SMS message to be send.</param>
        /// <remarks>
        /// the default SMS delivery channel should be specified 
        /// in <see cref="SmsServiceOptions.DefaultDeliveryChannel"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">the given message instance is null.</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the default <see cref="ISmsChannel"/>.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <remarks>
        /// the default SMS delivery channel should be specified 
        /// in <see cref="SmsServiceOptions.DefaultDeliveryChannel"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">the given message instance is null.</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the SMS delivery channel with the given name.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="edp_name">the name of the SMS delivery channel used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">the given message instance is null, or the channel name is null</exception>
        /// <exception cref="SmsDeliveryChannelNotFoundException">couldn't find any delivery channel with the given name</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message, string edp_name);

        /// <summary>
        /// Sends the specified SMS message using the SMS delivery channel with the given name.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="edp_name">the name of the SMS delivery channel used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">the given message instance is null, or the delivery channel name is null</exception>
        /// <exception cref="SmsDeliveryChannelNotFoundException">couldn't find any EDP with the given name</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, string edp_name);

        /// <summary>
        /// Sends the specified SMS message using the given SMS delivery channel.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="edp">the SMS delivery channel used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">the given message instance is null, or the delivery channel is null</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message, ISmsChannel edp);

        /// <summary>
        /// Sends the specified SMS message using the given SMS delivery channel.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="edp">the SMS delivery channel used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">the given message instance is null, or the delivery channel is null</exception>
        /// <exception cref="ArgumentException">
        /// the given message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, ISmsChannel edp);
    }
}