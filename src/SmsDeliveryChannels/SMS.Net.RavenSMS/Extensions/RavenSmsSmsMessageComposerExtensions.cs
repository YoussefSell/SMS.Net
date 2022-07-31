namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsMessageComposer"/> factory.
/// </summary>
public static class RavenSmsSmsMessageComposerExtensions
{
    /// <summary>
    /// set the delay to wait before sending the message.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="delay">the Timespan delay.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SendAfter(this SmsMessageComposer messageComposer, TimeSpan delay)
        => messageComposer.PassChannelData(CustomChannelData.Delay, delay);
}
