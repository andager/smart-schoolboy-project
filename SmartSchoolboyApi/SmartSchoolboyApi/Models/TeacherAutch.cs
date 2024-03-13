using System.ComponentModel.DataAnnotations;

namespace SmartSchoolboyApi.Models
{
    public class TeacherAutch
    {
        [Key]
        public string NumberPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
