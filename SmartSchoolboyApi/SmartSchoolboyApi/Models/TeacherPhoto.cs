using System.Text.Json.Serialization;

namespace SmartSchoolboyApi.Models
{
    public partial class TeacherPhoto
    {
        public TeacherPhoto()
        {
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public byte[] Photo { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
