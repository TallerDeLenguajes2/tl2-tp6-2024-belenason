public class InMemoryUserRepository :IUserRepository
{
    private readonly List<User> _users;
    public InMemoryUserRepository()
    {
        _users = new List<User>
        {
            new User {Id = 1, Username = "admin", Password = "password123", AccessLevel = AccessLevel.Admin},
            new User {Id = 1, Username = "cliente", Password = "password123", AccessLevel = AccessLevel.Cliente}
        };
    }

    public User GetUser (string username, string password)
    {
        return _users.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
}

public interface IUserRepository
{
    User GetUser (string username, string password);
}