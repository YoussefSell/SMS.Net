namespace SMS.Net.Channel.MessageBird;

/// <summary>
/// the extensions methods over the <see cref="SmsMessageComposer"/> factory.
/// </summary>
public static class MessageBirdMessageComposerExtensions
{
    /// <summary>
    /// pass a custom channel data to configure a custom AccessKey to be used when initializing the MessageBird client instead of using the one set in the <see cref="MessageBirdSmsDeliveryChannelOptions.AccessKey"/>
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="accessKey">the AccessKey to be used.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer UseAccessKey(this SmsMessageComposer messageComposer, string accessKey)
        => messageComposer.WithCustomData(CustomChannelData.AccessKey, accessKey);
}
