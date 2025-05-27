using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMasterDetailsCrud.Models.ViewModels;
using RazorMasterDetailsCrud.Models;
using RazorMasterDetailsCrud.Repositories;

namespace RazorMasterDetailsCrud.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentRepository _studentService;
        private readonly IWebHostEnvironment _environment;

        public EditModel(IStudentRepository studentService, IWebHostEnvironment environment)
        {
            _studentService = studentService;
            _environment = environment;
        }

        [BindProperty]
        public StudentViewModel StudentViewModel { get; set; }

        [BindProperty]
        public IFormFile ProfileFile { get; set; }

        [BindProperty]
        public string OldImageUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentService.GetStudentAsync(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            StudentViewModel = new StudentViewModel
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                AdmissionDate = student.AdmissionDate,
                MobileNo = student.MobileNo,
                IsEnrolled = student.IsEnrolled,
                CourseId = student.CourseId,
                Courses = (await _studentService.GetCoursesAsync()).ToList(),
                Modules = student.Modules.ToList()
            };
            OldImageUrl = student.ImageUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (ProfileFile != null && ProfileFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileFile.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileFile.CopyToAsync(fileStream);
                }               
                if (!string.IsNullOrEmpty(OldImageUrl) && OldImageUrl != "noimage.png")
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, "images", OldImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                StudentViewModel.ImageUrl = fileName;
            }
            else
            {
                StudentViewModel.ImageUrl = OldImageUrl; // Keep the existing image URL
            }

            // Map the ViewModel back to the Student entity
            var studentToUpdate = new Student
            {
                StudentId = StudentViewModel.StudentId,
                StudentName = StudentViewModel.StudentName,
                AdmissionDate = StudentViewModel.AdmissionDate,
                MobileNo = StudentViewModel.MobileNo,
                IsEnrolled = StudentViewModel.IsEnrolled,
                CourseId = StudentViewModel.CourseId,
                ImageUrl = StudentViewModel.ImageUrl,
                Modules = StudentViewModel.Modules // Ensure Modules are correctly bound
            };
            //await _studentService.DeleteModuleByStudentAsync(studentToUpdate.StudentId);
            await _studentService.AddModuleByStudentIdAsync(studentToUpdate.StudentId,studentToUpdate.Modules);

            await _studentService.UpdateStudentAsync(studentToUpdate);

            return RedirectToPage("./Index");
        }
    }
}
