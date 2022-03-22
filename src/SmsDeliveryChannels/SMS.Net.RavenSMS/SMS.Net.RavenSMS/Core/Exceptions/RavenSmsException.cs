namespace SMS.Net.Channel.RavenSMS.Exceptions;

/// <summary>
/// the base exception for all RavenSms related errors
/// </summary>
[Serializable]
public class RavenSmsException : Exception
{
    /// <inheritdoc/>
    public RavenSmsException() { }

    /// <inheritdoc/>
    public RavenSmsException(string message) : base(message) { }

    /// <inheritdoc/>
    protected RavenSmsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
