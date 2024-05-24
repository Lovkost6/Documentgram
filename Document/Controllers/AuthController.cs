using System.Security.Claims;
using Document.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Document.Controllers;

[ApiController]
[Route("/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationContext _context;

    public AuthController(ApplicationContext context)
    {
        _context = context;
    }
    
    [HttpGet("Notauthorize")]
    public async Task<ActionResult> ErrorNotAuth()
    {
        return Unauthorized();
    }
    
    [HttpGet("Forbidden")]
    public async Task<ActionResult> ErrorForbidden ()
    {
        return StatusCode(403);
    }
    
    [HttpGet]
    public async Task<ActionResult<long>> SignIn(string login,string password)
    {
        var user = await _context.Users.Where(l => l.Login == login && l.Password == password).FirstOrDefaultAsync();
        
        if (user == null)
        {
            return NotFound();
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            // Refreshing the authentication session should be allowed.

            IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            IssuedUtc = DateTimeOffset.Now,
            // The time at which the authentication ticket was issued.

        };
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);
        
        
        
        //Response.Headers.Append("AuthUserId", user.Id.ToString());
        //Response.Cookies.Append("AuthUserId",user.Id.ToString());
        return Ok();
    }
    
    [Authorize]
    [HttpGet("logout")]
    public async Task LogOut()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
    
    
}