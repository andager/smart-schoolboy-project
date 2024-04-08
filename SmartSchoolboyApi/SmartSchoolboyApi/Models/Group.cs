using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Group
    {
        public Group()
        {
            Schedules = new HashSet<Schedule>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CourseId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Course Course { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }
    }
}
