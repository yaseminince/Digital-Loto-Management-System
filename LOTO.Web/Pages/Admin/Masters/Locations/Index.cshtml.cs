using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Admin.Masters.Locations
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILocationService _locationService;
        public List<Location> Locations { get; set; } = new();

        [BindProperty]
        public Location Location { get; set; } = new();
        public bool IsEditMode { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public IndexModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public void OnGet()
        {
            LoadLocations();
        }

        public IActionResult OnPostAdd()
        {
            if (string.IsNullOrWhiteSpace(Location.Name))
            {
                ModelState.AddModelError("Location.Name","Location name is required.");
                LoadLocations();
                return Page();
            }

            Location.IsActive = true;
            Location.CreatedBy = 1;
            Location.CreatedDate = DateTime.Now;

            var request = new LocationRequest
            {
                Entity = Location
            };

            var response = _locationService.Add(request);

            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty,response.Error.Message);
                LoadLocations();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Location added successfully.";
            Location = new LOTO.Domain.Entities.Location();
            LoadLocations();
            return Page();
        }

        public IActionResult OnPostEdit(int locationId)
        {
            var response = _locationService.GetById(locationId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            Location = response.Entity;
            IsEditMode = true;
            LoadLocations();
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (string.IsNullOrWhiteSpace(Location.Name))
            {
                IsEditMode = true;
                ModelState.AddModelError("Location.Name","Location name is required.");
                LoadLocations();
                return Page();
            }

            var response = _locationService.GetById(Location.Id);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.Name = Location.Name;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new LocationRequest
            {
                Entity = response.Entity
            };

            var updateResponse = _locationService.Update(request);

            if (updateResponse.Error.HasException)
            {
                IsEditMode = true;
                ModelState.AddModelError(string.Empty,updateResponse.Error.Message);
                LoadLocations();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Location updated successfully.";
            IsEditMode = false;
            Location = new LOTO.Domain.Entities.Location();
            LoadLocations();
            return Page();
        }

        public IActionResult OnPostChangeStatus(int locationId)
        {
            var response = _locationService.GetById(locationId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.IsActive = !response.Entity.IsActive;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new LocationRequest
            {
                Entity = response.Entity
            };

            _locationService.Update(request);
            ShowSuccessMessage = true;
            SuccessMessage = "Location deactivated successfully.";
            LoadLocations();
            return Page();
        }

        private void LoadLocations()
        {
            var response = _locationService.GetList();

            if (response.Error.HasException)
            {
                Locations = new List<Location>();
                return;
            }

            Locations = response.EntityList.OrderBy(x => x.Name).ToList();
        }
    }
}