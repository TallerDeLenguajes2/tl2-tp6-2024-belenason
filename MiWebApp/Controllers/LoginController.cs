using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly IUserRepository _userRepository;
    public LoginController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model);
    }

    public IActionResult Login(LoginViewModel model)
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
            HttpContext.Session.SetString("User", usuario.Username);
            HttpContext.Session.SetString("AccessLevel,", usuario.AccessLevel.ToString());

            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales inválidas.";
        model.IsAuthenticated = false;

        return View("Index", model);

    }

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
    
}