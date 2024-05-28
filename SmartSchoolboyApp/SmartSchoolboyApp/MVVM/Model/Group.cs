using System.Collections.Generic;

namespace SmartSchoolboyApp.Classes
{
    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public int courseId { get; set; }
        public bool isActive { get; set; }
        public Course course { get; set; }
        public List<Student> students { get; set; } 
    }
}
