using Microsoft.AspNetCore.SignalR;

namespace SMS.Net.Channel.RavenSMS;

public class RavenSmsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Task.Delay(1);

        var context = this.Context;
        var context1 = this.Clients;
        var context2 = this.Groups;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("SendMessage", user, message);
    }
}