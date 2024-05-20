using Document.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Document.Controllers;

[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationContext _context;

    public AuthController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<long>> SignIn(string login,string password)
    {
        var user = await _context.Users.Where(l => l.Login == login && l.Password == password).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        Response.Headers.Append("AuthUserId", user.Id.ToString());
        return NoContent();
    }
    
    
}