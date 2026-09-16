using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LOTO.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public UsersModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public List<User> Users { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public bool ShowErrorMessage { get; set; }
        public string ErrorMessage { get; set; }
        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.Include(x => x.Role).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostActivateAsync(int userId)
        {

            var currentUserId =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                ShowErrorMessage = true;
                ErrorMessage = "User does not exist.";
                await OnGetAsync();
                return Page();
            }

            user.IsActive = true;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = currentUserId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ShowErrorMessage = true;
                ErrorMessage = "User could not be activated.";
                await OnGetAsync();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = $"{user.Name} has been activated.";
            await OnGetAsync();
            return Page();

        }

        public async Task<IActionResult> OnPostDeactivateAsync(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // admin
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if ( user== null)
            {
                ShowErrorMessage = true;
                ErrorMessage = "User not found.";
                await OnGetAsync();
                return Page();
            }

            user.IsActive = false;
            user.UpdatedBy= currentUserId;
            user.UpdatedDate= DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ShowErrorMessage= true;
                ErrorMessage = "User could not be activated.";
                await OnGetAsync();
                return Page();
            }
            ShowSuccessMessage = true;
            SuccessMessage = $"{user.Name} has been deactivated successfully.";
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(currentUserId == userId)
            {
                ShowErrorMessage = true;
                ErrorMessage = "You cannot delete your own account.";
                await OnGetAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                ShowErrorMessage = true;
                ErrorMessage = "User not found.";
                await OnGetAsync();
                return Page();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ShowErrorMessage = true;
                ErrorMessage ="User could not be deleted.";
                await OnGetAsync();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage =$"{user.Name} has been deleted successfully.";
            await OnGetAsync();
            return Page();

        }
    }
}