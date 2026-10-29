
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private string? CurrentTeacher =>
            HttpContext.Session.GetString("TeacherUsername");

        private IActionResult? CheckLogin()
        {
            if (CurrentTeacher == null)
                return RedirectToAction("Login", "Account");

            return null;
        }

        public IActionResult Index(string search, string sortOrder)
        {
            var check = CheckLogin();
            if (check != null) return check;

            var students = AppData.Students
                .Where(s => s.TeacherUsername == CurrentTeacher)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                students = students.Where(s =>
                    s.Name.Contains(search,
                        StringComparison.OrdinalIgnoreCase) ||
                    s.Roll_No.ToString().Contains(search));
            }

            students = sortOrder switch
            {
                "name" => students.OrderBy(s => s.Name),
                "roll" => students.OrderBy(s => s.Roll_No),
                "dob" => students.OrderBy(s => s.DOB),
                "gender" => students.OrderBy(s => s.Gender),
                _ => students.OrderBy(s => s.Roll_No)
            };

            ViewBag.Search = search;
            ViewBag.SortOrder = sortOrder;

            return View(students.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var check = CheckLogin();
            if (check != null) return check;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            var check = CheckLogin();
            if (check != null) return check;

            // ID generation goes here
            student.Id = AppData.Students.Count == 0
                ? 1
                : AppData.Students.Max(s => s.Id) + 1;

            // Then assign the teacher
            student.TeacherUsername = CurrentTeacher!;

            // Finally save the student
            AppData.Students.Add(student);


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var check = CheckLogin();
            if (check != null) return check;

            var student = AppData.Students.FirstOrDefault(s =>
                s.Id == id &&
                s.TeacherUsername == CurrentTeacher);

            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            var check = CheckLogin();
            if (check != null) return check;

            var existing = AppData.Students.FirstOrDefault(s =>
                s.Id == student.Id &&
                s.TeacherUsername == CurrentTeacher);

            if (existing == null) return NotFound();

            existing.Name = student.Name;
            existing.Roll_No = student.Roll_No;
            existing.DOB = student.DOB;
            existing.Gender = student.Gender;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var check = CheckLogin();
            if (check != null) return check;

            var student = AppData.Students.FirstOrDefault(s =>
                s.Id == id &&
                s.TeacherUsername == CurrentTeacher);

            if (student != null)
                AppData.Students.Remove(student);

            return RedirectToAction("Index");
        }
    }
}
