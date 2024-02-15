namespace SMS.Net.Channel.D7Networks;

/// <summary>
/// the extensions methods over the <see cref="SmsMessageComposer"/> factory.
/// </summary>
public static class D7NetworksMessageComposerExtensions
{
    /// <summary>
    /// pass a custom channel data to configure a custom api-key to be used when initializing 
    /// the D7Networks client instead of using the one set in the <see cref="D7NetworksSmsDeliveryChannelOptions.ApiKey"/>
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the api-key to be used.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer UseApiKey(this SmsMessageComposer messageComposer, string value)
        => messageComposer.WithCustomData(CustomChannelData.ApiKey, value);

    /// <summary>
    /// To receive delivery status (DLR) for your message, 
    /// specify the callback server URL where you want to receive the message status updates using the report_url parameter.
    /// When the delivery status changes, the status updates will be sent to the specified URL. 
    /// For information on the format of the DLR message, please refer to the "Receiving DLR" section.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the ReportUrl value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetReportUrl(this SmsMessageComposer messageComposer, Uri value)
        => messageComposer.WithCustomData(CustomChannelData.ReportUrl, value);

    /// <summary>
    /// The Sender/Header of a message. 
    /// We can use your brand name with a maximum character limit of 11 or your mobile number with your country code.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the Originator value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetOriginator(this SmsMessageComposer messageComposer, string value)
        => messageComposer.WithCustomData(CustomChannelData.Originator, value);

    /// <summary>
    /// Set as text for normal GSM 03.38 characters(English, normal characters). 
    /// Set as unicode for non GSM 03.38 characters (Arabic, Chinese, Hebrew, Greek like regional languages and Unicode characters).
    /// Set as auto so we will find the data_coding based on your content.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the DataCoding value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetDataCoding(this SmsMessageComposer messageComposer, DataCoding value)
        => messageComposer.WithCustomData(CustomChannelData.DataCoding, value);

    /// <summary>
    /// set the type of the message.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the MessageType value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetMessageType(this SmsMessageComposer messageComposer, MessageType value)
        => messageComposer.WithCustomData(CustomChannelData.MessageType, value);

    /// <summary>
    /// To schedule the message to be sent at a specific date and time.
    /// </summary>
    /// <param name="messageComposer">the message composer instance.</param>
    /// <param name="value">the ScheduleTime value.</param>
    /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
    public static SmsMessageComposer SetScheduleTime(this SmsMessageComposer messageComposer, DateTime value)
        => messageComposer.WithCustomData(CustomChannelData.ScheduleTime, value);
}
