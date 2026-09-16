using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.UserDashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEquipmentService _equipmentService;
        private readonly ILotoService _lotoService;
        private readonly UserManager<User> _userManager;

        public IndexModel(IDepartmentService departmentService, IEquipmentService equipmentService, ILotoService lotoService
            , UserManager<User> userManager)
        {
            _departmentService = departmentService;
            _equipmentService = equipmentService;
            _lotoService = lotoService;
            _userManager = userManager;
        }

        public int ActiveLoto { get; set; }
        public int ClosedLoto { get; set; }
        public int OverdueLoto { get; set; }

        public int TodaysLoto { get; set; }
        public int TodaysClosedLoto { get; set; }

        public List<DepartmentStatistic> DepartmentStatistics { get; set; } = new();
        public List<EquipmentStatistic> EquipmentStatistics { get; set; } = new();
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return;

            var lotoResponse = await _lotoService.GetListAsync();

            var lotos = lotoResponse.EntityList;

            var myLotos = lotos.Where(x => x.UserId == user.Id).ToList();
            var departments = _departmentService.GetList().EntityList ?? new List<Department>();
            var equipments = _equipmentService.GetList().EntityList ?? new List<Equipment>();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var activeLotos = myLotos.Where(x => x.Status == LotoStatus.Active && x.ExpectedEndDate >= DateTime.Now).ToList();
            var overdueLotos = myLotos.Where(x => x.Status == LotoStatus.Overdue || (x.Status == LotoStatus.Active &&
                     x.ExpectedEndDate < DateTime.Now)).ToList();
            var closedLotos = myLotos.Where(x => x.Status == LotoStatus.Closed).ToList();

            ActiveLoto = activeLotos.Count;
            ClosedLoto = closedLotos.Count;
            OverdueLoto = overdueLotos.Count;
            TodaysLoto = myLotos.Count(x => x.StartDate >= today && x.StartDate < tomorrow);
            TodaysClosedLoto = myLotos.Count(x => x.ClosedDate.HasValue && x.ClosedDate.Value >= today && x.ClosedDate.Value < tomorrow);

            DepartmentStatistics = myLotos.GroupBy(x => x.DepartmentId).Select(group => new DepartmentStatistic{
                     DepartmentName =departments.FirstOrDefault(x => x.Id == group.Key)?.Name ?? "Unknown",
                     ActiveCount = group.Count(x =>x.Status == LotoStatus.Active),
                     ClosedCount = group.Count(x =>x.Status == LotoStatus.Closed) })
            .OrderByDescending(x =>x.ActiveCount + x.ClosedCount).ToList();

            EquipmentStatistics = myLotos.Where(x => x.Status == LotoStatus.Active).GroupBy(x => x.EquipmentId) .Select(group => new EquipmentStatistic
                {
                    EquipmentName = equipments.FirstOrDefault(x => x.Id == group.Key)?.Name?? "Unknown",
                    Count = group.Count()
                }).OrderByDescending(x => x.Count).Take(5).ToList();
        }
        public class DepartmentStatistic
        {
            public string DepartmentName { get; set; }
            public int ActiveCount { get; set; }
            public int ClosedCount { get; set; }
        }
        public class EquipmentStatistic
        {
            public string EquipmentName { get; set; }
            public int Count { get; set; }
        }
    }
}





