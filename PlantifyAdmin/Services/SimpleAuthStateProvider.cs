using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace PlantifyAdmin.Services;

public sealed class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    private ClaimsPrincipal _currentUser = Anonymous;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public Task<bool> LoginAsync(string username, string password)
    {
        var normalizedUsername = username?.Trim() ?? string.Empty;
        var normalizedPassword = password?.Trim() ?? string.Empty;

        // Minimal starter auth. Replace with Identity/DB validation later.
        if (string.Equals(normalizedUsername, "admin", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(normalizedPassword, "admin123", StringComparison.Ordinal))
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, normalizedUsername),
                    new Claim(ClaimTypes.Role, "Admin")
                },
                authenticationType: "SimpleAuth");

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task LogoutAsync()
    {
        _currentUser = Anonymous;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}
