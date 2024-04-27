using System;
namespace TaskManagment.Infrastructure.DataContracts;

public class TokenResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}

