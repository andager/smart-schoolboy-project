namespace SmartSchoolboyApp.Classes
{
    public class Attendance
    {
        public int id { get; set; }
        public int studentId { get; set; }
        public int scheduleId { get; set; }
        public bool markOfPresence { get; set; }
        public Schedule schedule { get; set; }
        public Student student { get; set; }
    }
}
