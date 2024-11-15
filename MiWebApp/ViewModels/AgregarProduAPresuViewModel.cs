using System.ComponentModel.DataAnnotations; 
public class AgregarProduAPresuViewModel
{
    int idPresupuesto;
    int idProducto;

    int cantidad;

    public AgregarProduAPresuViewModel()
    {
    }


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }


    [Required(ErrorMessage = "La selección de un producto es obligatoria")]
    public int IdProducto { get => idProducto; set => idProducto = value; }

    [Required(ErrorMessage = "La cantidad del producto es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "El número debe ser un entero positivo.")]
    public int Cantidad { get => cantidad; set => cantidad = value; }
}