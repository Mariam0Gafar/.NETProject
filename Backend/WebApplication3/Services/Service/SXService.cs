using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class SXService : ISXService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SXService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<bool>> AddGrade(int sxId, int grade)
        {
            var studentExam = await _unitOfWork.SXRepositroy.GetByIdAsync(sxId);
            if (studentExam == null)
            {
                return ServiceResult<bool>.Fail("Student didnt take exam!");
            }
            if (studentExam.isSubmitted == false)
            {
                return ServiceResult<bool>.Fail("Student didn't submit yet!");
            }
            studentExam.Grade = grade;

            _unitOfWork.SXRepositroy.Update(studentExam);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to add grade!");
        }

        public async Task<IEnumerable<StudentExamViewModel>> GetAllStudentExams(Expression<Func<StudentExam, bool>> filter = null, Expression<Func<StudentExam, object>> sortBy = null, bool descending = false)
        {
            Expression<Func<StudentExam, StudentExamViewModel>> selector = x => new StudentExamViewModel()
            {
                StudentId = x.StudentId,
                ExamId = x.ExamId,
                isSubmitted = x.isSubmitted,
                Grade = x.Grade
            };
            return await _unitOfWork.SXRepositroy.GetAllAsync(selector: selector, filter: filter, sortBy: sortBy, descending: descending);
        }



        public async Task<IEnumerable<StudentExamViewModel>> GetGradesForStudent(int studentId)
        {
            Expression<Func<StudentExam, StudentExamViewModel>> selector = x => new StudentExamViewModel()
            {
                StudentId = x.StudentId,
                ExamId = x.ExamId,
                Grade = x.Grade,
                isSubmitted = x.isSubmitted
            };
            return await _unitOfWork.SXRepositroy.GetAllAsync(selector, filter: x => x.StudentId == studentId && x.Grade != null);
        }

        public async Task<ServiceResult<bool>> SubmitExam(int sxId)
        {
            var studentExam = await _unitOfWork.SXRepositroy.GetByIdAsync(sxId);
            if (studentExam == null)
            {
                return ServiceResult<bool>.Fail("Student didnt take exam!");
            }
            if(studentExam.isSubmitted == true)
            {
                return ServiceResult<bool>.Fail("Already submitted!");
            }
            studentExam.isSubmitted = true;
            
            _unitOfWork.SXRepositroy.Update(studentExam);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to submit exam");
        }

        public async Task<ServiceResult<bool>> TakeExam(int studentId, int examId)
        {
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(examId);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                return ServiceResult<bool>.Fail("Student not valid!");
            }
            if (exam != null)
            {
                if (exam.isActive)
                {
                    await _unitOfWork.SXRepositroy.AddAsync(new StudentExam()
                    {
                        ExamId = examId,
                        StudentId = studentId,
                        isSubmitted = false,
                    });

                    var result = _unitOfWork.SaveChanges();
                    return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create student exam relation");
                }
                return ServiceResult<bool>.Fail("Exam is not active!");
            }
            return ServiceResult<bool>.Fail("Exam not found!");
        }

        public async Task<(double grade, int correct, int total)> Submit(SubmitExamBindingModel x)
        {
           
            var exam = await _unitOfWork.ExamRepository.GetExamWithQuestions(x.ExamId);
            var studentExam = new StudentExam()
            {
                StudentId = x.StudentId,
                ExamId = x.ExamId,
                studentAnswers = new List<StudentAnswer>()
            };
            int correct = 0;
            foreach(var answer in x.studentAnswers)
            {
                var question = await _unitOfWork.QuestionRepository.GetByIdAsync(answer.questionId);
                if (question == null) continue;              
                if (string.Equals(question.CorrectAnswer,answer.answer)) correct++;
                studentExam.studentAnswers.Add(new StudentAnswer()
                {
                    questionId = answer.questionId,
                    studentAnswer = answer.answer,
                    
                });
            }
            int total = exam.Questions.Count();
            double grade = ((double)correct / total) * 100;
            
            studentExam.Grade = grade;
            studentExam.isSubmitted = true;
            _unitOfWork.SXRepositroy.Update(studentExam);
            var result = _unitOfWork.SaveChanges();
            return (grade, correct, total);
        }

        public async Task<SubmitExamBindingModel> PrepareSubmissionAsync(int examId, int studentId)
        {
            var exam = await _unitOfWork.ExamRepository.GetExamWithQuestions(examId);
            if (exam == null)
                return null;

            return new SubmitExamBindingModel
            {
                Title = exam.Title,
                ExamId = examId,
                StudentId = studentId,
                isActive = exam.isActive,
                studentAnswers = exam.Questions.Select(q => new StudentAnswerViewModel
                {
                    questionId = q.Id,
                    question = q.Question,
                    answer = ""
                }).ToList()
            };
        }

        public async Task<IEnumerable<GradesForExamViewModel>> GetGradesForExam(int examId)
        {
            return  await _unitOfWork.SXRepositroy.GetExamWithStudent(examId);
           
        }
        public async Task<IEnumerable<GradesForExamViewModel>> GetExamWithStudent(int examId)
        {
            return await _unitOfWork.SXRepositroy.GetExamWithStudent(examId);

        }

    }
}
