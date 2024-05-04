using System;
namespace TaskManagement.Infrastructure.DataContracts;

public class RegisterRequest
{
    public string Username { get; set; }

    public string Password { get; set; }
}

