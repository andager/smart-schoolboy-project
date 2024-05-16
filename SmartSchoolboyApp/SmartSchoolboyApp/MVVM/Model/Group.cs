using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
