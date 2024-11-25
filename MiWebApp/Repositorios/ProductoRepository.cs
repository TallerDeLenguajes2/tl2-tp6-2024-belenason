using Microsoft.Data.Sqlite;
class ProductoRepository : IProductoRepository
{

    private readonly string _ConnectionString;

    public ProductoRepository(string CadenaDeConexion)
    {
        _ConnectionString = CadenaDeConexion;
    }

    public void CrearProducto(Producto producto)
    {
        string query = $"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public void ModificarProducto(Producto producto)
    {
        string query = $"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@idProducto", producto.IdProducto);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);

            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            
            if (rowsAffected == 0)
                throw new Exception($"No se encontró ningún producto con id = {producto.IdProducto}.");
        }
    }

    public List<Producto> ListarProductos()
    {
        List<Producto> productos = new List<Producto>();

        string query = $"SELECT * FROM Productos";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
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

        string query = $"SELECT * FROM Productos WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
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
        if (producto == null)
            throw new Exception("Producto inexistente.");

        return producto;
    }

    public void EliminarProductoPorId(int idProducto)
    {
        string query = $"DELETE FROM productos WHERE idProducto = @idProducto";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            
            if (rowsAffected == 0)
                throw new Exception($"No se encontró ningún producto con id = {idProducto}.");
        }
    }

}