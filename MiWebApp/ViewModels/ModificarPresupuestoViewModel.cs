using System.ComponentModel.DataAnnotations; 
public class ModificarPresupuestoViewModel
{
    int idPresupuesto;
    int clienteId;

    DateTime fechaCreacion;

    public ModificarPresupuestoViewModel()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }

    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public int ClienteId { get => clienteId; set => clienteId = value; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}