namespace SMS.Net;

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
    /// Gets or sets the message body.
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the sender.
    /// </summary>
    public PhoneNumber From { get; set; }

    /// <summary>
    /// Gets or sets the phone numbers of recipients to send the SMS message to.
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
    /// <summary>
    /// create an instance of <see cref="SmsMessage"/>
    /// </summary>
    /// <param name="priority">the message priority</param>
    /// <param name="body">the message body</param>
    /// <param name="from">the sender phone number</param>
    /// <param name="to">the recipient phone number</param>
    /// <param name="channelData">the custom delivery channel data</param>
    /// <exception cref="ArgumentNullException">thrown if the <paramref name="to"/> is null</exception>
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
        => new();
}

/// <summary>
/// Specifies the priority of the message.
/// </summary>
public enum Priority
{
    /// <summary>
    ///  The email has low priority.
    /// </summary>
    Low = 0,

    /// <summary>
    /// The email has normal priority.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// The email has high priority.
    /// </summary>
    High = 2
}
