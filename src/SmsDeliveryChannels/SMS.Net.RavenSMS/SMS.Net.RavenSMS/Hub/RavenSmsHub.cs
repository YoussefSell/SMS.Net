namespace SMS.Net.Channel.RavenSMS;

public class RavenSmsHub : Hub
{
    private readonly ILogger _logger;
    private readonly IRavenSmsClientsManager _clientsManager;
    private readonly IRavenSmsMessagesManager _messagesManager;

    public RavenSmsHub(
        IRavenSmsMessagesManager messagesManager,
        IRavenSmsClientsManager manager,
        ILogger<RavenSmsHub> logger
    )
    {
        _logger = logger;
        _clientsManager = manager;
        _messagesManager = messagesManager;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // disconnecting the client
        _logger.LogInformation("client associated with connection Id: {connectionId}, has been disconnected", Context.ConnectionId);
        await _clientsManager.ClientDisconnectedAsync(Context.ConnectionId);
    }

    public override async Task OnConnectedAsync()
    {
        // send an event to the client app to indicate that the connection has been established
        // because we don't have a way to get this info from the client app
        _logger.LogInformation("client associated with connection Id: {connectionId}, has connected", Context.ConnectionId);
        await Clients.Caller.SendAsync("ClientConnected");
    }

    public async Task PersistClientConnectionAsync(string clientId, bool forceConnection)
    {
        // get the client associated with the given id
        var client = await _clientsManager.FindClientByIdAsync(clientId);
        if (client is null)
        {
            await Clients.Caller.SendAsync("forceDisconnect", DisconnectionReason.ClientNotFound);
            return;
        }

        // check if the client already connected
        if (client.Status == RavenSmsClientStatus.Connected)
        {
            // check if the client is already connected
            if (client.ConnectionId == Context.ConnectionId)
                return;

            // new connection, check if we need to force the connection, or not
            if (!forceConnection)
            {
                await Clients.Caller.SendAsync("forceDisconnect", DisconnectionReason.ClientAlreadyConnected);
                return;
            }
        }

        // attach the client to the current connection
        _logger.LogInformation("connecting client with Id: {clientId}, connection Id: {connectionId}", client.Id, Context.ConnectionId);
        await _clientsManager.ClientConnectedAsync(client, Context.ConnectionId);

        // send the command to update the client info
        await Clients.Caller.SendAsync("updateClientInfo", new
        {
            clientId = client.Id,
            clientName = client.Name,
            clientDescription = client.Description,
            clientPhoneNumber = client.PhoneNumber,
        });
    }

    public async Task UpdateMessageStatusAsync(string messageId, RavenSmsMessageStatus status, string error)
    {
        var message = await _messagesManager.FindByIdAsync(messageId);
        if (message is null)
            return;

        var attempt = new RavenSmsMessageSendAttempt { Status = SendAttemptStatus.Sent };

        message.Status = status;
        message.SentOn = DateTimeOffset.UtcNow;
        message.SendAttempts.Add(attempt);

        if (status == RavenSmsMessageStatus.Failed)
        {
            attempt.Status = SendAttemptStatus.Failed;
            attempt.AddError(error, error.Equals(SmsErrorCodes.SmsPermissionDenied)
                ? "failed to send the sms message, you need to give the app the permission to send sms message, go to the app settings and check the sms permission"
                : "failed to send the sms message, check that your phone has a SIM card with credits to send the messages");
        }

        await _messagesManager.SaveAsync(message);
    }
}

public static class RavenSmsHubExtensions
{
    public static async Task<Result> UpdateClientInfosync(this IHubContext<RavenSmsHub> hub, RavenSmsClient client, CancellationToken cancellationToken = default)
    {
        try
        {
            if (client.Status != RavenSmsClientStatus.Connected)
                return Result.Failure()
                    .WithMessage("the client is not connected")
                    .WithCode("client_disconnected");

            if (client.ConnectionId is null)
                return Result.Failure()
                    .WithMessage("the client connection id is null or empty")
                    .WithCode("invalid_client_connection_id");

            await hub.Clients.Client(client.ConnectionId).SendAsync("updateClientInfo", new
            {
                clientId = client.Id,
                clientName = client.Name,
                clientDescription = client.Description,
                clientPhoneNumber = client.PhoneNumber,
            }, cancellationToken: cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure()
                .WithErrors(ex);
        }
    }

    public static async Task<Result> SendSmsMessageAsync(this IHubContext<RavenSmsHub> hub, RavenSmsClient client, RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            if (client.Status != RavenSmsClientStatus.Connected)
                return Result.Failure()
                    .WithMessage("the client is not connected")
                    .WithCode("client_disconnected");

            if (client.ConnectionId is null)
                return Result.Failure()
                    .WithMessage("the client connection id is null or empty")
                    .WithCode("invalid_client_connection_id");

            await hub.Clients.Client(client.ConnectionId).SendAsync("sendSmsMessage", new
            {
                from = client.PhoneNumber,
                createdOn = message.CreateOn,
                to = message.To.ToString(),
                sentOn = message.SentOn,
                status = message.Status,
                content = message.Body,
                id = message.Id,
            }, cancellationToken: cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure()
                .WithErrors(ex);
        }
    }

    public static async Task<Result> ForceDisconnectAsync(this IHubContext<RavenSmsHub> hub, RavenSmsClient client, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            if (client.Status != RavenSmsClientStatus.Connected)
                return Result.Failure()
                    .WithMessage("the client is not connected")
                    .WithCode("client_disconnected");

            if (client.ConnectionId is null)
                return Result.Failure()
                    .WithMessage("the client connection id is null or empty")
                    .WithCode("invalid_client_connection_id");

            await hub.Clients.Client(client.ConnectionId)
                .SendAsync("forceDisconnect", reason, cancellationToken: cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure()
                .WithErrors(ex);
        }
    }
}