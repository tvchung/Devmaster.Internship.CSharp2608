using Ex04.StudentManagement.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ex04.StudentManagement.Models
{
    public class Student
    {
        public string studentId { get; set; } = string.Empty;
        public string fullName { get; set; }=string.Empty;
        public DateTime dateOfBith { get; set; }

        public Gender gender { get; set; }
        public string email { get; set; } = string.Empty;
        public string phoneNumber {  get; set; }  = string.Empty;
        public string major { get; set; } = string.Empty;
        public double gpa { get; set; }
        public StudentStatus status { get; set; }   
    }
}
