﻿namespace SMS.Net.Channel.RavenSMS.Managers;

/// <summary>
/// the ravenSMS manager, used to manage all messages sent with RavenSMS. 
/// </summary>
public partial class RavenSmsManager : IRavenSmsManager
{
    /// <inheritdoc/>
    public async Task ProcessAsync(string messageId)
    {
        var message = await _messagesManager.FindByIdAsync(messageId);
        if (message is null)
            throw new RavenSmsMessageNotFoundException($"there is no message with the given Id {messageId}");

        // get the client associated with the given from number
        var client = await _clientsManagers.FindClientByIdAsync(message.ClientId);
        if (client is null)
            throw new RavenSmsMessageSendingFailedException("client_not_found_by_phone");

        if (client.Status != RavenSmsClientStatus.Connected)
            throw new RavenSmsMessageSendingFailedException("client_not_connected");

        // send the SMS message command to the client
        var sendResult = await _clientConnector.SendSmsMessageAsync(client, message);
        if (sendResult.IsFailure())
        {
            // if we failed to send the message to the client we need to record this failed attempt
            // create an attempt recored & update message status
            var attempt = new RavenSmsMessageSendAttempt { Status = SendAttemptStatus.Failed };
            
            // add the error to the attempt
            attempt.AddError(sendResult.Code, sendResult.Message);

            // update the message status
            message.SendAttempts.Add(attempt);
            message.SentOn = DateTimeOffset.UtcNow;
            message.Status = RavenSmsMessageStatus.Failed;
        }

        await _messagesManager.SaveAsync(message);

        if (sendResult.IsFailure())
            throw new RavenSmsMessageSendingFailedException(sendResult.Code);
    }

    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message)
    {
        // queue the message for future processing
        message.JobQueueId = await _queueManager.QueueMessageAsync(message);
        message.Status = RavenSmsMessageStatus.Queued;

        // save the message
        var saveResult = await _messagesManager.SaveAsync(message);
        if (saveResult.IsFailure())
        {
            return Result.Failure()
                .WithMessage("failed to persist the message")
                .WithErrors(saveResult.Errors.ToArray());
        }

        // all done
        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result> QueueMessageAsync(RavenSmsMessage message, TimeSpan delay)
    {
        // queue the message for future processing
        message.JobQueueId = await _queueManager.QueueMessageAsync(message, delay);
        message.Status = RavenSmsMessageStatus.Queued;

        // save the message
        var saveResult = await _messagesManager.SaveAsync(message);
        if (saveResult.IsFailure())
        {
            return Result.Failure()
                .WithMessage("failed to persist the message")
                .WithErrors(saveResult.Errors.ToArray());
        }

        // all done
        return Result.Success();
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsManager"/>
/// </summary>
public partial class RavenSmsManager
{
    private readonly IQueueManager _queueManager;
    private readonly IHubContext<RavenSmsHub> _clientConnector;
    private readonly IRavenSmsClientsManager _clientsManagers;
    private readonly IRavenSmsMessagesManager _messagesManager;

    public RavenSmsManager(
        IQueueManager queueManager,
        IHubContext<RavenSmsHub> clientConnector,
        IRavenSmsClientsManager clientsManagers,
        IRavenSmsMessagesManager messagesManager)
    {
        _queueManager = queueManager;
        _clientConnector = clientConnector;
        _clientsManagers = clientsManagers;
        _messagesManager = messagesManager;
    }
}
