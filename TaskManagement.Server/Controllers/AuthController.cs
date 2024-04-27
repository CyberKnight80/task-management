using Microsoft.AspNetCore.Mvc;
using TaskManagement.Server.Models;
using TaskManagement.Server.Services;
using TaskManagment.Infrastructure.DataContracts;

namespace TaskManagement.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ConfigurationService _configurationService;
    private readonly AppDbContext _context;

    public AuthController(
        AuthService authService,
        ConfigurationService configurationService,
        AppDbContext context)
    {
        _authService = authService;
        _configurationService = configurationService;
        _context = context;
    }

    // POST api/auth/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        // Проверяем учетные данные пользователя
        var user = _context.Users.SingleOrDefault(u => u.Username == loginModel.Username);
        if (user == null ||
            !_authService.VerifyPasswordHash(
                loginModel.Password, user.PasswordHash, user.Salt))
        {
            return Unauthorized("Invalid username or password.");
        }

        // Генерируем access и refresh токены
        var accessToken = _authService.GenerateAccessToken(user);
        var refreshToken = _authService.GenerateRefreshToken();

        // Сохраняем refresh токен в базе данных
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
            _configurationService.JwtRefreshTokenExpirationDays);
        _context.SaveChanges();

        // Возвращаем токены клиенту
        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    // POST api/auth/refresh
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenModel refreshTokenModel)
    {
        var principal = _authService.GetPrincipalFromExpiredToken(refreshTokenModel.AccessToken);
        var username = principal.Identity.Name; // Извлекаем имя пользователя из принципала
        var user = _context.Users.SingleOrDefault(u => u.Username == username);

        // Проверяем валидность refresh токена
        if (user == null || user.RefreshToken != refreshTokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid token.");
        }

        // Генерируем новые токены
        var newAccessToken = _authService.GenerateAccessToken(user);
        var newRefreshToken = _authService.GenerateRefreshToken();

        // Обновляем refresh токен в базе данных
        user.RefreshToken = newRefreshToken;
        // TODO: save new life time
        _context.SaveChanges();

        // Возвращаем новые токены клиенту
        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel registerModel)
    {
        // Проверяем, существует ли уже пользователь с таким именем
        if (_context.Users.Any(u => u.Username == registerModel.Username))
        {
            return BadRequest("Username already exists.");
        }

        // Создаем хеш пароля
        _authService.CreatePasswordHash(registerModel.Password,
            out string passwordHash, out string passwordSalt);

        // Создаем нового пользователя
        var user = new User
        {
            Username = registerModel.Username,
            PasswordHash = passwordHash,
            Salt = passwordSalt
        };

        // Сохраняем пользователя в базе данных
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered successfully.");
    }
}

