using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMasterDetailsCrud.Models;
using RazorMasterDetailsCrud.Models.ViewModels;
using RazorMasterDetailsCrud.Repositories;

namespace RazorMasterDetailsCrud.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly IStudentRepository _studentService;
        private readonly IWebHostEnvironment _environment;
        public IEnumerable<Student> Students;

        public CreateModel(IStudentRepository studentService, IWebHostEnvironment environment)
        {
            _studentService = studentService;
            _environment = environment;
        }

        [BindProperty]
        public StudentViewModel StudentViewModel { get; set; }

        [BindProperty]
        public IFormFile ProfileFile { get; set; }

        public IActionResult OnGet()
        {

            StudentViewModel = new StudentViewModel
            {
                Courses = _studentService.GetCoursesAsync().Result.ToList()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StudentViewModel.Courses = _studentService.GetCoursesAsync().Result.ToList();

                return Page();
            }

            if (ProfileFile != null && ProfileFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileFile.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileFile.CopyToAsync(fileStream);
                }

                StudentViewModel.ImageUrl = fileName;
            }
            else
            {
                StudentViewModel.ImageUrl = "noimage.png"; 
            }
            var student = new Student
            {
                StudentName = StudentViewModel.StudentName,
                MobileNo = StudentViewModel.MobileNo,
                IsEnrolled = StudentViewModel.IsEnrolled,
                AdmissionDate = StudentViewModel.AdmissionDate,
                RegistrationFee = StudentViewModel.RegistrationFee,
                ImageUrl = StudentViewModel.ImageUrl,
                CourseId = StudentViewModel.CourseId,
                Modules = StudentViewModel.Modules,
               
            };
            _studentService.AddStudentAsync(student);
            return RedirectToPage("./Index");
        }
    }
}
