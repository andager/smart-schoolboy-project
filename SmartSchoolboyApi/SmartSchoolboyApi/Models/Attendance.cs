using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ScheduleId { get; set; }
        public bool MarkOfPresence { get; set; }
        [JsonIgnore]
        public virtual Schedule Schedule { get; set; } = null!;
        [JsonIgnore]
        public virtual Student Student { get; set; } = null!;
    }
}
