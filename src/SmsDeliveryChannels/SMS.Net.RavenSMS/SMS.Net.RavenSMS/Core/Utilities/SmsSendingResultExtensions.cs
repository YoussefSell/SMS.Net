namespace SMS.Net.Channel.RavenSMS.Utilities;

/// <summary>
/// extensions for the <see cref="SmsSendingResult"/> class
/// </summary>
internal static class SmsSendingResultExtensions
{
    /// <summary>
    /// add a new error to the errors list.
    /// </summary>
    /// <param name="error">the error to add.</param>
    /// <returns>the <see cref="SmsSendingResult"/> instance to enable methods chaining.</returns>
    /// <exception cref="ArgumentNullException">the error is null</exception>
    internal static SmsSendingResult AddError(this SmsSendingResult smsSendingResult, Result result)
    {
        if (result is null)
            throw new ArgumentNullException(nameof(result));

        smsSendingResult.AddError(new SmsSendingError(result.Code, result.Message));

        if (result.HasErrors())
        {
            foreach (var error in result.Errors)
                smsSendingResult.AddError(new SmsSendingError(error.Code, error.Message));
        }

        return smsSendingResult;
    }
}
