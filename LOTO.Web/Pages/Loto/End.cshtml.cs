using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Loto
{
    [Authorize]
    public class EndModel : PageModel
    {
        private readonly ILotoService _lotoService;
        private readonly UserManager<User> _userManager;
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public EndModel(ILotoService lotoService,UserManager<User> userManager)
        {
            _lotoService = lotoService;
            _userManager = userManager;
        }

        public List<LOTO.Domain.Entities.Loto> Lotos { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return;

            var response = await _lotoService.GetListAsync();

            if (response.Error.HasException)
                return;

            Lotos = response.EntityList.Where(x =>x.UserId == user.Id &&(x.Status == LotoStatus.Active ||
            x.Status == LotoStatus.Overdue)).OrderByDescending(x => x.StartDate).ToList();
        }

        public async Task<IActionResult> OnPostAsync(int lotoId)
        {
            var response = await _lotoService.CloseAsync(lotoId);

            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty,response.Error.Message);
                await OnGetAsync();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Loto ended successfully.";
            await OnGetAsync();
            return Page();
            
        }
    }
}