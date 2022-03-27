namespace SMS.Net.Channel.RavenSMS;

/// <summary>
/// exception thrown when the sending of sms message has failed
/// </summary>
[Serializable]
public class RavenSmsMessageSendingFailedException : RavenSmsException
{
    /// <inheritdoc/>
    public RavenSmsMessageSendingFailedException(string cause) 
        : base($"failed to send the sms message, cause: [{cause}]") { }

    /// <inheritdoc/>
    protected RavenSmsMessageSendingFailedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
