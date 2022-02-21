namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing messages
/// </summary>
public interface IRavenSmsMessagesManager
{

    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <param name="filter">the filter used to retrieve the messages.</param>
    /// <returns>the list of messages and total count of rows</returns>
    Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync(RavenSmsMessageFilter filter);
}
