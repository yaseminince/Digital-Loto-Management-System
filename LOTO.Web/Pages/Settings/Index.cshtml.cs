using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOTO.Web.Pages.Settings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public User UserAccount { get; set; }
        [BindProperty]
        public string CurrentPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }

        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return;

            UserAccount = await _userManager.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == user.Id);
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            UserAccount = await _userManager.GetUserAsync(User);

            if (UserAccount == null)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword","Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ModelState.AddModelError("NewPassword","New password is required.");
            }

            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword","Passwords do not match.");
            }

            if (!ModelState.IsValid)
                return Page();

            var result = await _userManager.ChangePasswordAsync(UserAccount,CurrentPassword,NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("NewPassword",error.Description);
                }

                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Password changed successfully.";
            ModelState.Clear();
            UserAccount = await _userManager.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == UserAccount.Id);
            return Page();
        }
    }
}