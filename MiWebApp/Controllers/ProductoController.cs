using Microsoft.AspNetCore.Mvc;
 
public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;

    private IProductoRepository repoProductos;

    public ProductoController(ILogger<ProductoController> logger, IProductoRepository productosRepository)
    {
        _logger = logger;
        repoProductos = productosRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(repoProductos.ListarProductos());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la lista de productos.";
            return RedirectToAction("Index", "Home");
        }
    }


    [HttpGet]
    public IActionResult AltaProducto()
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
            ViewBag.ErrorMessage = "No se pudo cargar el formulario.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM)
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
            var producto = new Producto(productoVM);
            repoProductos.CrearProducto(producto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo dar de alta el producto.";
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult ModificarProducto(int IdProducto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
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
            var producto = new Producto(productoVM);
            repoProductos.ModificarProducto(producto);
            return RedirectToAction ("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo modificar el producto.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarProducto(int IdProducto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            if (HttpContext.Session.GetString("AccessLevel") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            return View(repoProductos.ObtenerProductoPorId(IdProducto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargarla información del producto.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EliminarProductoId(int IdProducto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo eliminar el producto.";
            return RedirectToAction("Index");
        }
    }

}