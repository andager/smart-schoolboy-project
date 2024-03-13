using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Students = new HashSet<Student>();
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }
        [JsonIgnore]
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
