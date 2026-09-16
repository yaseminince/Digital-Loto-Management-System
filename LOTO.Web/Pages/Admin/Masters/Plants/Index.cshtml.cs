using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Admin.Masters.Plants
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IPlantService _plantService;

        public List<Plant> Plants { get; set; } = new();

        [BindProperty]
        public Plant Plant { get; set; } = new();
        public bool IsEditMode { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public IndexModel(IPlantService plantService)
        {
            _plantService = plantService;
        }

        public void OnGet()
        {
            LoadPlants();
        }

        public IActionResult OnPostAdd()
        {
            if (string.IsNullOrWhiteSpace(Plant.Name))
            {
                ModelState.AddModelError("Plant.Name","Plant name is required.");
                LoadPlants();
                return Page();
            }

            Plant.IsActive = true;
            Plant.CreatedBy = 1;
            Plant.CreatedDate = DateTime.Now;

            var request = new PlantRequest
            {
                Entity = Plant
            };

            var response = _plantService.Add(request);

            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty,response.Error.Message);
                LoadPlants();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Plant added successfully.";
            Plant = new LOTO.Domain.Entities.Plant();
            LoadPlants();
            return Page();
        }

        public IActionResult OnPostEdit(int plantId)
        {
            var response = _plantService.GetById(plantId);
            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            Plant = response.Entity;
            IsEditMode = true;
            LoadPlants();
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (string.IsNullOrWhiteSpace(Plant.Name))
            {
                IsEditMode = true;
                ModelState.AddModelError( "Plant.Name","Plant name is required.");
                LoadPlants();
                return Page();
            }

            var response = _plantService.GetById(Plant.Id);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.Name = Plant.Name;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new PlantRequest
            {
                Entity = response.Entity
            };

            var updateResponse = _plantService.Update(request);

            if (updateResponse.Error.HasException)
            {
                IsEditMode = true;
                ModelState.AddModelError( string.Empty,updateResponse.Error.Message);
                LoadPlants();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Plant updated successfully.";
            IsEditMode = false;
            Plant = new LOTO.Domain.Entities.Plant();
            LoadPlants();
            return Page();
        }

        public IActionResult OnPostChangeStatus(int plantId)
        {
            var response = _plantService.GetById(plantId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.IsActive = !response.Entity.IsActive;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new PlantRequest
            {
                Entity = response.Entity
            };

            _plantService.Update(request);

            ShowSuccessMessage = true;
            SuccessMessage = "Plant deactivated successfully.";
            LoadPlants();
            return Page();
        }

        private void LoadPlants()
        {
            var response = _plantService.GetList();

            if (response.Error.HasException)
            {
                Plants = new List<Plant>();
                return;
            }

            Plants = response.EntityList.OrderBy(x => x.Name).ToList();
        }
    }
}