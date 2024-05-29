using Document.HubConf;
using Document.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Document.Controllers;

[Authorize]
[ApiController]
[Route("v1/documents")]
public class MessageController : ControllerBase
{
    private readonly ApplicationContext _context;
    private IHubContext<MessageHub> _chatHubContext;
    private IMemoryCache cache;
    public MessageController(ApplicationContext context, IHubContext<MessageHub> chatHubContext, IMemoryCache cache)
    {
        _context = context;
        _chatHubContext = chatHubContext;
        this.cache = cache;
    }
    
    
    [HttpGet("sent-messages")]
    public async Task<ActionResult<Object>> GetAllSentMessage(int page = 1, int size = 10)
    {
        var authUserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
        var cacheKey = new {method = "sent-messages",authUserId = authUserId, skip = (page - 1) * size, size = size};
        cache.TryGetValue(cacheKey, out List<object>? sentMessages);
        
        if (sentMessages != null)
        {
            return sentMessages.Skip((page - 1)*size).Take(size).ToList();
        } 
        var messages = await _context.Messages.Where(x => x.OwnerId == authUserId).ToListAsync();
        var message = new List<Object>();

        foreach (var k in messages)
        {
            var names = await _context.MessageRecipients.Where(x => x.MessageId == k.Id)
                .Include(x => x.User)
                .Select(x => new {x.User.Name, x.State})
                .ToListAsync();
            string file = null;
            if (!string.IsNullOrEmpty(k.PicturePath))
            {
                var bytes = System.IO.File.ReadAllBytes("D:\\RiderProjects\\Document\\Document" + k.PicturePath);
                file = "data:image/png;base64, " + Convert.ToBase64String(bytes);
            }

            message.Add(new { k.Id, k.Name, k.Description, file, names });
        }
        var cacheOptions = new MemoryCacheEntryOptions()
        {
            // кэширование в течение 1 минуты
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
            // низкий приоритет
            Priority = 0,
        };
        
        cache.Set(cacheKey, message, cacheOptions);
        var paginatedMessage = message.Skip((page - 1) * size).Take(size).ToList();
        return Ok(paginatedMessage);
    }

    [HttpGet("recipient-messages")]
    public async Task<ActionResult<object>?> GetAllRecipientMessage(int page = 1, int size = 10)
    {
        var authUserId = Convert.ToInt32( User.Claims.FirstOrDefault()?.Value);
        var cacheKey = new {method = "recipient-messages",authUserId = authUserId, skip = (page - 1) * size, size = size};
        cache.TryGetValue(cacheKey, out List<object>? messageRecipients);
        
        if (messageRecipients != null)
        {
            return messageRecipients.Skip((page - 1)*size).Take(size).ToList();
        } 
        
        
        var listMessages = await _context.MessageRecipients
            .Where(user => user.UserId == authUserId)
            .Include(name => name.Message)
            .ThenInclude(name => name.Owner)
            .ToListAsync();

        var result = new List<Object>();

        foreach (var x in listMessages)
        {
            var names = await _context.MessageRecipients.Where(k => k.MessageId == x.MessageId)
                .Include(x => x.User)
                .Select(x => x.User.Name)
                .ToListAsync();
            string file = null;
            if (!string.IsNullOrEmpty(x.Message.PicturePath))
            {
                var bytes = System.IO.File.ReadAllBytes("D:\\RiderProjects\\Document\\Document" + x.Message.PicturePath);
                file = "data:image/png;base64, " + Convert.ToBase64String(bytes);
            }
            
            
            result.Add(new
                { x.MessageId,x.Message.Name, x.Message.Description, file, Owner = x.Message.Owner.Name, names });
        }
        
        var cacheOptions = new MemoryCacheEntryOptions()
        {
            // кэширование в течение 1 минуты
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
            // низкий приоритет
            Priority = 0,
        };
        
        var paginationResult = result.Skip((page - 1)*size).Take(size).ToList();
        cache.Set(cacheKey, paginationResult, cacheOptions);
        return Ok(paginationResult);
    }

    [HttpPost]
    public async Task<ActionResult<Message>> CreateMessage([FromForm] MessageCreate? message)
    {
        var authUserId = Convert.ToInt64(User.Claims.FirstOrDefault().Value);
        string? path = null;
        if (message.PicturePath != null)
        {
            path = "\\Images\\" + (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds +
                   message.PicturePath.FileName;

            using (var fileStream = new FileStream("D:\\RiderProjects\\Document\\Document" + path, FileMode.Create))
            {
                await message.PicturePath.CopyToAsync(fileStream);
            }
        }
        

        var newMessage = new Message
        {
            Name = message.Name, Description = message.Description, PicturePath = path, OwnerId = authUserId
        };
        await _context.Users.FindAsync(authUserId);


        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        for (int i = 0; i < message.RecipientsId.Count(); i++)
        {
            var recipients = new MessageRecipient { MessageId = newMessage.Id, UserId = message.RecipientsId[i] };
            _context.MessageRecipients.Add(recipients);
        }

        await _context.SaveChangesAsync();

        var names = await _context.MessageRecipients
            .Where(message => message.MessageId == newMessage.Id)
            .Include(name => name.User)
            .Select(name => name.User.Name)
            .ToListAsync();

        _chatHubContext.Clients.Users(message.RecipientsId
                .Select(x => x.ToString()))
            .SendAsync("SendRecipients", $"{newMessage.Owner.Name} прислал вам сообщенние: {message.Name}");
        
        return Ok(new
        {
            newMessage.Name, newMessage.Description, newMessage.PicturePath, OwnerName = newMessage.Owner.Name,
            names
        });
    }

    [HttpDelete("{messageId}")]
    public async Task<ActionResult> DeleteMessage(long messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message != null) _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return Ok(message);
    }

    [HttpPatch]
    public async Task<ActionResult> ChangeState(MessageStateUpdate messageStateUpdate)
    {
        var authUserId = Convert.ToInt32( User.Claims.FirstOrDefault().Value);
        var messageRecipients =  await _context.MessageRecipients
            .Where(x => x.MessageId == messageStateUpdate.Id && x.UserId == authUserId).FirstOrDefaultAsync();
        messageRecipients.State = (int)messageStateUpdate.State;
        await _context.SaveChangesAsync();
        return Ok(messageRecipients);
    }
}