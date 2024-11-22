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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoPresupuestos.ObtenerPresupuestos());
    }


    [HttpGet]
    public IActionResult AltaPresupuesto()
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

    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupVM)
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

    [HttpGet]
    public IActionResult ModificarPresupuesto(int IdPresupuesto)
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

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presuVM)
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

    [HttpGet]
    public IActionResult EliminarPresupuesto(int IdPresupuesto)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
    }

    [HttpPost]
    public IActionResult EliminarPresupuestoId(int IdPresupuesto)
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

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int IdPresupuesto)
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

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(AgregarProduAPresuViewModel infoProducto)
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

    [HttpGet]
    public IActionResult EliminarProductoDePresupuesto(int IdPresupuesto)
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

    [HttpPost]
    public IActionResult EliminarProductoEnPresupuesto1(int IdPresupuesto, int IdProducto) //Preguntar al profe x q me decia que era ambiguo si solo tengo uno con ese nombre
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

    [HttpGet]
    public IActionResult DetallesPresupuesto(int IdPresupuesto)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
    }
}