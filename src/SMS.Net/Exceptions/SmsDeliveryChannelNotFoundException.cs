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
    public SmsDeliveryChannelNotFoundException(string emailDeliveryProviderName)
        : base(message.Replace("{{name}}", emailDeliveryProviderName)) 
    {
        SmsDeliveryChannelName = string.Empty;
    }

    /// <inheritdoc/>
    public SmsDeliveryChannelNotFoundException(string message, string emailDeliveryProviderName)
        : base(message)
    {
        SmsDeliveryChannelName = emailDeliveryProviderName;
    }

    /// <inheritdoc/>
    protected SmsDeliveryChannelNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        SmsDeliveryChannelName = string.Empty;
    }
}
