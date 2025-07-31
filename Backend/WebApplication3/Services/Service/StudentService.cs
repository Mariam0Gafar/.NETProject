using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Repository.Repo;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<bool>> Create(studentBindingModel student)
        {
            if (student == null)
                return ServiceResult<bool>.Fail("Student data is null");

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(student.DepartmentId);
            if (department == null)
                return ServiceResult<bool>.Fail("The department ID specified is not valid!");

            Student c = new Student()
            {
                Name = student.Name,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                DepartmentId = student.DepartmentId,
            };
            await _unitOfWork.StudentRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create student");
        }

        public async Task<ServiceResult<bool>> Delete(int studentId)
        {
            if (studentId <= 0)
                return ServiceResult<bool>.Fail("Invalid student ID");

            var studentDetails = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (studentDetails == null)
                return ServiceResult<bool>.Fail("Student not found");

            _unitOfWork.StudentRepository.Delete(studentId);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to delete student");
        }

        public async Task<ServiceResult<IEnumerable<Student>>> GetAll()
        {
            var studentDetailsList = await _unitOfWork.StudentRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Student>>.Ok(studentDetailsList);
        }

        public async Task<ServiceResult<studentViewModel>> GetById(int studentId)
        {
            if (studentId <= 0)
                return ServiceResult<studentViewModel>.Fail("Invalid student ID");

            var studentDetails = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (studentDetails == null)
                return ServiceResult<studentViewModel>.Fail("Student not found");
            var studentViewModel =  new studentViewModel
            {
                Name = studentDetails.Name,
                Email = studentDetails.Email,
                PhoneNumber = studentDetails.PhoneNumber,
                DepartmentId = studentDetails.DepartmentId
            };
            return ServiceResult<studentViewModel>.Ok(studentViewModel);
        }

        public async Task<ServiceResult<bool>> Update(studentBindingModel student)
        {
            if (student == null)
                return ServiceResult<bool>.Fail("Student data is null");

            var studentDetails = await _unitOfWork.StudentRepository.GetByIdAsync(student.Id);
            if (studentDetails == null)
                return ServiceResult<bool>.Fail("Student not found");

            studentDetails.Name = student.Name;
            studentDetails.Email = student.Email;
            studentDetails.PhoneNumber = student.PhoneNumber;

            _unitOfWork.StudentRepository.Update(studentDetails);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update student");
        }

        public async Task<ServiceResult<IEnumerable<studentViewModel>>> GetStudentsByDepId(int depId)
        {
            if (depId <= 0)
                return ServiceResult<IEnumerable<studentViewModel>>.Fail("Invalid Department Id");
            var dep = await _unitOfWork.DepartmentRepository.GetByIdAsync(depId);
            if(dep == null)
                return ServiceResult<IEnumerable<studentViewModel>>.Fail("Department not found");
            var students = await _unitOfWork.StudentRepository.GetStudentsWithDepId(depId);
           
            return ServiceResult<IEnumerable<studentViewModel>>.Ok(students);

        }

        public async Task<ServiceResult<IEnumerable<studentWithCourseViewModel>>> GetStudentCoursesWithDepId(int depId)
        {
            if (depId <= 0)
                return ServiceResult<IEnumerable<studentWithCourseViewModel>>.Fail("Invalid Department Id");
            var dep = await _unitOfWork.DepartmentRepository.GetByIdAsync(depId);
            if (dep == null)
                return ServiceResult<IEnumerable<studentWithCourseViewModel>>.Fail("Department not found");
            var students = await _unitOfWork.StudentRepository.GetStudentCoursesWithDepId(depId);

            return ServiceResult<IEnumerable<studentWithCourseViewModel>>.Ok(students);
        }

        public async Task<ServiceResult<bool>> TakeExam(int studentId, int examId)
        {
            if (examId <= 0)
                return ServiceResult<bool>.Fail("Invalid exam Id");
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(examId);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if(student == null)
            {
                return ServiceResult<bool>.Fail("Student Id not valid!");
            }
            if(exam != null)
            {
                if (exam.isActive)
                {
                    await _unitOfWork.SXRepositroy.AddAsync(new StudentExam()
                    {
                        ExamId = examId,
                        StudentId = studentId,
                        isSubmitted = false
                    });
                    return ServiceResult<bool>.Ok(true);
                }
                return ServiceResult<bool>.Fail("Exam is not active!");
            }
            return ServiceResult<bool>.Fail("Exam not found!");
        }

        public async Task<bool> SubmitExam(int examId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<studentViewModel>> GetPaginatedStudents(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            Expression<Func<Student, bool>> filter = string.IsNullOrWhiteSpace(name) ? null : c => c.Name.Contains(name);

            Expression<Func<Student, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => c => c.Name,
                "email" => c => c.Email,
                "phone-number" => c => c.PhoneNumber,
                "id" => c => c.Id,
                _ => c => c.Name
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            IQueryable<Student> s = _unitOfWork.StudentRepository.GetAllQueryable();

            if (filter != null)
                s = s.Where(filter);

            s = isDescending ? s.OrderByDescending(orderBy) : s.OrderBy(orderBy);

            var response = s.Select(c => new studentViewModel
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                DepartmentId = c.DepartmentId
            });

            return await PagedList<studentViewModel>.CreateAsync(response, page, pageSize, cancellationToken);
        }
    }
}
