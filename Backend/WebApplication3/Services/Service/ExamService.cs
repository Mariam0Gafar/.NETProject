using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Repository.Repo;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<bool>> Active(int examId)
        {
      
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(examId);
            if (exam == null)
                return ServiceResult<bool>.Fail("exam not found");

            if (exam.isActive == true)
            {
                return ServiceResult<bool>.Fail("Exam already active!");
            }
            exam.isActive = true;

            _unitOfWork.ExamRepository.Update(exam);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update exam");
        }

        public async Task<Exam> Create(ExamBindingModel exam)
        {
            if (exam == null)
                return null;

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(exam.CourseId);
            if (course == null)
                return null;

            Exam c = new Exam()
            {
                CourseId = exam.CourseId,
                Title = exam.Title,
                isActive = exam.isActive,
            };
            await _unitOfWork.ExamRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();

            return result > 0 ? c : null;
        }

        public async Task<ServiceResult<bool>> Delete(int examId)
        {
            if (examId <= 0)
                return ServiceResult<bool>.Fail("Invalid exam ID");

            var Details = await _unitOfWork.ExamRepository.GetByIdAsync(examId);
            if (Details == null)
                return ServiceResult<bool>.Fail("exam not found");

            _unitOfWork.ExamRepository.Delete(examId);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to delete exam");
        }

        public async Task<IEnumerable<ExamViewModel>> GetAllExams(Expression<Func<Exam, bool>> filter = null, Expression<Func<Exam, object>> sortBy = null, bool descending = false)
        {
            Expression<Func<Exam, ExamViewModel>> selector = x => new ExamViewModel()
            {
                Id = x.Id,
                CourseId = x.CourseId,
                Title = x.Title,
                isActive = x.isActive
            };
            return await _unitOfWork.ExamRepository.GetAllAsync(selector: selector, filter: filter, sortBy: sortBy, descending: descending);
        }

        public async Task<ExamViewModel> GetExamById(int examid)
        {

            return await _unitOfWork.ExamRepository.GetExamWithQuestions(examid);

        }

        public async Task<ExamBindingModel> GetExam(int examid)
        {

            if (examid <= 0)
                return null;

            var examDetails = await _unitOfWork.ExamRepository.GetByIdAsync(examid);
            if (examDetails != null)
            {
                return new ExamBindingModel()
                {
                    Id = examid,
                    Title = examDetails.Title,
                    CourseId = examDetails.CourseId,
                    isActive = examDetails.isActive
                };
            }
            return null;
        }

        public async Task<ServiceResult<bool>> Update(ExamBindingModel exam)
        {
            if (exam == null)
                return ServiceResult<bool>.Fail("exam data is null");

            var Details = await _unitOfWork.ExamRepository.GetByIdAsync(exam.Id);
            if (Details == null)
                return ServiceResult<bool>.Fail("exam not found");

            Details.CourseId = exam.CourseId;
            Details.isActive = exam.isActive;

            _unitOfWork.ExamRepository.Update(Details);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update exam");
        }

        public async Task<PagedList<ExamViewModel>> GetPaginatedExams(
                    string? name,
                    string? sortBy,
                    string? sortOrder,
                    int page,
                    int pageSize,
                    CancellationToken cancellationToken)
        {
            Expression<Func<Exam, bool>> filter = string.IsNullOrWhiteSpace(name) ? null : c => c.Title.Contains(name);

            Expression<Func<Exam, object>> orderBy = sortBy?.ToLower() switch
            {
                "title" => c => c.Title,
                "id" => c => c.Id,
                _ => c => c.Title
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            IQueryable<Exam> exams = _unitOfWork.ExamRepository.GetAllQueryable();

            if (filter != null)
                exams = exams.Where(filter);

            exams = isDescending ? exams.OrderByDescending(orderBy) : exams.OrderBy(orderBy);

            var response =  exams.Select(c => new ExamViewModel
            {
                Id = c.Id,
                Title = c.Title,
                isActive = c.isActive,
                CourseId = c.CourseId
            });

            return await PagedList<ExamViewModel>.CreateAsync(response, page, pageSize, cancellationToken);
        }


    }
}
