using Microsoft.AspNetCore.Mvc;
 
public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;

    private ProductoRepository repoProductos;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        repoProductos = new ProductoRepository();
    }

    public IActionResult Index()
    {
        return View(repoProductos.ListarProductos());
    }


    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel producto)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }
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
    public IActionResult ModificarProducto(ModificarProductoViewModel producto) //Como hago?
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }
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
    }

}