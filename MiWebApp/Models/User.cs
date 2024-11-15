using System.Security.Principal;

public class User
{
    public int Id {get; set;}
    public string Username {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public AccessLevel AccessLevel {get; set;}
}

public enum AccessLevel
{
    Admin,
    Cliente
}