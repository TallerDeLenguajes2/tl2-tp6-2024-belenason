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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoClientes.ListarClientes());
    }


    [HttpGet]
    public IActionResult AltaCliente()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var cliente = repoClientes.ObtenerClientePorId(ClienteId);
        var clienteVM = new ModificarClienteViewModel(cliente);
        return View(clienteVM);
    }

    [HttpPost]
    public IActionResult ModificarCliente(ModificarClienteViewModel clienteVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        repoClientes.ModificarCliente(cliente);
        return RedirectToAction ("Index"); 
    }

    [HttpGet]
    public IActionResult EliminarCliente(int ClienteId)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View(repoClientes.ObtenerClientePorId(ClienteId));
    }

    [HttpPost]
    public IActionResult EliminarClienteId(int ClienteId)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        repoClientes.EliminarClientePorId(ClienteId);
        return RedirectToAction ("Index");
    }

}