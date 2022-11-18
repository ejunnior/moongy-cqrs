namespace Logic.Students
{
    public enum Grade
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        F = 5
    }

    public class Enrollment : Entity
    {
        public Enrollment(Student student, Course course, Grade grade)
            : this()
        {
            Student = student;
            Course = course;
            Grade = grade;
        }

        private Enrollment()
        {
        }

        public Course Course { get; private set; }
        public Grade Grade { get; private set; }
        public Student Student { get; private set; }

        public void Update(Course course, Grade grade)
        {
            Course = course;
            Grade = grade;
        }
    }
}