using Microsoft.AspNetCore.Mvc;
 
public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;

    private ClienteRepository repoClientes;

    public ClienteController(ILogger<ClienteController> logger)
    {
        _logger = logger;
        repoClientes = new ClienteRepository();
    }

    public IActionResult Index()
    {
        return View(repoClientes.ListarClientes());
    }


    [HttpGet]
    public IActionResult AltaCliente()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }
        var cliente = new Cliente(clienteVM);
        repoClientes.CrearCliente(cliente);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarCliente(int ClienteId)
    {
        var cliente = repoClientes.ObtenerClientePorId(ClienteId);
        var clienteVM = new ModificarClienteViewModel(cliente);
        return View(clienteVM);
    }

    [HttpPost]
    public IActionResult ModificarCliente(ModificarClienteViewModel clienteVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        repoClientes.ModificarCliente(cliente);
        return RedirectToAction ("Index"); 
    }

    [HttpGet]
    public IActionResult EliminarCliente(int ClienteId)
    {
        return View(repoClientes.ObtenerClientePorId(ClienteId));
    }

    [HttpPost]
    public IActionResult EliminarClienteId(int ClienteId)
    {
        repoClientes.EliminarClientePorId(ClienteId);
        return RedirectToAction ("Index");
    }

}