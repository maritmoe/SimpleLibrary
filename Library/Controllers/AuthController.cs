using Library.Data;
using Library.DTOs;
using Library.Enums;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    private readonly DatabaseContext _context;

    public AuthController(IAuthService authService, ILogger<AuthController> logger, DatabaseContext context)
    {
        _authService = authService;
        _logger = logger;
        _context = context;
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Login(model);
            if (status == 0)
                return BadRequest(message);
            User? user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user is null)
            {
                return Unauthorized();
            }
            return Ok(new LogInResponse(message, user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Registeration(model, Role.User);
            if (status == 0)
            {
                return BadRequest(message);
            }
            return CreatedAtAction(nameof(Register), new RegisteredResponse(model));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}