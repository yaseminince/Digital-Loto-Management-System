using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILotoService _lotoService;
        private readonly IDepartmentService _departmentService;
        private readonly IEquipmentService _equipmentService;

        public IndexModel(ILotoService lotoService,IDepartmentService departmentService,IEquipmentService equipmentService)
        {
            _lotoService = lotoService;
            _departmentService = departmentService;
            _equipmentService = equipmentService;
        }


        // dashboardun en üstündeki lotolar için
        public int ActiveLoto { get; set; }
        public int ClosedLoto { get; set; }
        public int OverdueLoto { get; set; }
        public int TodaysLoto { get; set; }
        public int TodaysClosedLoto { get; set; }

        // departmanların doluluk oranlarını gösteren kutucuk
        public List<DepartmentStatistic> DepartmentStatistics { get; set; } = new();
        // ekipmanların kullanım oranını gösteren kutucuk
        public List<EquipmentStatistic> EquipmentStatistics { get; set; } = new();

        public async Task OnGetAsync()
        {
            var lotoResponse = await _lotoService.GetListAsync();

            var lotos = lotoResponse.EntityList;

            var departments =_departmentService .GetList().EntityList  ?? new List<Department>();
            var equipments = _equipmentService.GetList().EntityList ?? new List<Equipment>();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);


            var activeLotos =lotos.Where(x =>x.Status == LotoStatus.Active && x.ExpectedEndDate >= DateTime.Now).ToList();
            var overdueLotos = lotos.Where(x => x.Status == LotoStatus.Overdue || (x.Status == LotoStatus.Active &&
                x.ExpectedEndDate < DateTime.Now)).ToList();
            var closedLotos =lotos.Where(x =>x.Status == LotoStatus.Closed).ToList();

            ActiveLoto =activeLotos.Count;
            ClosedLoto =closedLotos.Count;
            OverdueLoto =overdueLotos.Count;
            TodaysLoto =lotos.Count(x => x.StartDate >= today &&x.StartDate < tomorrow);
            TodaysClosedLoto =lotos.Count(x =>x.ClosedDate.HasValue &&x.ClosedDate.Value >= today &&x.ClosedDate.Value < tomorrow);


            DepartmentStatistics = lotos.GroupBy(x => x.DepartmentId).Select(group => new DepartmentStatistic{
                  DepartmentName =departments.FirstOrDefault(x => x.Id == group.Key)?.Name?? "Unknown",
                  ActiveCount = group.Count(x =>x.Status == LotoStatus.Active),
                  ClosedCount = group.Count(x =>x.Status == LotoStatus.Closed)}).OrderByDescending(x =>x.ActiveCount + x.ClosedCount)
                  .ToList();

            EquipmentStatistics = lotos.Where(x => x.Status == LotoStatus.Active).GroupBy(x => x.EquipmentId).Select(group => new EquipmentStatistic{
                   EquipmentName =equipments.FirstOrDefault(x => x.Id == group.Key)?.Name?? "Unknown",
                   Count = group.Count()}).OrderByDescending(x => x.Count).Take(5).ToList();}

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