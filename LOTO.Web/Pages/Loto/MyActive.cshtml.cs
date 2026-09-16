using LOTO.Application.Interfaces;
using LOTO.Application.Services;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Loto
{
    [Authorize]
    public class MyActiveModel : PageModel
    {
        private readonly ILotoService _lotoService;
        public List<LOTO.Domain.Entities.Loto> Lotos { get; set; }
        public MyActiveModel(ILotoService lotoService)
        {
            _lotoService = lotoService;
        }

        public async Task OnGetAsync()
        {
            var response = await _lotoService.GetListAsync();
            if (response.Error.HasException)
            {
                return;
            }
            Lotos = response.EntityList.Where(x => x.Status == LotoStatus.Active || x.Status == LotoStatus.Overdue).ToList();
        }
    }
}
