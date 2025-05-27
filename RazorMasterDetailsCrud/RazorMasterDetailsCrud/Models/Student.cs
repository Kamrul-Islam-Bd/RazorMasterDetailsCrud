using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RazorMasterDetailsCrud.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; } = null!;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AdmissionDate { get; set; }

        public string MobileNo { get; set; } = null!;

        public bool IsEnrolled { get; set; }

        public string? ImageUrl { get; set; }

        public int CourseId { get; set; }

        public decimal RegistrationFee { get; set; }

        public virtual Course Course { get; set; } = null!;

        public virtual List<Module> Modules { get; set; } = new List<Module>();
    }

    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public virtual ICollection<Student>? Students { get; set; } = new List<Student>();
    }
    public class Module
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; } = null!;

        public int Duration { get; set; }

        public int StudentId { get; set; }

        public virtual Student? Student { get; set; } = null!;
    }
}
