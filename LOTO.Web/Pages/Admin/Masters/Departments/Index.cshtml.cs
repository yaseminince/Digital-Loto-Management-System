using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOTO.Web.Pages.Admin.Masters.Departments
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IDepartmentService _departmentService;
        public List<Department> Departments { get; set; } = new();
        [BindProperty]
        public Department Department { get; set; } = new();
        public bool IsEditMode { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }
        public IndexModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public void OnGet()
        {
            LoadDepartments();
        }

        public IActionResult OnPostAdd()
        {
            if (string.IsNullOrWhiteSpace(Department.Name))
            {
                ModelState.AddModelError("Department.Name", "Department name is required.");
                LoadDepartments();
                return Page();
            }

            Department.IsActive = true;
            Department.CreatedBy = 1;
            Department.CreatedDate = DateTime.Now;

            var request = new DepartmentRequest
            {
                Entity = Department
            };
            var response = _departmentService.Add(request);
            if (response.Error.HasException)
            {
                ModelState.AddModelError(string.Empty, response.Error.Message);
                LoadDepartments();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Department added succesfully.";
            LoadDepartments();
            return Page();
        }

        public IActionResult OnPostEdit(int departmentId)
        {
            var response = _departmentService.GetById(departmentId);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            Department = response.Entity;
            IsEditMode = true;

            LoadDepartments();

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (string.IsNullOrWhiteSpace(Department.Name))
            {
                IsEditMode = true;
                ModelState.AddModelError("Department.Name","Department name is required.");
                LoadDepartments();
                return Page();
            }

            var response = _departmentService.GetById(Department.Id);

            if (response.Error.HasException || response.Entity == null)
                return RedirectToPage();

            response.Entity.Name = Department.Name;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new DepartmentRequest
            {
                Entity = response.Entity
            };

            var updateResponse = _departmentService.Update(request);

            if (updateResponse.Error.HasException)
            {
                IsEditMode = true;

                ModelState.AddModelError(string.Empty,updateResponse.Error.Message);

                LoadDepartments();
                return Page();
            }

            ShowSuccessMessage = true;
            SuccessMessage = "Department updated successfully.";
            IsEditMode = false;
            Department = new LOTO.Domain.Entities.Department();
            LoadDepartments();
            return Page();
        }

        public IActionResult OnPostChangeStatus(int departmentId)
        {
            var response = _departmentService.GetById(departmentId);
            if(response.Error.HasException || response.Entity == null)
            {
                return RedirectToPage();
            }

            response.Entity.IsActive = !response.Entity.IsActive;
            response.Entity.UpdatedDate = DateTime.Now;

            var request = new DepartmentRequest
            {
                Entity = response.Entity
            };

            _departmentService.Update(request);
            ShowSuccessMessage = true;
            SuccessMessage = "Department deactivated succesfully.";
            LoadDepartments();
            return Page();

        }

        private void LoadDepartments()
        {
            var response = _departmentService.GetList();
            if (response.Error.HasException)
            {
                Departments = new List<Department>();
                return;
            }
            Departments = response.EntityList.OrderBy(x => x.Name).ToList();
        }
    }
}
