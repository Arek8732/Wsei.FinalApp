using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

public class LoginModel : PageModel
{
    [BindProperty] public string UserName { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    public string Message { get; set; } = "";

    public async Task<IActionResult> OnPost()
    {
        if ((UserName == "admin" || UserName == "user") && Password == "wsei")
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, UserName == "admin" ? "Admin" : "User")
            };
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return Redirect("/Admin/Stats");
        }
        Message = "Błędne dane logowania (admin/user, hasło: wsei)";
        return Page();
    }
}
