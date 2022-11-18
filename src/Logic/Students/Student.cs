using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Students
{
    public class Student : Entity
    {
        private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
        private readonly IList<Enrollment> _enrollments = new List<Enrollment>();

        public Student(string name, string email)
            : this()
        {
            Name = name;
            Email = email;
        }

        private Student()
        {
        }

        public IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();
        public string Email { get; set; }
        public IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
        public Enrollment FirstEnrollment => GetEnrollment(0);
        public string Name { get; set; }
        public Enrollment SecondEnrollment => GetEnrollment(1);

        public void AddDisenrollmentComment(Enrollment enrollment, string comment)
        {
            var disenrollment = new Disenrollment(enrollment.Student, enrollment.Course, comment);
            _disenrollments.Add(disenrollment);
        }

        public void Enroll(Course course, Grade grade)
        {
            if (_enrollments.Count >= 2)
                throw new Exception("Cannot have more than 2 enrollments");

            var enrollment = new Enrollment(this, course, grade);
            _enrollments.Add(enrollment);
        }

        public void RemoveEnrollment(Enrollment enrollment)
        {
            _enrollments.Remove(enrollment);
        }

        private Enrollment GetEnrollment(int index)
        {
            if (_enrollments.Count > index)
                return _enrollments[index];

            return null;
        }
    }
}