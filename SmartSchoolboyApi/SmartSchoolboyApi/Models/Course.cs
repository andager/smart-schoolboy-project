using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Course
    {
        public Course()
        {
            AchievementStudents = new HashSet<AchievementStudent>();
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TeacherId { get; set; }
        public int ControlThemePlaneId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ControlThemePlane ControlThemePlane { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<AchievementStudent> AchievementStudents { get; set; }
        [JsonIgnore]
        public virtual ICollection<Group> Groups { get; set; }
    }
}
