namespace SMS.Net
{
    using SMS.Net.Channel;
    using SMS.Net.Factories;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// the SMS message.
    /// </summary>
    public partial class SmsMessage
    {
        /// <summary>
        /// Gets or sets the priority of this e-mail message.
        /// </summary>
        public Priority Priority { get; }

        /// <summary>
        /// the message body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// the phone number of the sender.
        /// </summary>
        public PhoneNumber From { get; set; }

        /// <summary>
        /// the phone numbers of recipients to send the SMS message to.
        /// </summary>
        public PhoneNumber To { get; }

        /// <summary>
        /// get the collection of additional data need to be passed to the SMS delivery channel for further configuration
        /// </summary>
        public ICollection<ChannelData> ChannelData { get; set; }
    }

    /// <summary>
    /// partial part for <see cref="SmsMessage"/>
    /// </summary>
    public partial class SmsMessage
    {
        public SmsMessage(Priority priority, string body, PhoneNumber from, PhoneNumber to, ICollection<ChannelData> channelData)
        {
            if (to is null)
                throw new ArgumentNullException(nameof(to));

            To = to;
            From = from;

            Priority = priority;
            Body = body ?? string.Empty;
            ChannelData = channelData ?? new HashSet<ChannelData>();
        }

        /// <summary>
        /// set the sender phone number.
        /// </summary>
        /// <param name="from">the sender phone number.</param>
        public void SetFrom(PhoneNumber from)
            => From = from;

        /// <summary>
        /// create an instance of <see cref="SmsMessageComposer"/> to start composing the message data.
        /// </summary>
        /// <returns>instance of <see cref="SmsMessageComposer"/>.</returns>
        public static SmsMessageComposer Compose()
            => new SmsMessageComposer();
    }
}
