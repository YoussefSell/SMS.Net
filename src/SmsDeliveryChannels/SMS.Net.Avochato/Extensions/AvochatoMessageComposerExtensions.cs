namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsMessageComposer"/> factory.
/// </summary>
public static class AvochatoMessageComposerExtensions
{
    /// <summary>
    /// pass a custom channel data to configure a custom AuthKey to be used when initializing 
    /// the Avochato client instead of using the one set in the <see cref="AvochatoSmsDeliveryChannelOptions.AuthId"/>
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the AccessKey to be used.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer UseAuthKey(this SmsMessageComposer messageComposer, string value)
        => messageComposer.WithCustomData(CustomChannelData.AuthId, value);

    /// <summary>
    /// pass a custom channel data to configure a custom AuthSecret to be used when initializing the Avochato client instead of using the one set in the <see cref="AvochatoSmsDeliveryChannelOptions.AuthSecret"/>
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the AuthSecret to be used.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer UseAuthSecret(this SmsMessageComposer messageComposer, string value)
        => messageComposer.WithCustomData(CustomChannelData.AuthSecret, value);

    /// <summary>
    /// list of tags to apply to this contact.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the Tags value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetTags(this SmsMessageComposer messageComposer, List<string> value)
        => messageComposer.WithCustomData(CustomChannelData.Tags, value);

    /// <summary>
    /// Mark this conversation as "addressed" (default: false).
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the MarkAddressed value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetMarkAddressed(this SmsMessageComposer messageComposer, bool value)
        => messageComposer.WithCustomData(CustomChannelData.MarkAddressed, value);

    /// <summary>
    /// Media attachment to send via MMS (500kb limit).
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the MediaUrl value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetMediaUrl(this SmsMessageComposer messageComposer, Uri value)
        => messageComposer.WithCustomData(CustomChannelData.MediaUrl, value);

    /// <summary>
    /// Send a callback via HTTP POST to this domain when the delivery status is updated.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the StatusCallback value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetStatusCallback(this SmsMessageComposer messageComposer, Uri value)
        => messageComposer.WithCustomData(CustomChannelData.StatusCallback, value);
}
