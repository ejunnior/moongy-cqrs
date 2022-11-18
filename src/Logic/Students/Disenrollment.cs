using System;

namespace Logic.Students
{
    public class Disenrollment : Entity
    {
        public Disenrollment(Student student, Course course, string comment)
            : this()
        {
            Student = student;
            Course = course;
            Comment = comment;
            DateTime = DateTime.UtcNow;
        }

        private Disenrollment()
        {
        }

        public string Comment { get; private set; }
        public Course Course { get; private set; }
        public DateTime DateTime { get; private set; }
        public Student Student { get; private set; }
    }
}