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
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
    {
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction ("Index");
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
    public IActionResult EliminarProductoEnPresupuesto(int IdPresupuesto, int IdProducto, int cantidad)
    {
        repoPresupuestos.EliminarProducto(IdProducto, IdPresupuesto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult DetallesPresupuesto(int IdPresupuesto)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(IdPresupuesto));
    }
}