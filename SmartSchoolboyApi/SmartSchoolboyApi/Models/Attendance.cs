namespace SmartSchoolboyApi.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ScheduleId { get; set; }
        public bool MarkOfPresence { get; set; }

        public virtual Schedule Schedule { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
