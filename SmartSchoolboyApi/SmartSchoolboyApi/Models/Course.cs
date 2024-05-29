using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Course
    {
        public Course()
        {
            Groups = new HashSet<Group>();
            ControlThemePlanes = new HashSet<ControlThemePlane>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TeacherId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Teacher Teacher { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ControlThemePlane> ControlThemePlanes { get; set; }
    }
}