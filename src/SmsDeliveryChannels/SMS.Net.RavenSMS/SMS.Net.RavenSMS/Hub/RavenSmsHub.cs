using Microsoft.AspNetCore.SignalR;

namespace SMS.Net.Channel.RavenSMS;

public class RavenSmsHub : Hub, IRavenSmsClientConnector
{
    private readonly IRavenSmsManager _manager;

    public RavenSmsHub(IRavenSmsManager manager)
        => _manager = manager;

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _manager.ClientDisconnectedAsync(Context.ConnectionId);
    }

    public async Task ClientConnectedAsync(string clientId)
    {
        // get the client associated with the given id
        var client = await _manager.FindClientByIdAsync(clientId);
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
        await _manager.ClientConnectedAsync(client, Context.ConnectionId);
    }
}