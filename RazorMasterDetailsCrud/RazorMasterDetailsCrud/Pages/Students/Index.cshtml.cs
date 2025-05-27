using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMasterDetailsCrud.Models;
using RazorMasterDetailsCrud.Repositories;

namespace RazorMasterDetailsCrud.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IStudentRepository _studentService;
        public IEnumerable<Student> Students;
        public IndexModel(IStudentRepository studentService, IEnumerable<Student> _Students)
        {
            _studentService = studentService;
            Students=_Students;
        }      

        public async Task OnGetAsync()
        {
            Students = await _studentService.GetStudentsAsync(); 
        }
              
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToPage("./Index"); // Redirect back to the Index page after deletion
        }
    }
}
