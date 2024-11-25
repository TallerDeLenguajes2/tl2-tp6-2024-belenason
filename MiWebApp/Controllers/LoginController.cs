using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IUserRepository userRepository, ILogger<LoginController> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        try
        {
            var model = new LoginViewModel
            {
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true",
                Username = HttpContext.Session.GetString("User")
            };
            return View(model);  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la página.";
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return View("Index", model);
            }
            User usuario = _userRepository.GetUser(model.Username, model.Password);
            if (usuario != null)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("User", usuario.Usuario);
                HttpContext.Session.SetString("AccessLevel", usuario.Rol.ToString());
                _logger.LogInformation("El usuario: "+ usuario.Usuario+" ingresó correctamente");
                return RedirectToAction("Index", "Home");
            }
            _logger.LogWarning("Intento de acceso invalido - Usuario: "+ usuario.Usuario + "Clave ingresada: "+ usuario.Contrasena);
            model.ErrorMessage = "Credenciales inválidas.";
            model.IsAuthenticated = false;

            return View("Index", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "Ha ocurrido un error al intentar iniciar sesión.";
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Logout()
    {
        try
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la vista de login
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "Ha ocurrido un error al intentar cerrar sesión.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]

    public IActionResult CrearUsuario()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]

    public IActionResult AltaUsuario(CrearUsuarioViewModel usuarioVM)
    {
        try
        {
            if(!ModelState.IsValid) return RedirectToAction ("CrearUsuario");
            User usuario = new User(usuarioVM);
            _userRepository.AltaUsuario(usuario);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo dar de alta el usuario.";
            return RedirectToAction("Index", "Home");
        }
    }
}