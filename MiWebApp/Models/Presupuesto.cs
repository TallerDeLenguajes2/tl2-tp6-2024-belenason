public class Presupuesto
{
    public int IdPresupuesto {get; set;}
    public string NombreDestinatario {get; set;}
    public List<PresupuestosDetalle> Detalle {get; set;}
    public DateTime FechaCreacion { get; set; }

    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
    {
        this.IdPresupuesto = idPresupuesto;
        this.NombreDestinatario = nombreDestinatario;
        this.Detalle = new List<PresupuestosDetalle> ();
        this.FechaCreacion = fechaCreacion;
    }
    public Presupuesto() 
    {
        this.Detalle = new List<PresupuestosDetalle>();
    }
    
    public Presupuesto(int idPresupuesto, string nombreDest, DateTime fecha, List<PresupuestosDetalle> detalle)
    {
        this.IdPresupuesto = idPresupuesto;
        this.NombreDestinatario = nombreDest;
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