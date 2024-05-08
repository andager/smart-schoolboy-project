using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.Classes
{
    public class Teacher
    {
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string patronymic { get; set; }
        public string fullName { get => $"{lastName} {firstName} {patronymic}"; }
        public string numberPhone { get; set; }
        public string password { get; set; }
        public int genderId { get; set; }
        public DateTime dateOfBirtch { get; set; }
        public int roleId { get; set; }
        public string workExperience { get; set; }
        public int teacherPhotoId { get; set; }
        public bool isActive { get; set; }
        public Gender gender { get; set; }
        public Role role { get; set; }
        public Teacherphoto teacherPhoto { get; set; }
    }
}
