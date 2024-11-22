public interface IClienteRepository{
    void CrearCliente(Cliente cliente);
    void ModificarCliente(Cliente cliente);

    List<Cliente> ListarClientes();

    Cliente ObtenerClientePorId(int idCliente);
    void EliminarClientePorId(int idCliente);
}