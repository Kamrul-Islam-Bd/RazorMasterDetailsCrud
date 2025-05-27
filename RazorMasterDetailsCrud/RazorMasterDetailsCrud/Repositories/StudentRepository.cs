

using Microsoft.EntityFrameworkCore;
using RazorMasterDetailsCrud.Models;

namespace RazorMasterDetailsCrud.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _db;

        public StudentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _db.Students.Include(s => s.Course).Include(s => s.Modules).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await _db.Courses.ToListAsync(); 
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync(); 
            return student;
        }

        public async Task<Student> DeleteStudentAsync(int id)
        {
            Student student = await _db.Students.FindAsync(id); 
            if (student != null)
            {
                _db.Students.Remove(student);
                await _db.SaveChangesAsync(); 
            }
            return student;
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _db.Students.Include(s => s.Course).Include(s => s.Modules).Where(s => s.StudentId == id).FirstOrDefaultAsync(); 
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _db.Entry(student).State = EntityState.Modified;
            await _db.SaveChangesAsync(); 
            return student;
        }

        public async Task DeleteModuleByStudentAsync(int id)
        {
            var modules = await _db.Modules.Where(m => m.StudentId == id).ToListAsync(); 
            if (modules != null)
            {
                _db.RemoveRange(modules);
                await _db.SaveChangesAsync(); 
            }
        }

        public async Task AddModuleByStudentIdAsync(int id, List<Module> modules)
        {
            var existingModules = await _db.Modules.Where(m => m.StudentId == id).ToListAsync(); 
            if (existingModules != null)
            {
                _db.RemoveRange(existingModules);
               
            }
            if (modules != null)
            {
                foreach (var m in modules)
                {
                    m.StudentId = id;
                    m.ModuleName = m.ModuleName;
                    m.Duration = m.Duration;
                    _db.Modules.Add(m);
                }
                await _db.SaveChangesAsync(); 
            }
        }
    }
}
