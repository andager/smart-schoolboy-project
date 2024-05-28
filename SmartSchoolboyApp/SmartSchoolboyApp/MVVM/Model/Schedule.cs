using System;

namespace SmartSchoolboyApp.Classes
{
    public class Schedule
    {
        public int id { get; set; }
        public int groupId { get; set; }
        public DateTime date { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public Group group { get; set; }
    }
}
