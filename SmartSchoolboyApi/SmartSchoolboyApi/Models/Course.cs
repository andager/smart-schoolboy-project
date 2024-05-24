using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
        [JsonIgnore]
        public virtual ICollection<ControlThemePlane> ControlThemePlanes { get; set; }
    }

    public partial class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

    }
}