using Microsoft.AspNetCore.Identity;
using TaskManagment.Infrastructure.Models;

namespace TaskManagment.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    // temporary solution
    private Dictionary<string, User> _users { get; }

    public AuthenticationService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _users = new Dictionary<string, User>();

        // for tests
        Task.Run(() => RegisterAsync("Test", "12345"));
    }

    public bool IsAuthenticated { get; private set; }

    public Task<bool> LoginAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        User user = null;
        IsAuthenticated = false;

        if (!_users.TryGetValue(username, out user))
        {
            throw new NullReferenceException($"User with username `{username}` is not exist");
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (verifyResult is not PasswordVerificationResult.Success)
        {
            throw new ArgumentOutOfRangeException($"Password is not correct");
        }

        IsAuthenticated = true;

        return Task.FromResult(IsAuthenticated);
    }

    public void Logout()
    {
        IsAuthenticated = false;
    }

    public Task<bool> RegisterAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        if (_users.ContainsKey(username))
        {
            throw new ArgumentOutOfRangeException(nameof(username));
        }

        var user = new User { Username = username };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _users.Add(username, user);

        return Task.FromResult(true);
    }


}

