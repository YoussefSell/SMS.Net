namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the manager for managing SMS messages.
/// </summary>
public interface IRavenSmsManager
{
    /// <summary>
    /// Process the sending of the message.
    /// </summary>
    /// <param name="messageId">the id of the message to process.</param>
    /// <returns>a Task instance</returns>
    Task ProcessAsync(Guid messageId);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue</param>
    /// <returns>a Task instance</returns>
    Task<Result> QueueMessageAsync(RavenSmsMessage message);

    /// <summary>
    /// queue the message for processing
    /// </summary>
    /// <param name="message">the message to queue.</param>
    /// <param name="delay">the delay to use before sending the message.</param>
    /// <returns>a Task instance</returns>
    Task<Result> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay);

    /// <summary>
    /// get the list of all messages
    /// </summary>
    /// <returns>the list of messages and total count of rows</returns>
    Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync();

    /// <summary>
    /// create a new ravenSMS client.
    /// </summary>
    /// <param name="model">the model used to create the client</param>
    /// <returns>the operation result</returns>
    Task<Result<RavenSmsClient>> CreateClientAsync(RavenSmsClient model);

    /// <summary>
    /// find the client with the given Id.
    /// </summary>
    /// <param name="clientId">the id of the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient?> FindClientByIdAsync(Guid clientId);

    /// <summary>
    /// find the client with the given phone number.
    /// </summary>
    /// <param name="phoneNumber">the phone number associated with the client to find.</param>
    /// <returns>instance of <see cref="RavenSmsClient"/> found, full if not exist.</returns>
    Task<RavenSmsClient?> FindClientByPhoneNumberAsync(PhoneNumber phoneNumber);
    
    /// <summary>
    /// get the list of all registered clients.
    /// </summary>
    /// <returns>the list of clients.</returns>
    Task<RavenSmsClient[]> GetAllClientsAsync();
}