using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(IAuthService authService, UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public LoginRequest Login { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _authService.LoginAsync(Login);

            if (!result)
            {
                ModelState.AddModelError(string.Empty,"Invalid email or password.");

                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Login.Email);

            if (user == null)
                return Page();

            var roles = await _userManager.GetRolesAsync(user); // rollerine göre dashboardu açıyor

            if (roles.Contains("Admin"))
            {
                return RedirectToPage("/Admin/Index");
            }

            return RedirectToPage("/UserDashboard/Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToPage("/Index");
        }
    }
}