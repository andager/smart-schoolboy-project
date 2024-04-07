using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class ControlThemePlane
    {
        public ControlThemePlane()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string LessonName { get; set; } = null!;
        public string LessonDescription { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
