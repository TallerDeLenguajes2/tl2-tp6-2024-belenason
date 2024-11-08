using Microsoft.AspNetCore.Mvc;
 
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
        return View(repoPresupuestos.ListarPresupuestos());
    }


    /*[HttpGet]
    public IActionResult AltaPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        repoProductos.CrearProducto(producto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarProducto(int IdProducto)
    {
        var producto = repoProductos.ObtenerProductoPorId(IdProducto);
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        repoProductos.ModificarProducto(producto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult EliminarProducto(int IdProducto)
    {
        return View(repoProductos.ObtenerProductoPorId(IdProducto));
    }

    [HttpPost]
    public IActionResult EliminarProductoId(int IdProducto)
    {
        repoProductos.EliminarProductoPorId(IdProducto);
        return RedirectToAction ("Index");
    }*/

}