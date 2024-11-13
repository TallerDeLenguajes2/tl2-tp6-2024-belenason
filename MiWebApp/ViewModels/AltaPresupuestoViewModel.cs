using System.ComponentModel.DataAnnotations; 
public class AltaPresupuestoViewModel
{

    int clienteId;

    DateTime fechaCreacion;

    public AltaPresupuestoViewModel()
    {
    }
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public int ClienteId { get => clienteId; set => clienteId = value; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}