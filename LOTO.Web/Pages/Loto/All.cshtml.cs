using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Loto
{
    [Authorize(Roles = "Admin")]
    public class AllModel : PageModel
    {
        private readonly ILotoService _lotoService;
        private readonly IUserService _userService;

        public AllModel(
            ILotoService lotoService,
            IUserService userService)
        {
            _lotoService = lotoService;
            _userService = userService;
        }

        public List<User> Users { get; set; } = new();

        public void OnGet()
        {
            Users = _userService.GetList().EntityList.ToList();
        }

        public async Task<IActionResult> OnGetDataAsync(int draw,int start,int length,[FromQuery] List<LotoStatus> statuses,
            [FromQuery] List<int> userIds,[FromQuery] List<LotoType> lotoTypes,DateTime? startDateFrom,DateTime? startDateTo,
            string searchValue,string sortColumn,string sortDirection)
        {
            int page = (start / length) + 1;

            var response = await _lotoService.GetFilteredAsync(
                statuses ?? new List<LotoStatus>(),
                userIds ?? new List<int>(),
                lotoTypes ?? new List<LotoType>(),
                startDateFrom,
                startDateTo,
                searchValue,
                sortColumn,
                sortDirection,
                page,
                length);

            var data = response.Items.Select(x => new
            {
                lotoNumber = x.LotoNumber,
                lotoType = x.LotoType.ToString(),
                user = x.User?.Name,
                plant = x.Plant?.Name,
                department = x.Department?.Name,
                location = x.Location?.Name,
                equipment = x.Equipment?.Name,
                description = x.Description,
                startDate = x.StartDate.ToString("dd.MM.yyyy HH:mm"),
                expectedEndDate = x.ExpectedEndDate.ToString("dd.MM.yyyy HH:mm"),
                endDate = x.ClosedDate.HasValue
                    ? x.ClosedDate.Value.ToString("dd.MM.yyyy HH:mm")
                    : "-",
                status = x.Status.ToString()
            });

            return new JsonResult(new
            {
                draw = draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = data
            });
        }
    }
}