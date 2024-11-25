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
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(repoClientes.ListarClientes());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la lista de clientes.";
            return RedirectToAction("Index", "Home");
        }
    }


    [HttpGet]
    public IActionResult AltaCliente()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            return View();  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de clientes.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM)
    {
        try
        {
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo dar de alta el cliente.";
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult ModificarCliente(int ClienteId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario para modificar el cliente.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult ModificarCliente(ModificarClienteViewModel clienteVM)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo modificar el cliente.";
            return RedirectToAction("Index");            
        }
    }

    [HttpGet]
    public IActionResult EliminarCliente(int ClienteId)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            return View(repoClientes.ObtenerClientePorId(ClienteId));            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la información del cliente.";
            return RedirectToAction("Index");  
        }
    }

    [HttpPost]
    public IActionResult EliminarClienteId(int ClienteId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo eliminar el cliente.";
            return RedirectToAction("Index");  
        }
    }

}