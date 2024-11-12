using Microsoft.Data.Sqlite;
class PresupuestoRepository
{
    public void CrearPresupuesto(Presupuesto presupuesto) //NO EJECUTAR SIN REVISAR
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"INSERT INTO Presupuestos (FechaCreacion, ClienteId) VALUES (@FechaCreacion, @ClienteId)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@ClienteId", presupuesto.Cliente.ClienteId);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

 public List<Presupuesto> ObtenerPresupuestos()
    {
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            idPresupuesto,
            FechaCreacion,
            P.ClienteId, 
            COALESCE(C.Nombre, 'No se asigno cliente') AS Cliente,
            C.Email,
            C.Telefono
        FROM 
            Presupuestos P
        LEFT JOIN 
            Clientes C ON P.ClienteId = C.IdCliente;";

        Cliente cliente = null; //Está bien que sea nulo si no hay ningún cliente?

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(!reader.IsDBNull(reader.GetOrdinal("ClienteId")))
                    {
                        cliente = new Cliente(Convert.ToInt32(reader["ClienteId"]), reader["Cliente"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
                    }
                    Presupuesto presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }

    /*public List<Presupuesto> ListarPresupuestos() //No lo uso
    {
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            P.idPresupuesto,
            P.NombreDestinatario,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad
        FROM 
            Presupuestos P
        LEFT JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        LEFT JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        ORDER BY P.idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                int id = -1; 
                Presupuesto ultPresupuesto = null;

                while (reader.Read())
                {
                    if (id == -1 || id != Convert.ToInt32(reader["idPresupuesto"]))
                    {
                        if (id != -1) presupuestos.Add(ultPresupuesto);
                        
                        ultPresupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]).Date);
                    }

                    // Manejo de posibles valores NULL en Producto y Detalle
                    if (reader["idProducto"] != DBNull.Value && reader["Producto"] != DBNull.Value && reader["Precio"] != DBNull.Value && reader["Cantidad"] != DBNull.Value)
                    {
                        Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));

                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));

                        ultPresupuesto.Detalle.Add(detalle);
                    }

                    id = Convert.ToInt32(reader["idPresupuesto"]);
                }

                if (ultPresupuesto != null) presupuestos.Add(ultPresupuesto);
            }
            connection.Close();
        }
        return presupuestos;
    }*/

    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
        Presupuesto presupuesto = null;
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            P.idPresupuesto,
            P.ClienteId,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad,
            COALESCE(C.Nombre, 'No se asigno cliente') AS cliente,
            C.Email,
            C.Telefono
        FROM 
            Presupuestos P
        LEFT JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        LEFT JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        LEFT JOIN Clientes C ON P.ClienteId = C.IdCliente
        WHERE 
            P.idPresupuesto = @id;";

        Cliente cliente = null; //Está bien que sea nulo si no hay ningún cliente?

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int cont = 1;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(cont == 1)
                    {
                        if(!reader.IsDBNull(reader.GetOrdinal("ClienteId")))
                        {
                            cliente = new Cliente(Convert.ToInt32(reader["ClienteId"]), reader["Cliente"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
                        }
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    if(!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                    {
                        Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto,Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalle.Add(detalle);
                    }
                    cont++;
                }
            }
            connection.Close();
        }
        return presupuesto;
    }

    public void AgregarProductoCantidadPresupuesto(int idProducto, int cantidad, int idPresupuesto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresup, @idProd, @Cantidad) ON CONFLICT(idPresupuesto, idProducto) DO UPDATE SET Cantidad = Cantidad + @Cantidad;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresup", idPresupuesto);
            command.Parameters.AddWithValue("@idProd", idProducto);
            command.Parameters.AddWithValue("@Cantidad", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    public void ModificarPresupuesto(Presupuesto presupuesto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"UPDATE Presupuestos SET FechaCreacion = @fecha, ClienteId = @ClienteId WHERE idPresupuesto = @Id";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@ClienteId", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();            
        }
    }

    public void EliminarpresupuestoPorId(int idPresupuesto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = $"DELETE FROM presupuestos WHERE idPresupuesto = @idPresupuesto";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteCommand command2 = new SqliteCommand(query2, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command2.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery();

            connection.Close();
            
        }
    }

    public void EliminarProducto(int idPresupuesto, int idProducto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idP AND idProducto = @idPR";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idP", idPresupuesto);
            command.Parameters.AddWithValue("@idPR", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Producto> ListarProductosAsociadosAPresupuesto(int IdPresupuesto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";
        List<Producto> productos = new List<Producto>();

        string query = @"SELECT p.IdProducto, p.Descripcion 
                        FROM Productos p
                        INNER JOIN PresupuestosDetalle pd ON p.IdProducto = pd.IdProducto
                        WHERE pd.IdPresupuesto = @IdPresupuesto";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", IdPresupuesto);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        IdProducto = reader.GetInt32(0),
                        Descripcion = reader.GetString(1)
                    };
                    productos.Add(producto);
                }
            }
            connection.Close();
        }
        return productos;
    }


}

/*    public bool CrearPresupuesto(Presupuesto presupuesto)
    {
        ProductosRepository repoProductos = new ProductosRepository();
        foreach (var detalle in presupuesto.Detalle)
        {
            if (repoProductos.ObtenerProductoPorId(detalle.Producto.IdProducto) == null)
            {
                return false;
            }
            
        }
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
        VALUES (@destinatario, @fecha)";

        string query2 = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
        VALUES (@idP, @idPr, @cant)";

        string query3 = @"SELECT MAX(idPresupuesto) AS idMax FROM Presupuestos;";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@idPresupuesto", presupuesto.IdPresupuesto);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            SqliteCommand command3 = new SqliteCommand(query3, connection);
            using (SqliteDataReader reader = command3.ExecuteReader())
            {
                if (reader.Read())
                {
                    foreach (var detalle in presupuesto.Detalle)
                    {
                        SqliteCommand command2 = new SqliteCommand(query2,connection);
                        command2.Parameters.AddWithValue("@idP", Convert.ToInt32(reader["idMax"]));
                        command2.Parameters.AddWithValue("@idPr", detalle.Producto.IdProducto);
                        command2.Parameters.AddWithValue("@cant", detalle.Cantidad);
                        command2.ExecuteNonQuery();
                    }
                }
            }
            connection.Close();            
        }
        return true;
    }*/