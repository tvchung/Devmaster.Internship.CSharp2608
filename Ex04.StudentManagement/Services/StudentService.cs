using Ex04.StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ex04.StudentManagement.Services
{
    /// <summary>
    /// Name: StudentService
    /// Author: Chung  Trinhj
    /// </summary>
    public class StudentService
    {
        private readonly List<Student> _students = new();

        public bool add (Student student)
        {
            // Kiem tra  ma trung
            bool exists = _students.Any(
                    s=>s.studentId.Equals(student.studentId,
                    StringComparison.OrdinalIgnoreCase));
            if (exists) return false;
            // Them sinh vien
            _students.Add(student);
            return true;
        }

        public IReadOnlyList<Student> getAll()
        {
            return _students;
        }
        //Tim theo ma
        public Student?  getById(string studentId)
        {
            // Tim theo ma

            return _students.FirstOrDefault(
                    s=>s.studentId.Equals(studentId, 
                                        StringComparison.OrdinalIgnoreCase)
                );
        }
    }
}
