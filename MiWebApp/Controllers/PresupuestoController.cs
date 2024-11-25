using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;

    private PresupuestoRepository repoPresupuestos;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        repoPresupuestos = new PresupuestoRepository();
    }

    public IActionResult Index()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(repoPresupuestos.ObtenerPresupuestos());  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la lista de presupuestos.";
            return RedirectToAction("Index", "Home");
        }
    }


    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            ClienteRepository repoClientes = new ClienteRepository();
            List<Cliente> Clientes = repoClientes.ListarClientes();
            ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
            {
                Value = c.ClienteId.ToString(), 
                Text = c.Nombre
            }).ToList();
            return View(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de presupuesto.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupVM)
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
            var presu = new Presupuesto(presupVM);
            repoPresupuestos.CrearPresupuesto(presu);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo dar de alta el presupuesto.";
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            ClienteRepository repoClientes = new ClienteRepository();
            List<Cliente> Clientes = repoClientes.ListarClientes();
            ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
            {
                Value = c.ClienteId.ToString(), 
                Text = c.Nombre
            }).ToList();
            var presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto);
            var presupuestoVM = new ModificarPresupuestoViewModel();
            presupuestoVM.IdPresupuesto = IdPresupuesto;
            presupuestoVM.FechaCreacion = presupuesto.FechaCreacion;
            return View(presupuestoVM); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de modificación de presupuestos.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presuVM)
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
            var presupuesto = new Presupuesto(presuVM);
            repoPresupuestos.ModificarPresupuesto(presupuesto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo modificar el presupuesto.";
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la información del presupuesto.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EliminarPresupuestoId(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            repoPresupuestos.EliminarpresupuestoPorId(IdPresupuesto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo eliminar el presupuesto.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            ProductoRepository repoProductos = new ProductoRepository();
            List<Producto> productos = repoProductos.ListarProductos();
            ViewData["Productos"] = productos.Select(p => new SelectListItem
            {
                Value = p.IdProducto.ToString(), 
                Text = p.Descripcion 
            }).ToList();
            var model = new AgregarProduAPresuViewModel();
            model.IdPresupuesto = IdPresupuesto;
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(AgregarProduAPresuViewModel infoProducto)
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
            repoPresupuestos.AgregarProductoCantidadPresupuesto(infoProducto.IdProducto, infoProducto.Cantidad, infoProducto.IdPresupuesto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo agregar el producto al presupuesto.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarProductoDePresupuesto(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            List<Producto> productos = repoPresupuestos.ListarProductosAsociadosAPresupuesto(IdPresupuesto);
            ViewData["Productos"] = productos.Select(p => new SelectListItem
            {
                Value = p.IdProducto.ToString(), 
                Text = p.Descripcion 
            }).ToList();

            return View(IdPresupuesto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EliminarProductoEnPresupuesto1(int IdPresupuesto, int IdProducto) //Preguntar al profe x q me decia que era ambiguo si solo tengo uno con ese nombre
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            repoPresupuestos.EliminarProducto(IdPresupuesto, IdProducto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo eliminar el producto del presupuesto.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult DetallesPresupuesto(int IdPresupuesto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la información del presupuesto.";
            return RedirectToAction("Index");
        }
    }
}