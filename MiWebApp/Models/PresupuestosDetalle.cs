
public class PresupuestosDetalle
{
    public Producto Producto { get; set; }
    public int Cantidad { get; set; }


    public PresupuestosDetalle(Producto producto, int cantidad)
    {
        Producto = producto;
        Cantidad = cantidad;
    }

    public PresupuestosDetalle()
    {

    }
}