using Document.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Document.HubConf;

[Authorize]
public class MessageHub : Hub
{
    public async Task SendRecipients(List<string> usersId  ,string message)
    {
        //Clients.Users(usersId).SendAsync("SendRecipients",message);
    }
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
        await base.OnConnectedAsync();
    }
}