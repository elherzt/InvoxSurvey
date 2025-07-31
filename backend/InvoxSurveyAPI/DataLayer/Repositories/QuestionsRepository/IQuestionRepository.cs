using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.QuestionsRepository
{
    public interface IQuestionRepository
    {
        public List<QuestionAnswerOptionDTO> GetAnswers(int question_id);
        public List<QuestionTypesDTO> GetQuestionTypes();
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<QuestionAnswerOptionDTO> GetAnswers(int question_id)
        {
            List<QuestionAnswerOptionDTO> answers = new List<QuestionAnswerOptionDTO>();
            try
            {
                answers = _context.AnswerOptions
                    .Include(x => x.Option)
                        .ThenInclude(x => x.Question)
                    .Where(x => x.Option.QuestionId == question_id)
                    .GroupBy(x => x.Option)
                    .Select(x => new QuestionAnswerOptionDTO
                    {
                        Option_Id = x.Key.Id,
                        Text = x.Key.Description,
                        Count = x.Count(),
                        IsOpen = x.Key.IsOpen
                    }).ToList();
            }
            catch (Exception ex)
            {
                return new List<QuestionAnswerOptionDTO>();
            }
            return answers;
        }

        public List<QuestionTypesDTO> GetQuestionTypes()
        {
            List<QuestionTypesDTO> types = new List<QuestionTypesDTO>();
            try
            {
                types = _context.QuestionsTypes
                    .Select(x => new QuestionTypesDTO
                    {
                        Id = x.Id,
                        Description = x.Description
                    }).ToList();
            }
            catch (Exception ex)
            {
                return new List<QuestionTypesDTO>();
            }
            return types;
        }


    }

}
