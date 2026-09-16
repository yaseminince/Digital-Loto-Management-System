using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace LOTO.Web.Pages.Admin.Masters.Equipment
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;

        public List<LOTO.Domain.Entities.Equipment> Equipments { get; set; } = new();

        [BindProperty]
        public LOTO.Domain.Entities.Equipment Equipment { get; set; } = new();
        public bool IsEditMode { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public IndexModel(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public void OnGet()
        {
            LoadEquipments();
        }

        public IActionResult OnPostAdd()
        {
            if (string.IsNullOrWhiteSpace(Equipment.Name))
            {
                ModelState.AddModelError("Equipment.Name","Equipment name is required.");
                LoadEquipments();
                return Page();
            }

            Equipment.IsActive = true;
            Equipment.CreatedBy = 1;
            Equipment.CreatedDate = DateTime.Now;

            var request = new EquipmentRequest
            {
                Entity = Equipment
            };

            var response = _equipmentService.Add(request);

            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty,response.Error.Message);
                LoadEquipments();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Equipment added succesfully.";
            Equipment = new LOTO.Domain.Entities.Equipment();
            LoadEquipments();
            return Page();
        }

        public IActionResult OnPostEdit(int equipmentId)
        {
            var response = _equipmentService.GetById(equipmentId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            Equipment = response.Entity;
            IsEditMode = true;
            LoadEquipments();
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (string.IsNullOrWhiteSpace(Equipment.Name))
            {
                IsEditMode = true;
                ModelState.AddModelError("Equipment.Name","Equipment name is required.");
                LoadEquipments();
                return Page();
            }

            var response = _equipmentService.GetById(Equipment.Id);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.Name = Equipment.Name;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new EquipmentRequest
            {
                Entity = response.Entity
            };

            var updateResponse = _equipmentService.Update(request);
            if (updateResponse.Error.HasException)
            {
                IsEditMode = true;
                ModelState.AddModelError(string.Empty,updateResponse.Error.Message);
                LoadEquipments();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Equipment updated succesfully.";
            IsEditMode = false;
            Equipment = new LOTO.Domain.Entities.Equipment();
            LoadEquipments();
            return Page();
        }

        public IActionResult OnPostChangeStatus(int equipmentId)
        {
            var response = _equipmentService.GetById(equipmentId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.IsActive = !response.Entity.IsActive;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new EquipmentRequest
            {
                Entity = response.Entity
            };

            _equipmentService.Update(request);

            ShowSuccessMessage = true;
            SuccessMessage = "Equipment deactivated successfully.";
            LoadEquipments();
            return Page();

        }

        private void LoadEquipments()
        {
            var response = _equipmentService.GetList();

            if (response.Error.HasException)
            {
                Equipments = new List<LOTO.Domain.Entities.Equipment>();
                return;
            }

            Equipments = response.EntityList.OrderBy(x => x.Name).ToList();
        }
    }
}