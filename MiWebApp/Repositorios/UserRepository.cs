

using Microsoft.Data.Sqlite;

public class UserRepository : IUserRepository
{
    string connectionString;

    public UserRepository()
    {
        connectionString = @"Data Source = db/Tienda.db;Cache=Shared";
    }

    public User GetUser(string usuario, string contra)
    {
        User user = null;

        string query = @"SELECT * FROM Usuarios WHERE Usuario = @usuario AND Contrasena = @contra ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@usuario", usuario);
            command.Parameters.AddWithValue("@contra", contra);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User();
                    user.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                    user.Nombre = reader["Nombre"].ToString();
                    user.Usuario = reader["Usuario"].ToString();
                    user.Contrasena = reader["Contrasena"].ToString();
                    user.Rol = (AccessLevel)Convert.ToInt32(reader["IdRol"]);;
                }

            }
            connection.Close();            
        }
        if (user == null)
        {
            throw new Exception("Usuario inexistente.");
        }
        return user;
    }

    public void AltaUsuario(User usuario)
    {
        string query = @"INSERT INTO Usuarios (Nombre, Usuario, Contrasena, IdRol) VALUES (@nombre, @usu, @contra, @rol)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@usu", usuario.Usuario);
            command.Parameters.AddWithValue("@contra", usuario.Contrasena);
            command.Parameters.AddWithValue("@rol", (int)usuario.Rol);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

}