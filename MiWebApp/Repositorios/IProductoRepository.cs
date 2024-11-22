public interface IProductoRepository
{
    public void CrearProducto(Producto producto);
    public void ModificarProducto(Producto producto);
    public List<Producto> ListarProductos();
    public Producto ObtenerProductoPorId(int idProd);
    public void EliminarProductoPorId(int idProducto);
}