using RazorMasterDetailsCrud.Models;

namespace RazorMasterDetailsCrud.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync(); 
        Task<Student> GetStudentAsync(int id);        
        Task<Student> UpdateStudentAsync(Student student); 
        Task<Student> AddStudentAsync(Student student);    
        Task<Student> DeleteStudentAsync(int id);       
        Task<IEnumerable<Course>> GetCoursesAsync();    
        Task DeleteModuleByStudentAsync(int id);     
        Task AddModuleByStudentIdAsync(int id, List<Module> modules); 
    }
}
