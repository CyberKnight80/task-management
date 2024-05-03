using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Infrastructure.Services;

namespace TaskManagementWin.Services;

internal class SecureStorageService : ISecureStorageService
{
    public Task<string> GetAsync(SecureStorageKey key)
    {
        var encryptedValue = Properties.Settings.Default[key.ToString()] as string;
        var descryptedValue = encryptedValue != null ? Unprotect(encryptedValue) : null;
        return Task.FromResult(descryptedValue);
    }

    public Task RemoveAll()
    {
        foreach (SettingsProperty prop in Properties.Settings.Default.Properties)
        {
            Properties.Settings.Default[prop.Name] = null;
        }

        Properties.Settings.Default.Save();
        return Task.CompletedTask;
    }

    public Task RemoveAsync(SecureStorageKey key)
    {
        Properties.Settings.Default[key.ToString()] = null;
        Properties.Settings.Default.Save();
        return Task.CompletedTask;
    }

    public Task SetAsync(SecureStorageKey key, string value)
    {
        var encryptedValue = Protect(value);
        Properties.Settings.Default[key.ToString()] = encryptedValue;
        Properties.Settings.Default.Save();
        return Task.CompletedTask;
    }

    private string Protect(string data)
    {
        var userData = Encoding.UTF8.GetBytes(data);
        var protectedData = ProtectedData.Protect(userData, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(protectedData);
    }

    private string Unprotect(string protectedData)
    {
        var protectedBytes = Convert.FromBase64String(protectedData);
        var userData = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(userData);
    }
}
