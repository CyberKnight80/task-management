using System;
namespace TaskManagement.Server.Models;

public class RefreshTokenModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
