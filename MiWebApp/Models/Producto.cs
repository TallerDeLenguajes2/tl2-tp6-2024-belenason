
public class Producto
{
    public int IdProducto {get; set;}
    public string Descripcion {get; set;}
    public int Precio {get; set;}

    public Producto(int idProducto, string descripcion, int precio)
    {
        this.IdProducto = idProducto;
        this.Descripcion = descripcion;
        this.Precio = precio;
    }


    public Producto()
    {

    }

}
