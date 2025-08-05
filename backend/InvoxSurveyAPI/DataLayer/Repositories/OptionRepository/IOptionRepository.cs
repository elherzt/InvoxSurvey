using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.OptionRepository
{
    public interface IOptionRepository
    {
        Task<CustomResponse> Add(OptionDTO model);
        Task<CustomResponse> Update(OptionDTO model);
        Task<CustomResponse> Delete(int id);
    }

    public class OptionRepository : IOptionRepository
    {
        private readonly AppDbContext _context;

        public OptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse> Add(OptionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Option added successfully");
            try
            {
                var question = await _context.Questions.FindAsync(model.question_id);
                if (question == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Question not found");
                }
                if (model.NextQuestionId <= 0)
                {
                    model.NextQuestionId = null;
                }
                Option option = new Option
                {
                    Description = model.Description,
                    QuestionId = model.question_id,
                    IsOpen = model.IsOpen,
                    SkipSection = model.SkipSection,
                    NextQuestionId = model.NextQuestionId
                };
                _context.Options.Add(option);
                await _context.SaveChangesAsync();
                response.Data = model;
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);

            }
            return response;
        }

        public async Task<CustomResponse> Update(OptionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Option edited successfully.");
            Option option = new Option();
            try
            {
                option = await _context.Options.FindAsync(model.Id);
                if (option == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Option not found");
                }
                if (model.NextQuestionId <= 0)
                {
                    model.NextQuestionId = null;
                }
                option.Description = model.Description;
                option.IsOpen = model.IsOpen;
                option.SkipSection = model.SkipSection;
                option.NextQuestionId = model.NextQuestionId;
                _context.Options.Update(option);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }

            return response;
        }
        public async Task<CustomResponse> Delete(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Option deletd successfully");
            try
            {
                var option = await _context.Options.FindAsync(id);
                if (option == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Option not found");
                }
                _context.Options.Remove(option);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }
    }
}
