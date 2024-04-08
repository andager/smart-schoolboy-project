using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Student
    {
        public Student()
        {
            Attendances = new HashSet<Attendance>();
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime DateOfBirch { get; set; }
        public int GenderId { get; set; }
        public string? NumberPhone { get; set; }
        public int? TelegramId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Gender Gender { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Attendance> Attendances { get; set; }
        [JsonIgnore]
        public virtual ICollection<Group> Groups { get; set; }
    }
}
