namespace SMS.Net.Channel.RavenSMS.Persistence;

/// <summary>
/// the repository for managing SMS messages data
/// </summary>
public interface IRavenSmsMessagesStore
{
    /// <summary>
    /// find the message with the given id.
    /// </summary>
    /// <param name="messageId">the id of the message</param>
    /// <returns>the message, or null if not found</returns>
    Task<RavenSmsMessage?> FindByIdAsync(Guid messageId);

    /// <summary>
    /// save the given message.
    /// </summary>
    /// <param name="message">the message to be saved</param>
    /// <returns>the operation result</returns>
    Task<Result> SaveAsync(RavenSmsMessage message);

    /// <summary>
    /// update the given message.
    /// </summary>
    /// <param name="message">the message to be updated</param>
    /// <returns>the operation result</returns>
    Task<Result> UpdateAsync(RavenSmsMessage message);
}
