using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Notifications
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public List<Notification> Notifications { get; set; } = new();

        public IndexModel(UserManager<User> userManager,INotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return;

            var response = await _notificationService.GetUserNotificationsAsync(user.Id);

            if (response.Error.HasException)
                return;

            Notifications = response.EntityList.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public async Task<IActionResult> OnPostMarkAsReadAsync(int notificationId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            await _notificationService.MarkAsReadAsync(
                notificationId,
                user.Id);

            return RedirectToPage();
        }
    }
}