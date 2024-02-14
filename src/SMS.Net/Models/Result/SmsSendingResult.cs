namespace SMS.Net;

/// <summary>
/// the SMS sending result
/// </summary>
public partial class SmsSendingResult
{
    /// <summary>
    /// Get if the SMS has been sent successfully.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Get the name of the channel used to send the SMS.
    /// </summary>
    public string ChannelName { get; }

    /// <summary>
    /// additional data associated with this result if any.
    /// </summary>
    public IDictionary<string, object> MetaData { get; }

    /// <summary>
    /// Get the errors associated with the sending failure.
    /// </summary>
    public IEnumerable<SmsSendingError> Errors => _errors;
}

/// <summary>
/// the partial part for <see cref="SmsSendingResult"/>
/// </summary>
public partial class SmsSendingResult
{
    private readonly HashSet<SmsSendingError> _errors;

    /// <summary>
    /// create an instance of <see cref="SmsSendingResult"/>.
    /// </summary>
    /// <param name="isSuccess">true if the sending was successfully</param>
    /// <param name="channelName">the name of the channel used to sent the SMS.</param>
    /// <param name="errors">the errors associated with the sending.</param>
    /// <exception cref="ArgumentNullException">if <paramref name="channelName"/> is null</exception>
    public SmsSendingResult(bool isSuccess, string channelName, params SmsSendingError[] errors)
    {
        IsSuccess = isSuccess;
        MetaData = new Dictionary<string, object>();
        _errors = new HashSet<SmsSendingError>(errors);
        ChannelName = channelName ?? throw new ArgumentNullException(nameof(channelName));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var stringbuilder = new System.Text.StringBuilder($"{ChannelName} -> sending: ");

        if (IsSuccess)
            stringbuilder.Append("Succeeded");
        else
            stringbuilder.Append("Failed");

        if (!(_errors is null) && _errors.Count != 0)
            stringbuilder.Append($" | {_errors.Count} errors");

        if (MetaData.Count != 0)
            stringbuilder.Append($" | {MetaData.Count} meta-data");

        return stringbuilder.ToString();
    }

    /// <summary>
    /// add a new error to the errors list.
    /// </summary>
    /// <param name="error">the error to add.</param>
    /// <returns>the <see cref="SmsSendingResult"/> instance to enable methods chaining.</returns>
    /// <exception cref="ArgumentNullException">the error is null</exception>
    public SmsSendingResult AddError(SmsSendingError error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));

        _errors.Add(error);
        return this;
    }

    /// <summary>
    /// add a new error to the errors list.
    /// </summary>
    /// <param name="exception">the exception to add as error.</param>
    /// <returns>the <see cref="SmsSendingResult"/> instance to enable methods chaining.</returns>
    /// <exception cref="ArgumentNullException">the error is null</exception>
    public SmsSendingResult AddError(Exception exception)
        => AddError(new SmsSendingError(exception));

    /// <summary>
    /// add a new error to the errors list.
    /// </summary>
    /// <param name="key">the key of the meta-data.</param>
    /// <param name="value">the value of the meta-data.</param>
    /// <returns>the <see cref="SmsSendingResult"/> instance to enable methods chaining.</returns>
    /// <exception cref="ArgumentNullException">the key is null</exception>
    /// <exception cref="ArgumentException">key is empty, or An element with the same key already exists</exception>
    public SmsSendingResult AddMetaData(string key, object value)
    {
        MetaData.Add(key, value);
        return this;
    }

    /// <summary>
    /// get the meta-data with the given key, if not found default will be returned
    /// </summary>
    /// <typeparam name="TValue">the type of the value</typeparam>
    /// <param name="key">the meta data key</param>
    /// <param name="defaultValue">the default value to return if noting found, by default is it set to "default"</param>
    /// <returns>instance of the value for the given key, or default if not found</returns>
    public TValue? GetMetaData<TValue>(string key, TValue? defaultValue = default)
    {
        if (MetaData.TryGetValue(key, out object value))
            return (TValue)value;

        return defaultValue;
    }

    /// <summary>
    /// create an instance of <see cref="SmsSendingResult"/> with a success state.
    /// </summary>
    /// <param name="channelName">the name of the channel used to send the email.</param>
    /// <returns>instance of <see cref="SmsSendingResult"/></returns>
    public static SmsSendingResult Success(string channelName)
        => new(true, channelName);

    /// <summary>
    /// create an instance of <see cref="SmsSendingResult"/> with a failure state.
    /// </summary>
    /// <param name="channelName">the name of the channel used to send the email.</param>
    /// <param name="errors">errors associated with the failure if any.</param>
    /// <returns>instance of <see cref="SmsSendingResult"/></returns>
    public static SmsSendingResult Failure(string channelName, params SmsSendingError[] errors)
        => new(false, channelName);

    /// <summary>
    /// this static class holds the keys names used in the email sending meta-data
    /// </summary>
    public static class MetaDataKeys
    {
        /// <summary>
        /// key to indicate whether the sending is paused.
        /// </summary>
        public const string SendingPaused = "sending_paused";
    }
}
