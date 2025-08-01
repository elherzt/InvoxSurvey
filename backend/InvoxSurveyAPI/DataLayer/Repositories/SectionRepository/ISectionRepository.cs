using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.SectionRepository
{
    public interface ISectionRepository
    {
        Task<CustomResponse> GetAll();
        Task<CustomResponse> Add(AddSectionDTO model);
        Task<CustomResponse> Update(AddSectionDTO model);
        Task<CustomResponse> Delete(int id);
    }

    internal class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;

        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse> GetAll()
        {
            CustomResponse res = new CustomResponse(TypeOfResponse.OK, "Sections Retrieved Successfully");
            try
            {
                var sections = await _context.Sections.Select(s => new SectionDTO(s, true)).ToListAsync();
                if (sections.Count == 0)
                {
                    res = new CustomResponse(TypeOfResponse.FailedResponse, "No Sections Found");
                }
                else
                {
                    res.Data = sections;
                }
            }
            catch (Exception ex)
            {
                res = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }

            return res;
        }

        public async Task<CustomResponse> Add(AddSectionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Section added successfully.");
            try
            {
                var survey = await _context.Surveys.FindAsync(model.SurveyId);
                if (survey == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Section not found");
                }
                Section section = new Section
                {
                    Description = model.Description,
                    SurveyId = model.SurveyId,
                };

                _context.Sections.Add(section);
                await _context.SaveChangesAsync();

                response.Data = new SectionDTO(section, true);
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> Delete(int id)
        {
            CustomResponse res = new CustomResponse(TypeOfResponse.OK, "Section deleted successfully.");
            try
            {
                var section = await _context.Sections.FindAsync(id);
                if (section == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Section not found");
                }
                var questions = await _context.Questions
                    .Where(q => q.SectionId == section.Id)
                    .ToListAsync();

                var options = await _context.Options
                    .Where(o => questions.Select(q => q.Id).Contains(o.QuestionId))
                    .ToListAsync();

                _context.Options.RemoveRange(options);
                _context.Questions.RemoveRange(questions);
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.TypeOfResponse = TypeOfResponse.Exception;
            }
            return res;
        }

        public async Task<CustomResponse> Update(AddSectionDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Section updated successfully.");
            try
            {
                var section = await _context.Sections.FindAsync(model.Id);
                if (section == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Section not found");
                }
                section.Description = model.Description;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();
                response.Data = new SectionDTO(section, false);
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }
    }
}
