namespace SMS.Net.Exceptions;

/// <summary>
/// exception thrown when no SMS delivery channel has been found
/// </summary>
[Serializable]
public class SmsDeliveryChannelNotFoundException : Exception
{
    private static readonly string message = "there is no SMS delivery channel with the name {{name}}, make sure you have registered the channel with the SMS service";

    /// <summary>
    /// the name of the SMS delivery channel.
    /// </summary>
    public string SmsDeliveryChannelName { get; set; }

    /// <inheritdoc/>
    public SmsDeliveryChannelNotFoundException(string smsDeliveryChannelName)
        : base(message.Replace("{{name}}", smsDeliveryChannelName)) 
    {
        SmsDeliveryChannelName = string.Empty;
    }

    /// <inheritdoc/>
    public SmsDeliveryChannelNotFoundException(string message, string smsDeliveryChannelName)
        : base(message)
    {
        SmsDeliveryChannelName = smsDeliveryChannelName;
    }

    /// <inheritdoc/>
    protected SmsDeliveryChannelNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        SmsDeliveryChannelName = string.Empty;
    }
}
