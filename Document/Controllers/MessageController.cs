using Document.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Document.Controllers;
[ApiController]
[Route("v1/documents")]
public class MessageController : ControllerBase
{
    private readonly ApplicationContext _context;

    public MessageController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet("/sent-messages")]
    public async Task<ActionResult<Object>> GetAllSentMessage([FromHeader]long? authUserId)
    {
        
        if (authUserId == null)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(authUserId);
        if (user == null)
        {
            return BadRequest();
        }

        var recipient = await _context.Messages.Where(x => x.OwnerId == authUserId).ToListAsync();
        var message = new List<Object>();
        foreach (var k in recipient)
        {
            var names = await _context.MessageRecipients.Where(x => x.MessageId == k.Id)
                .Include(x => x.User)
                .Select(x => x.User.Name)
                .ToListAsync();
            message.Add(new {k.Id,k.Name, k.Description, k.PicturePath, names});
        }
        
        return Ok(message);
        //return  await _context.Messages.Where(message => message.OwnerId == authUserId).ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<List<Message>>> GetAllRecipientMessage([FromHeader] long? authUserId)
    {
        if (authUserId == null)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(authUserId);
        if (user == null)
        {
            return BadRequest();
        }
        
        var listMessages = await _context.MessageRecipients
            .Where(user => user.UserId == authUserId)
            .Include(name => name.Message)
            .ToListAsync();
        
        var result = new List<Object>();
        
        foreach (var x in listMessages)
        {
            result.Add(new {x.Message.Name,x.Message.Description,x.Message.PicturePath,  });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Message>> CreateMessage( MessageCreate? message, [FromHeader]long? authUserId)
    {
        if (message == null || authUserId == null || message.RecipientsId == null || message.RecipientsId.Count == 0)
        {
            return BadRequest();
        }
        
        var user = await _context.Users.FindAsync(authUserId);
        if (user == null)
        {
            return BadRequest();
        }

        var newMessage = new Message{Name = message.Name,Description = message.Description,PicturePath = message.PicturePath, OwnerId = authUserId.Value};
        await _context.Users.FindAsync(authUserId);
        
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        for (int i = 0; i < message.RecipientsId.Count(); i++)
        {
            var recipients = new MessageRecipient{MessageId = newMessage.Id, UserId = message.RecipientsId[i]};
            _context.MessageRecipients.Add(recipients);
        }

        await _context.SaveChangesAsync();

        var names = await _context.MessageRecipients
            .Where(message => message.MessageId == newMessage.Id)
            .Include(name => name.User)
            .Select(name => name.User.Name)
            .ToListAsync();
        
        return Ok(new {newMessage.Name, newMessage.Description, newMessage.PicturePath, OwnerName = newMessage.Owner.Name, names});
    }

    [HttpDelete("{messageId}")]
    public async Task<ActionResult> DeleteMessage(long messageId, [FromHeader]long? authUserId)
    {
        if (authUserId == null)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(authUserId);
        if (user == null)
        {
            return BadRequest();
        }

        var message = await _context.Messages.FindAsync(messageId);
        if (message != null) _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return Ok(message);
    } 
}