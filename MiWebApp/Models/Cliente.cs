public class Cliente
{
    int clienteId;
    string nombre;
    string email;
    string telefono;

    public int ClienteId { get => clienteId; set => clienteId = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Email { get => email; set => email = value; }
    public string Telefono { get => telefono; set => telefono = value; }

    public Cliente(int id, string nombreCliente, string emailCliente, string telCliente)
    {
        this.ClienteId = id;
        this.Nombre = nombreCliente;
        this.Email = emailCliente;
        this.Telefono = telCliente;
    }

    public Cliente()
    {
        
    }

    public Cliente(AltaClienteViewModel clienteVM)
    {
        this.Nombre = clienteVM.Nombre;
        this.Email = clienteVM.Email;
        this.Telefono = clienteVM.Telefono;
    }

    public Cliente(ModificarClienteViewModel clienteVM)
    {
        this.ClienteId = clienteVM.ClienteId;
        this.Nombre = clienteVM.Nombre;
        this.Email = clienteVM.Email;
        this.Telefono = clienteVM.Telefono;
    }
}