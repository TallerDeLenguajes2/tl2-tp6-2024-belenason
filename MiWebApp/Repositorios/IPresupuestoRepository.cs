public interface IPresupuestoRepository
{
    
    public void CrearPresupuesto(Presupuesto presupuesto);
    public List<Presupuesto> ObtenerPresupuestos();
    public Presupuesto ObtenerPresupuestoPorId(int id);
    public void AgregarProductoCantidadPresupuesto(int idProducto, int cantidad, int idPresupuesto);
    public void ModificarPresupuesto(Presupuesto presupuesto);
    public void EliminarpresupuestoPorId(int idPresupuesto);
    public void EliminarProducto(int idPresupuesto, int idProducto);
    public List<Producto> ListarProductosAsociadosAPresupuesto(int IdPresupuesto);
}