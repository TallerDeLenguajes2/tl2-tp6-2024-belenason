public class Presupuesto
{
    public int IdPresupuesto {get; set;}
    public Cliente Cliente {get; set;}
    public List<PresupuestosDetalle> Detalle {get; set;}
    public DateTime FechaCreacion { get; set; }

    public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fechaCreacion)
    {
        this.IdPresupuesto = idPresupuesto;
        this.Cliente = cliente;
        this.Detalle = new List<PresupuestosDetalle> ();
        this.FechaCreacion = fechaCreacion;
    }
    public Presupuesto() 
    {
        this.Detalle = new List<PresupuestosDetalle>();
    }

    public Presupuesto(AltaPresupuestoViewModel presuVM)
    {
        this.Cliente = new Cliente();
        this.Cliente.ClienteId = presuVM.ClienteId;
        this.FechaCreacion = presuVM.FechaCreacion;
    }

    public Presupuesto(ModificarPresupuestoViewModel presuVM)
    {
        IdPresupuesto = presuVM.IdPresupuesto;
        Cliente = new Cliente();
        Cliente.ClienteId = presuVM.ClienteId;
        FechaCreacion = presuVM.FechaCreacion;
    }
    
    public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fecha, List<PresupuestosDetalle> detalle)
    {
        this.IdPresupuesto = idPresupuesto;
        this.Cliente = cliente;
        this.Detalle = detalle;
        this.FechaCreacion = fecha;
    }

    public double MontoPresupuesto()
    {
        int monto = Detalle.Sum(d => d.Cantidad*d.Producto.Precio);
        return monto;
    }

    public double MontoPresupuestoConIVA()
    {
        return MontoPresupuesto()*1.21;
    }

    public int CantidadProductos()
    {
        return Detalle.Sum(d => d.Cantidad);
    }
}