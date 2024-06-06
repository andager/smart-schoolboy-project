using System;

namespace SmartSchoolboyApp.Classes
{
    public class Schedule
    {
        public int id { get; set; }
        public int groupId { get; set; }
        public DateTime date { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public Group group { get; set; }
    }
}
