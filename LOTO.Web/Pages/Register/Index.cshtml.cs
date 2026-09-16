using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Register
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterRequest Register { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _authService.RegisterAsync(Register);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration failed. Please check your email and password.");

                return Page();
            }

            return RedirectToPage("/Login/Index");
        }
    }
}