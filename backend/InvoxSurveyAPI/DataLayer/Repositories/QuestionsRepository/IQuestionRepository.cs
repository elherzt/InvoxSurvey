using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.QuestionsRepository
{
    public interface IQuestionRepository
    {
        Task<CustomResponse> Add(AddUpdateQuestionDTO model);
        Task<CustomResponse> Update(AddUpdateQuestionDTO model);
        Task<CustomResponse> Delete(int id);

        List<QuestionAnswerOptionDTO> GetAnswers(int question_id);
        List<QuestionTypesDTO> GetQuestionTypes();


    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<CustomResponse> Update(AddUpdateQuestionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Question updated successfully.");
            try
            {
                var question = await _context.Questions.FindAsync(model.Id);
                if (question == null)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "No se encontro la pregunta";
                }
                else
                {
                    question.Description = model.Description;
                    question.TypeId = model.TypeId;
                    question.HasOther = model.Other;
                    _context.Questions.Update(question);
                    await _context.SaveChangesAsync();
                }

                response.Data = model;
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> Add(AddUpdateQuestionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Question added successfully.");
            try
            {
                var question = new Question
                {
                    Description = model.Description,
                    TypeId = model.TypeId,
                    HasOther = model.Other,
                    SectionId = model.SectionId
                };
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                response.Data = new QuestionDTO(question, true);
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> Delete(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Question deleted successfully");
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Question not found");
                }


                var options = await _context.Options
                    .Where(x => x.QuestionId == question.Id)
                    .ToListAsync();


                _context.Options.RemoveRange(options);


                _context.Questions.Remove(question);


                await _context.SaveChangesAsync();

                response.Data = question;
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
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
