namespace SMS.Net.Channel.RavenSMS;

/// <summary>
/// the ravenSMS client connector to send commands to the client
/// </summary>
public interface IRavenSmsClientConnector
{
    /// <summary>
    /// send the given message to using the client connection
    /// </summary>
    /// <param name="client">the client to send the message with</param>
    /// <param name="message">the message to send</param>
    /// <returns>the sending result</returns>
    Task<Result> SendSmsMessageAsync(RavenSmsClient client, RavenSmsMessage message);
}
