using System.Security.Principal;

public class User
{
    public int IdUsuario {get; set;}
    public string Nombre {get; set;}
    public string Usuario {get; set;} = string.Empty;
    public string Contrasena {get; set;} = string.Empty;
    public AccessLevel Rol {get; set;}

    public User()
    {
    }
    public User(CrearUsuarioViewModel usuVM)
    {
        Usuario = usuVM.Usuario;
        Nombre = usuVM.Nombre;
        Contrasena = usuVM.Contrasena;
        Rol = usuVM.Rol;
    }
}

public enum AccessLevel
{
    Admin,
    Cliente
}