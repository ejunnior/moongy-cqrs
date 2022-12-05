using System;
using System.Collections.Generic;
using System.Linq;
using Api.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    using FluentNHibernate.Utils;

    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly CourseRepository _courseRepository;
        private readonly IDispatcher _dispatcher;
        private readonly StudentRepository _studentRepository;
        private readonly UnitOfWork _unitOfWork;

        public StudentController(
            UnitOfWork unitOfWork,
            IDispatcher dispatcher)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = new StudentRepository(unitOfWork);
            _courseRepository = new CourseRepository(unitOfWork);
            _dispatcher = dispatcher;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            _studentRepository.Delete(student);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisEnrollmentDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            if (string.IsNullOrWhiteSpace(dto.Comment))
                return Error($"DisEnrollment Comment is Required");

            var enrollment = student.GetEnrollment(enrollmentNumber);
            if (enrollment == null)
                return Error($"No enrollment found with the id {enrollmentNumber}");

            student.RemoveEnrollment(enrollment, dto.Comment);

            _studentRepository.Save(student);

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] EditPersonalInfoDto dto)
        {
            _dispatcher
                .Dispatch(new EditPersonalInfoCommand
                {
                    Id = id,
                    Email = dto.Email,
                    Name = dto.Name
                });

            return Ok();
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, StudentEnrollmentDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            Course course = _courseRepository.GetByName(dto.Course);
            if (course == null)
                return Error($"Course is incorrect {dto.Course}");

            bool sucess = Enum.TryParse(dto.Grade, out Grade grade);
            if (!sucess)
                return Error($"Grade is incorrect {dto.Grade}");

            student.Enroll(course, grade);

            _studentRepository.Save(student);

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            IReadOnlyList<Student> students = _studentRepository.GetList(enrolled, number);
            List<StudentDto> dtos = students.Select(x => ConvertToDto(x)).ToList();
            _unitOfWork.Commit();
            return Ok(dtos);
        }

        [HttpPost]
        public IActionResult Register([FromBody] StudentDto dto)
        {
            var student = new Student(dto.Name, dto.Email);

            if (dto.Course1 != null && dto.Course1Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course1);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
            }

            if (dto.Course2 != null && dto.Course2Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course2);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
            }

            _studentRepository.Save(student);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            Course course = _courseRepository.GetByName(dto.Course);
            if (course == null)
                return Error($"Course is incorrect {dto.Course}");

            bool sucess = Enum.TryParse(dto.Grade, out Grade grade);
            if (!sucess)
                return Error($"Grade is incorrect {dto.Grade}");

            var enrollment = student.GetEnrollment(enrollmentNumber);
            if (enrollment == null)
                return Error($"No enrollment found with the id {enrollmentNumber}");

            enrollment.Update(course, grade);

            _studentRepository.Save(student);
            _unitOfWork.Commit();

            return Ok();
        }

        private StudentDto ConvertToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course1 = student.FirstEnrollment?.Course?.Name,
                Course1Grade = student.FirstEnrollment?.Grade.ToString(),
                Course1Credits = student.FirstEnrollment?.Course?.Credits,
                Course2 = student.SecondEnrollment?.Course?.Name,
                Course2Grade = student.SecondEnrollment?.Grade.ToString(),
                Course2Credits = student.SecondEnrollment?.Course?.Credits,
            };
        }
    }
}