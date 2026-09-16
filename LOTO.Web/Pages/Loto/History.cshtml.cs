using LOTO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Loto
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly ILotoService _lotoService;
        public List<LOTO.Domain.Entities.Loto> Lotos { get; set; } = new();
        public HistoryModel(ILotoService lotoService)
        {
            _lotoService= lotoService;
        }
        public async Task OnGetAsync()
        {
            var response = await _lotoService.GetListAsync();
            if (response.Error.HasException)
            {
                return;
            }
            Lotos = response.EntityList.Where(x=> x.Status == Domain.Enums.LotoStatus.Closed).OrderByDescending(x => x.ClosedDate).ToList();
        }
    }
}
