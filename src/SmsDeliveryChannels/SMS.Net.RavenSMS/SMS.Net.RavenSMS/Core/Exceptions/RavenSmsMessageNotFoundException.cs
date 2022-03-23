namespace SMS.Net.Channel.RavenSMS;

/// <summary>
/// exception thrown when no RavenSms message has been found
/// </summary>
[Serializable]
public class RavenSmsMessageNotFoundException : RavenSmsException
{
    /// <inheritdoc/>
    public RavenSmsMessageNotFoundException() { }

    /// <inheritdoc/>
    public RavenSmsMessageNotFoundException(string message) : base(message) { }

    /// <inheritdoc/>
    protected RavenSmsMessageNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
