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
        return View(repoPresupuestos.ObtenerPresupuestos());
    }


    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
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
    public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    {
        repoPresupuestos.CrearPresupuesto(presupuesto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int IdPresupuesto)
    {
        var presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto);
        ClienteRepository repoClientes = new ClienteRepository();
        List<Cliente> Clientes = repoClientes.ListarClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
    {
    if (ModelState.IsValid)
    {
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }
        return View (presupuesto);
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int IdPresupuesto)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
    }

    [HttpPost]
    public IActionResult EliminarPresupuestoId(int IdPresupuesto)
    {
        repoPresupuestos.EliminarpresupuestoPorId(IdPresupuesto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int IdPresupuesto)
    {
        ProductoRepository repoProductos = new ProductoRepository();
        List<Producto> productos = repoProductos.ListarProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();

        return View(IdPresupuesto);
    }

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(int IdPresupuesto, int IdProducto, int cantidad)
    {
        repoPresupuestos.AgregarProductoCantidadPresupuesto(IdProducto, cantidad, IdPresupuesto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult EliminarProductoDePresupuesto(int IdPresupuesto)
    {
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
        repoPresupuestos.EliminarProducto(IdPresupuesto, IdProducto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult DetallesPresupuesto(int IdPresupuesto)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
    }
}