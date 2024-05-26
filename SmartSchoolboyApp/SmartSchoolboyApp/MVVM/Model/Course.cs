using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.Classes
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public int teacherId { get; set; }
        public bool isActive { get; set; }
        public Teacher teacher { get; set; }
        public List<ControlThemePlane> controlThemePlane { get; set; }
    }
}
