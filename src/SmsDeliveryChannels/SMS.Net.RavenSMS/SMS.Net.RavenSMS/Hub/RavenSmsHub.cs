using Microsoft.AspNetCore.SignalR;

namespace SMS.Net.Channel.RavenSMS;

public class RavenSmsHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("SendMessage", user, message);
    }
}