using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual Group Group { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
