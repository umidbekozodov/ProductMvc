using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductMvc.Dtoes;
using ProductMvc.Entities;

namespace Car_Configuration.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult SignIn(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInDto signIn, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid) return View();

        User user;
        if (signIn.UsernameOrEmail!.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
        }
        else
        {
            user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);
        }

        if (user == null)
        {
            ModelState.AddModelError("", "Login or parol incorrect");
            return View(signIn);
        }

        var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, true, true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Login or parol incorrect");
            return View(signIn);
        }
        if (returnUrl != null) return Redirect(returnUrl);

        return Redirect("/");
    }


    public IActionResult Register(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto register, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid) return View();

        var userExists = await _userManager.FindByNameAsync(register.Username) != null;
        var emailExists = await _userManager.FindByEmailAsync(register.Email) != null;

        if (userExists)
        {
            ModelState.AddModelError("", "Username already exists.");
            return View(register);
        }

        if (emailExists)
        {
            ModelState.AddModelError("", "Email already exists.");
            return View(register);
        }

        User user = new User
        {
            Email = register.Email,
            UserName = register.Username
        };

        IdentityResult result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            return View();
        }

        return Redirect($"SignIn?returnUrl={returnUrl}");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(SignIn));
    }
}
