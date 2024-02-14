namespace SMS.Net;

/// <summary>
/// class defines the email sending error
/// </summary>
public class SmsSendingError
{
    /// <summary>
    /// create an instance of <see cref="SmsSendingError"/>
    /// </summary>
    /// <param name="exception">the exception associated with this error</param>
    /// <exception cref="ArgumentNullException">exception is null</exception>
    public SmsSendingError(Exception exception)
    {
        Code = "internal_error";
        Message = "an exception has been throw while performing the action";
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }

    /// <summary>
    /// create an instance of <see cref="SmsSendingError"/>
    /// </summary>
    /// <param name="code">the code of the error</param>
    /// <param name="message">the message of the error</param>
    /// <exception cref="ArgumentNullException">if any argument null</exception>
    public SmsSendingError(string code, string message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// create an instance of <see cref="SmsSendingError"/>
    /// </summary>
    /// <param name="code">the code of the error</param>
    /// <param name="message">the message of the error</param>
    /// <param name="exception">the exception associated with this error</param>
    /// <exception cref="ArgumentNullException">if any argument null</exception>
    public SmsSendingError(string code, string message, Exception exception)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }

    /// <summary>
    /// Get the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Get the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// the exception associated with this error if any
    /// </summary>
    public Exception? Exception { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Code} | {Message}";
}
