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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoProductos.ListarProductos());
    }


    [HttpGet]
    public IActionResult AltaProducto()
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
    public IActionResult CrearProducto(AltaProductoViewModel productoVM)
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
        var producto = new Producto(productoVM);
        repoProductos.CrearProducto(producto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarProducto(int IdProducto)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var producto = repoProductos.ObtenerProductoPorId(IdProducto);
        ModificarProductoViewModel productoVM = new ModificarProductoViewModel(producto);
        return View(productoVM);
    }

    [HttpPost]
    public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
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
        var producto = new Producto(productoVM);
        repoProductos.ModificarProducto(producto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult EliminarProducto(int IdProducto)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View(repoProductos.ObtenerProductoPorId(IdProducto));
    }

    [HttpPost]
    public IActionResult EliminarProductoId(int IdProducto)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        repoProductos.EliminarProductoPorId(IdProducto);
        return RedirectToAction ("Index");
    }

}