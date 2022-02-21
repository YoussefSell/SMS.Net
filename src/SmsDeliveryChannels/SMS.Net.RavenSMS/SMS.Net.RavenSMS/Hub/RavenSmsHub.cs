namespace SMS.Net.Channel.RavenSMS;

public class RavenSmsHub : Hub, IRavenSmsClientConnector
{
    private readonly ILogger _logger;
    private readonly IRavenSmsClientsManager _clientsManager;

    public RavenSmsHub(IRavenSmsClientsManager manager, ILogger<RavenSmsHub> logger)
    {
        _logger = logger;
        _clientsManager = manager;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // disconnecting the client
        _logger.LogInformation("client associated with connection Id: {connectionId} has been disconnected", Context.ConnectionId);
        await _clientsManager.ClientDisconnectedAsync(Context.ConnectionId);
    }

    public async Task ClientConnectedAsync(string clientId)
    {
        // get the client associated with the given id
        var client = await _clientsManager.FindClientByIdAsync(clientId);
        if (client is null)
        {
            await Clients.Caller.SendAsync("forceDisconnect", "client_not_found");
            return;
        }

        // check if the client already connected
        if (client.Status == RavenSmsClientStatus.Connected)
        {
            if (client.ConnectionId != Context.ConnectionId)
                await Clients.Caller.SendAsync("forceDisconnect", "client_already_connected");

            return;
        }

        // attach the client to the current connection
        _logger.LogInformation("connecting client with Id: {clientId}, connection Id: {connectionId}", client.Id, Context.ConnectionId);
        await _clientsManager.ClientConnectedAsync(client, Context.ConnectionId);
    }

    public async Task<Result> SendSmsMessageAsync(RavenSmsClient client, RavenSmsMessage message)
    {
        if (client.ConnectionId is null)
            throw new ArgumentNullException($"{nameof(client)}.{nameof(client.ConnectionId)}");

        try
        {
            await Clients.Client(client.ConnectionId).SendAsync("sendSmsMessage", new
            {
                content = message.Body,
                from = message.From,
                to = message.To,
            });

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure()
                .WithErrors(ex);
        }
    }
}