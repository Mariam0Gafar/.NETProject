using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class SCService : ISCService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SCService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<bool>> Create(studentCourseBindingModel sc)
        {
            if (sc == null)
                return ServiceResult<bool>.Fail("Data is null");

            var s =_unitOfWork.StudentRepository.GetByIdAsync(sc.studentId);
            if (s == null)
            {
               return ServiceResult<bool>.Fail("Student id is invalid");

            }
            var c = _unitOfWork.CourseRepository.GetByIdAsync(sc.courseId);
            if (c == null)
            {
                return ServiceResult<bool>.Fail("Course id is invalid");
            }
            StudentCourse studentCourse = new StudentCourse()
            {
                studentId = sc.studentId,
                courseId = sc.courseId,
                Grade = sc.Grade

            };
            await _unitOfWork.SCRepository.AddAsync(studentCourse);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create relation");

        }

        public async Task<ServiceResult<bool>> Delete(int scId)
        {
            if (scId <= 0)
                return ServiceResult<bool>.Fail("Invalid ID");
            var scDetails = await _unitOfWork.SCRepository.GetByIdAsync(scId);
            if (scDetails == null)
                return ServiceResult<bool>.Fail("relation not found");

            _unitOfWork.SCRepository.Delete(scId);
            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to delete relation");

        }

        public async Task<ServiceResult<IEnumerable<StudentCourse>>> GetAll()
        {
            var scDetailsList = await _unitOfWork.SCRepository.GetAllAsync();
            return ServiceResult<IEnumerable<StudentCourse>>.Ok(scDetailsList);
        }

        public async Task<ServiceResult<StudentCourse>> GetById(int scId)
        {
            if (scId <= 0)
                return ServiceResult<StudentCourse>.Fail("Invalid ID");
            var scDetails = await _unitOfWork.SCRepository.GetByIdAsync(scId);
            if (scDetails == null)
                return ServiceResult<StudentCourse>.Fail("relation not found");
            return ServiceResult<StudentCourse>.Ok(scDetails);
        }

        public async Task<ServiceResult<bool>> Update(studentCourseBindingModel sc)
        {
            if (sc == null)
                return ServiceResult<bool>.Fail("relation data is null");
            var scDetails = await _unitOfWork.SCRepository.GetByIdAsync(sc.Id);
            if (scDetails == null)
                return ServiceResult<bool>.Fail("relation not found");
            var s = _unitOfWork.StudentRepository.GetByIdAsync(sc.studentId);
            if (s == null)
            {
                return ServiceResult<bool>.Fail("Student id is invalid");

            }
            var c = _unitOfWork.CourseRepository.GetByIdAsync(sc.courseId);
            if (c == null)
            {
                return ServiceResult<bool>.Fail("Course id is invalid");
            }
            scDetails.studentId = sc.studentId;
            scDetails.courseId = sc.courseId;
            scDetails.Grade = sc.Grade;

            _unitOfWork.SCRepository.Update(scDetails);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update relation");

        }

        public async Task<ServiceResult<CoursesWithDep>> GetCoursesByStudentId(int studentId)
        {
            if (studentId <= 0)
                return ServiceResult<CoursesWithDep>.Fail("Invalid ID");
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if(student == null)
                return ServiceResult<CoursesWithDep>.Fail("Student ID is invalid");
            var courses = await _unitOfWork.SCRepository.GetCoursesByStudentId(studentId);

            return ServiceResult<CoursesWithDep>.Ok(courses);
        }
    }
}
