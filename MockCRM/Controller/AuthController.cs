using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;
using MockCRM.Services;

namespace MockCRM.Controller;

[ApiController]
[Route("api/authorization/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly CrmDbContext _context;

    public AuthController(CrmDbContext context , TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registerRequest.Username || u.Email == registerRequest.Email))
            return BadRequest("Username already exists");
        var user = new User
        {
            Email = registerRequest.Email,
            Username = registerRequest.Username,
            PasswordHash = PasswordHasher.HashPassword(registerRequest.Password),
            Role = registerRequest.Role,
            CreatedDate = DateTime.Now
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok($"User details : {user }  \n  Registered Successfully");
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == loginRequest.EmailOrUsername || u.Username == loginRequest.EmailOrUsername);
        if (user == null || !PasswordHasher.VerifyHashedPassword(loginRequest.Password, user.PasswordHash))
            return Unauthorized("Invalid Username &/or password");
        var token = _tokenService.GenerateToken(user.Id.ToString(), user.Role.ToString());
        return Ok(new { token });
    }
}