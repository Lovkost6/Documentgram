using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Document.Utils;

public class CustomUserIdProvider : IUserIdProvider
{
    public virtual string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Sid)?.Value;
    }
}