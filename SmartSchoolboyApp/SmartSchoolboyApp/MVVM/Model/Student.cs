using System;

namespace SmartSchoolboyApp.Classes
{
    public class Student
    {
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string patronymic { get; set; }
        public DateTime dateOfBirch { get; set; }
        public int genderId { get; set; }
        public string numberPhone { get; set; }
        public object telegramId { get; set; }
        public bool isActive { get; set; }
        public Gender gender { get; set; }
    }
}
