using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace LOTO.Web.Pages.Loto
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILotoService _lotoService;
        private readonly IPlantService _plantService;
        private readonly IDepartmentService _departmentService;
        private readonly ILocationService _locationService;
        private readonly IEquipmentService _equipmentService;

        public CreateModel(UserManager<User> userManager, ILotoService lotoService, IPlantService plantService,
            IDepartmentService departmentService, ILocationService locationService, IEquipmentService equipmentService)
        {
            _userManager = userManager;
            _lotoService = lotoService;
            _plantService = plantService;
            _departmentService = departmentService;
            _locationService = locationService;
            _equipmentService = equipmentService;
        }

        [BindProperty]
        public LOTO.Domain.Entities.Loto Loto { get; set; }
        public string CurrentUserName { get; set; }
        public List<Plant> Plants { get; set; } = new();
        public List<Department> Departments { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();

        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month,now.Day, now.Hour, now.Minute, 0);

            Loto = new LOTO.Domain.Entities.Loto
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddHours(1)
            };

            CurrentUserName = user.Name;
            LoadMasterData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            ModelState.Remove("Loto.User");
            ModelState.Remove("Loto.Plant");
            ModelState.Remove("Loto.Department");
            ModelState.Remove("Loto.Location");
            ModelState.Remove("Loto.Equipment");
            ModelState.Remove("Loto.LotoNumber");
            ModelState.Remove("Loto.Notifications");

            var now = DateTime.Now;
            var currentDateTime = new DateTime(now.Year,now.Month,now.Day, now.Hour,now.Minute,0);

            if (Loto.StartDate < currentDateTime)
            {
                ModelState.AddModelError("Loto.StartDate", "Start date cannot be in the past.");
            }

            if (Loto.ExpectedEndDate <= currentDateTime)
            {
                ModelState.AddModelError("Loto.ExpectedEndDate", "Expected end date must be later than the start date.");
            }

            if (!ModelState.IsValid)
            {
                CurrentUserName = user.Name;
                LoadMasterData();
                return Page();
            }

            var request = new LotoRequest
            {
                Entity = Loto
            };

            var response = await _lotoService.AddAsync(request);

            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty, response.Error.Message);
                CurrentUserName = user.Name;
                LoadMasterData();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = $"{Loto.LotoNumber} created successfully.";

            now = DateTime.Now;
            var startDate = new DateTime(now.Year,now.Month,now.Day,now.Hour,now.Minute,0);

            Loto = new LOTO.Domain.Entities.Loto
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddHours(1)
            };

            ModelState.Clear();

            CurrentUserName = user.Name;
            LoadMasterData();
            return Page();
        }

        private void LoadMasterData()
        {
            Plants = _plantService.GetList().EntityList.Where(x => x.IsActive).ToList();
            Departments = _departmentService.GetList().EntityList.Where(x => x.IsActive).ToList();
            Locations = _locationService.GetList().EntityList .Where(x => x.IsActive).ToList();
            Equipments = _equipmentService.GetList() .EntityList.Where(x => x.IsActive).ToList();
        }
    }
}