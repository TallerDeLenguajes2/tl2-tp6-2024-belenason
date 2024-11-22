

public interface IUserRepository
{
    public User GetUser(string usuario, string contra);
    public void AltaUsuario(User usuario);
}