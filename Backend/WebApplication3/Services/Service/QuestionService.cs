using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<bool>> Create(QuestionBindingModel q)
        {
            if (q == null)
                return ServiceResult<bool>.Fail("question data is null");

            Question c = new Question()
            {
                Description = q.Description,
                CorrectAnswer = q.CorrectAnswer,
                ExamId = q.ExamId
            };
            await _unitOfWork.QuestionRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create question");
        }

        public async Task<ServiceResult<Question>> GetById(int qId)
        {
            if (qId <= 0)
                return ServiceResult<Question>.Fail("Invalid question ID");

            var qDetails = await _unitOfWork.QuestionRepository.GetByIdAsync(qId);
            if (qDetails == null)
                return ServiceResult<Question>.Fail("Question not found");
            return ServiceResult<Question>.Ok(qDetails);
        }

        public async Task<ServiceResult<IEnumerable<QuestionBindingModel>>> GetQuestionsByExamId(int examId)
        {
            if (examId <= 0)
                return ServiceResult<IEnumerable<QuestionBindingModel>>.Fail("Invalid exam Id");
           
            var questions = await _unitOfWork.QuestionRepository.GetQuestionsByExamId(examId);

            return ServiceResult<IEnumerable<QuestionBindingModel>>.Ok(questions);
        }

        public async Task<ServiceResult<bool>> UpdateQuestion(QuestionBindingModel q)
        {
            if (q == null)
                return ServiceResult<bool>.Fail("question data is null");

            var Details = await _unitOfWork.QuestionRepository.GetByIdAsync(q.Id);
            if (Details == null)
                return ServiceResult<bool>.Fail("question not found");

            Details.Description = q.Description;
            Details.CorrectAnswer = q.CorrectAnswer;
            Details.ExamId = q.ExamId;

            _unitOfWork.QuestionRepository.Update(Details);
            var result = _unitOfWork.SaveChanges();

            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update question");
        }
    }
}
