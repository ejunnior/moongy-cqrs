﻿namespace Api.Dtos
{
    public sealed class StudentDisEnrollmentDto
    {
        public string Comment { get; set; }
    }

    public sealed class StudentDto
    {
        public string Course1 { get; set; }
        public int? Course1Credits { get; set; }
        public string Course1DisenrollmentComment { get; set; }
        public string Course1Grade { get; set; }
        public string Course2 { get; set; }
        public int? Course2Credits { get; set; }
        public string Course2DisenrollmentComment { get; set; }
        public string Course2Grade { get; set; }
        public string Email { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class StudentEnrollmentDto
    {
        public string Course { get; set; }
        public string Grade { get; set; }
    }

    public sealed class StudentTransferDto
    {
        public string Course { get; set; }
        public string Grade { get; set; }
    }
}