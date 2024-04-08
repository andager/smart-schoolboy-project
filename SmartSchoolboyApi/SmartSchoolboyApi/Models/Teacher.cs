using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Courses = new HashSet<Course>();
            SchoolSubjects = new HashSet<SchoolSubject>();
        }

        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string NumberPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? GenderId { get; set; }
        public DateTime DateOfBirtch { get; set; }
        public int RoleId { get; set; }
        public string WorkExperience { get; set; } = null!;
        public int? TeacherPhotoId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Gender? Gender { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual TeacherPhoto? TeacherPhoto { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }
        [JsonIgnore]
        public virtual ICollection<SchoolSubject> SchoolSubjects { get; set; }
    }
}
