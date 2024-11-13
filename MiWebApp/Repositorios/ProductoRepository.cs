using Microsoft.Data.Sqlite;
class ProductoRepository
{
    public void CrearProducto(AltaProductoViewModel producto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public void ModificarProducto(ModificarProductoViewModel producto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@idProducto", producto.IdProducto);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);

            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public List<Producto> ListarProductos()
    {

        List<Producto> productos = new List<Producto>();

        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"SELECT * FROM Productos";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                    productos.Add(producto);
                }   
            }
            
            connection.Close();
            
        }
        return productos;
    }

    public Producto ObtenerProductoPorId(int idProd)
    {

        Producto producto;

        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"SELECT * FROM Productos WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idProducto", idProd);

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto = new Producto(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                } else
                {
                    producto = null;
                }
            }
            
            connection.Close();
            
        }
        return producto;
    }

    public void EliminarProductoPorId(int idProducto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"DELETE FROM productos WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

}