using Microsoft.Data.Sqlite;
class ClienteRepository
{
    public void CrearCliente(AltaClienteViewModel cliente)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@nombre, @email, @tel)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@tel", cliente.Telefono);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public void ModificarCliente(ModificarClienteViewModel cliente)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"UPDATE Clientes SET Nombre = @nombre, Email = @mail, Telefono = @tel WHERE IdCliente = @idCliente";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@mail", cliente.Email);
            command.Parameters.AddWithValue("@tel", cliente.Telefono);
            command.Parameters.AddWithValue("@idCliente", cliente.ClienteId);

            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public List<Cliente> ListarClientes()
    {

        List<Cliente> clientes = new List<Cliente>();

        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"SELECT * FROM Clientes";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente(Convert.ToInt32(reader["IdCliente"]), reader["Nombre"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
                    clientes.Add(cliente);
                }   
            }
            
            connection.Close();
            
        }
        return clientes;
    }

    public Cliente ObtenerClientePorId(int idCliente)
    {

        Cliente cliente;

        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"SELECT * FROM Clientes WHERE IdCliente = @idCliente";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idCliente", idCliente);

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cliente = new Cliente(Convert.ToInt32(reader["IdCliente"]), reader["Nombre"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
                } else
                {
                    cliente = null;
                }
            }
            
            connection.Close();
            
        }
        return cliente;
    }

    public void EliminarClientePorId(int idCliente)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"DELETE FROM Clientes WHERE IdCliente = @IdCliente";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IdCliente", idCliente);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

}