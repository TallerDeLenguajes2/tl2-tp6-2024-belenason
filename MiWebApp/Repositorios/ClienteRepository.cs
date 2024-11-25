using Microsoft.Data.Sqlite;
class ClienteRepository : IClienteRepository
{
    public void CrearCliente(Cliente cliente)
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

    public void ModificarCliente(Cliente cliente)
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
            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();
            
            if (rowsAffected == 0)
            throw new Exception($"No se encontró ningún cliente con IdCliente = {cliente.ClienteId}.");
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
        //if (clientes.Count == 0)
        //    throw new Exception("No se encontraron clientes en la base de datos.");
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
        if (cliente == null)
            throw new Exception($"No se encontró ningún cliente con IdCliente = {idCliente}");
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
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            if (rowsAffected == 0)
                throw new Exception($"No se encontró ningún cliente con IdCliente = {idCliente} para eliminar.");
        }
    }

}