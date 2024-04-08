using Microsoft.EntityFrameworkCore;

namespace SmartSchoolboyApi.Models
{
    public partial class SmartSchoolboyBaseContext : DbContext
    {
        public SmartSchoolboyBaseContext()
        {
        }

        public SmartSchoolboyBaseContext(DbContextOptions<SmartSchoolboyBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; } = null!;
        public virtual DbSet<ControlThemePlane> ControlThemePlanes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<SchoolSubject> SchoolSubjects { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<TeacherPhoto> TeacherPhotos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_Attendance_Schedule");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Attendance_Student");
            });

            modelBuilder.Entity<ControlThemePlane>(entity =>
            {
                entity.ToTable("ControlThemePlane");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LessonDescription).HasMaxLength(500);

                entity.Property(e => e.LessonName).HasMaxLength(100);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Course_Teacher");

                entity.HasMany(d => d.ControlThemePlanes)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "CourseControlThemePlane",
                        l => l.HasOne<ControlThemePlane>().WithMany().HasForeignKey("ControlThemePlaneId").HasConstraintName("FK_CourseControlThemePlane_ControlThemePlane"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CourseControlThemePlane_Course"),
                        j =>
                        {
                            j.HasKey("CourseId", "ControlThemePlaneId");

                            j.ToTable("CourseControlThemePlane");

                            j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");

                            j.IndexerProperty<int>("ControlThemePlaneId").HasColumnName("ControlThemePlaneID");
                        });
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Group_Course");

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.Groups)
                    .UsingEntity<Dictionary<string, object>>(
                        "GroupStudent",
                        l => l.HasOne<Student>().WithMany().HasForeignKey("StudentId").HasConstraintName("FK_GroupStudent_Student"),
                        r => r.HasOne<Group>().WithMany().HasForeignKey("GroupId").HasConstraintName("FK_GroupStudent_Group"),
                        j =>
                        {
                            j.HasKey("GroupId", "StudentId");

                            j.ToTable("GroupStudent");

                            j.IndexerProperty<int>("GroupId").HasColumnName("GroupID");

                            j.IndexerProperty<int>("StudentId").HasColumnName("StudentID");
                        });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EndTime).HasColumnType("time(0)");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.StartTime).HasColumnType("time(0)");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Schedule_Group");
            });

            modelBuilder.Entity<SchoolSubject>(entity =>
            {
                entity.ToTable("SchoolSubject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirch).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(35);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName).HasMaxLength(35);

                entity.Property(e => e.NumberPhone).HasMaxLength(11);

                entity.Property(e => e.Patronymic).HasMaxLength(35);

                entity.Property(e => e.TelegramId).HasColumnName("TelegramID");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Student_Gender");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirtch).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(35);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName).HasMaxLength(35);

                entity.Property(e => e.NumberPhone).HasMaxLength(11);

                entity.Property(e => e.Password).HasMaxLength(25);

                entity.Property(e => e.Patronymic).HasMaxLength(35);

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasDefaultValueSql("((4))");

                entity.Property(e => e.TeacherPhotoId).HasColumnName("TeacherPhotoID");

                entity.Property(e => e.WorkExperience).HasMaxLength(2);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Teacher_Gender");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Teacher_Role");

                entity.HasOne(d => d.TeacherPhoto)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.TeacherPhotoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Teacher_TeacherPhoto");

                entity.HasMany(d => d.SchoolSubjects)
                    .WithMany(p => p.Teachers)
                    .UsingEntity<Dictionary<string, object>>(
                        "TeacherSchoolSubject",
                        l => l.HasOne<SchoolSubject>().WithMany().HasForeignKey("SchoolSubjectId").HasConstraintName("FK_TeacherSchoolSubject_SchoolSubject"),
                        r => r.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId").HasConstraintName("FK_TeacherSchoolSubject_Teacher"),
                        j =>
                        {
                            j.HasKey("TeacherId", "SchoolSubjectId");

                            j.ToTable("TeacherSchoolSubject");

                            j.IndexerProperty<int>("TeacherId").HasColumnName("TeacherID");

                            j.IndexerProperty<int>("SchoolSubjectId").HasColumnName("SchoolSubjectID");
                        });
            });

            modelBuilder.Entity<TeacherPhoto>(entity =>
            {
                entity.ToTable("TeacherPhoto");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
