using System;
using System.Collections.Generic;

namespace SmartSchoolboyApi.Models
{
    public partial class AchievementStudent
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public byte[] Photo { get; set; } = null!;

        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
