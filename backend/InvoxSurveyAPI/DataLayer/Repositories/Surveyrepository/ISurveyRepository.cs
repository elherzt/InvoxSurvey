using DataLayer.Models;
using DataLayer.Repositories.OptionRepository;
using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataLayer.Repositories.Surveyrepository.SurveyDTO;

namespace DataLayer.Repositories.Surveyrepository
{
    public interface ISurveyRepository
    {
        Task<CustomResponse> GetAll(int user_id);
        Task<CustomResponse> GetById(int id);
        Task<CustomResponse> GetByStatus(SurveyStatus status, int user_id);
        Task<CustomResponse> Create(SurveyCreateDTO model);
        Task<CustomResponse> Update(SurveyCreateDTO model);
        Task<CustomResponse> Delete(int id);
        Task<CustomResponse> SaveAnswer(SurveyAnswerDTO model);
        Task<CustomResponse> UpdateSurveyWithoutSections(SurveyWithoutSectiosDTO model);
        Task<CustomResponse> Publicar(int id);

        //pending move to another repository, like ReportsRepository
        Task<CustomResponse> GetOpenAnswer(int survey_id, int question_id);   
        Task<CustomResponse> GetResults(int id);  
        Task<CustomResponse> GetResultsByFilters(int id, List<int> filters); 

    }


    public class SurveyRepository : ISurveyRepository
    {
        private readonly AppDbContext _context;

        public SurveyRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<CustomResponse> GetByStatus(SurveyStatus status, int user_id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");
            try
            {
                List<SurveyDTO> surveys = new List<SurveyDTO>();

                switch (status)
                {
                    case SurveyStatus.Draft:
                        surveys = await _context.Surveys.Where(s => s.StatusId == (int)SurveyStatus.Draft && s.UserId == user_id).Select(s => new SurveyDTO(s, false)).ToListAsync();
                        break;
                    case SurveyStatus.Published:
                        surveys = await _context.Surveys.Where(s => s.StatusId == (int)SurveyStatus.Published && s.UserId == user_id).Select(s => new SurveyDTO(s, false)).ToListAsync();
                        break;
                    case SurveyStatus.Finished:
                        surveys = await _context.Surveys.Where(s => s.StatusId == (int)SurveyStatus.Finished && s.UserId == user_id).Select(s => new SurveyDTO(s, false)).ToListAsync();
                        break;
                    case SurveyStatus.Archived:
                        surveys = await _context.Surveys.Where(s => s.StatusId == (int)SurveyStatus.Archived && s.UserId == user_id).Select(s => new SurveyDTO(s, false)).ToListAsync();
                        break;
                    default:
                        break;
                }

                if (surveys.Count == 0)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Surveys not found");
                    response.Data = new List<SurveyDTO>();
                }
                else
                {
                    response.Data = surveys;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

       

        public async Task<CustomResponse> GetAll(int user_id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");

            try
            {
                var surveys = await _context.Surveys.Where(x=> x.UserId == user_id).Select(s => new SurveyDTO(s, true)).ToListAsync();
                if (surveys.Count == 0)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Surveys not found");
                }
                else
                {

                    response.Data = surveys;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }


        public async Task<CustomResponse> GetById(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");
            try
            {
                var survey = await _context.Surveys
                .Include(s => s.Sections)
                    .ThenInclude(s => s.Questions)
                        .ThenInclude(s => s.Options)
                .Where(s => s.Id == id)
                .Select(s => new SurveyDTO(s, true))
                .FirstOrDefaultAsync();

                if (survey == null)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Surveys not found");
                }
                else
                {
                    response.Data = survey;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> GetResults(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");
            try
            {
                var survey = await _context.Surveys
                .Include(s => s.Sections)
                    .ThenInclude(s => s.Questions)
                        .ThenInclude(s => s.Options)
                .Where(s => s.Id == id)
                .Select(s => new SurveyAnswerDTO(s))
                .FirstOrDefaultAsync();

                if (survey == null)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Surveys not found");
                }
                else
                {
                    //get total answers
                    survey.Answers = _context.Answers
                        .Where(x => x.SurveyId == id)
                        .Count();

                    //get answers
                    foreach (var section in survey.Sections)
                    {
                        foreach (var question in section.Questions)
                        {
                            var answers = _context.AnswerOptions
                              .Include(x => x.Option)
                                  .ThenInclude(x => x.Question)
                              .Where(x => x.Option.QuestionId == question.Id)
                              .GroupBy(x => x.Option)
                              .Select(x => new QuestionAnswerOptionDTO
                              {
                                  Option_Id = x.Key.Id,
                                  Text = x.Key.Description,
                                  Count = x.Count(),
                                  IsOpen = x.Key.IsOpen
                              }).OrderByDescending(o => o.Count).ToList();
                            survey.Sections.Where(x => x.Id == section.Id).FirstOrDefault().Questions.Where(x => x.Id == question.Id).FirstOrDefault().Answers = answers;
                        }
                    }

                    response.Data = survey;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> GetResultsByFilters(int id, List<int> filterIds)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");
            try
            {
                var survey = await _context.Surveys
                 .Include(s => s.Sections)
                     .ThenInclude(s => s.Questions)
                         .ThenInclude(s => s.Options)
                 .Where(s => s.Id == id)
                 .Select(s => new SurveyAnswerDTO(s))
                 .FirstOrDefaultAsync();

                if (survey == null)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Surveys not found");
                }
                else
                {

                    

                    var included_answers = await _context.Answers
                        .Include(a => a.AnswerOptions)
                            .ThenInclude(ao => ao.Option)
                        // Filtra Answers donde TODOS los filterIds estén en sus AnswerOptions
                        .Where(a => filterIds.ToArray().All(fid =>
                            a.AnswerOptions.Any(ao => ao.OptionId == fid)
                        ))
                        .ToListAsync();

                    survey.Answers = included_answers.Count();
                    var filteredAnswerIds = included_answers.Select(a => a.Id).ToList();


                    //get answers
                    foreach (var section in survey.Sections)
                    {
                        foreach (var question in section.Questions)
                        {
                            var answers = _context.AnswerOptions
                              .Include(x => x.Option)
                                  .ThenInclude(x => x.Question)
                              .Where(x => x.Option.QuestionId == question.Id && filteredAnswerIds.ToArray().Contains(x.AnswerId))
                              .GroupBy(x => x.Option)
                              .Select(x => new QuestionAnswerOptionDTO
                              {
                                  Option_Id = x.Key.Id,
                                  Text = x.Key.Description,
                                  Count = x.Count(),
                                  IsOpen = x.Key.IsOpen
                              }).OrderByDescending(o => o.Count).ToList();
                            survey.Sections.Where(x => x.Id == section.Id).FirstOrDefault().Questions.Where(x => x.Id == question.Id).FirstOrDefault().Answers = answers;
                        }
                    }

                    string AppliedFilters = "";
                    foreach (var filter in filterIds)
                    {
                        var option = await _context.Options
                            .Include(q => q.Question)
                            .Where(x => x.Id == filter)
                            .FirstOrDefaultAsync();

                        if (option != null)
                        {
                            AppliedFilters += $@"| {option.Question.Description}: <b> {option.Description} </b>";
                        }
                    }

                    survey.Filters = AppliedFilters;

                    response.Data = survey;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> GetOpenAnswer(int survey_id, int question_id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "OK");
            try
            {
                var answers = await _context.AnswerOptions
                    .Include(x => x.Answer)
                    .Include(x => x.Option)
                    .Where(x => x.OptionId == question_id && x.Answer.SurveyId == survey_id)
                    .Select(x => new QuestionAnswerOptionDTO
                    {
                        Option_Id = x.Option.Id,
                        Text = x.Text,
                        Audio = x.Audio
                    }).ToListAsync();

                if (answers.Count == 0)
                {
                    response = new CustomResponse(TypeOfResponse.FailedResponse, "Questions not found");
                }
                else
                {
                    response.Data = answers;
                }
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> SaveAnswer(SurveyAnswerDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey saved, thaks.");
            try
            {
                var place = await _context.Places.FindAsync(model.PlaceId);


                int minutes = 0;
                if (model.EndDate.HasValue && model.EndDate.HasValue)
                {
                    minutes = (model.EndDate.Value - model.StartDate.Value).Minutes;
                }



                Answer answer = new Answer
                {
                    SurveyId = model.Id,
                    Date = DateTime.Now,
                    UserId = model.UserId,
                    UserName = model.UserName,
                    PlaceId = model.PlaceId,
                    Place = place.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Time = minutes
                };


                _context.Answers.Add(answer);
                _context.SaveChanges();
                //asnweroptions have an audio


                foreach (var question in model.Questions)
                {
                    foreach (var item in question.Answers)
                    {
                        if (item.Option_Id != -1) // -1 is used for skiped answers, front must be send -1 
                        {
                            AnswerOption aws = new AnswerOption();
                            aws.AnswerId = answer.Id;
                            aws.OptionId = item.Option_Id;
                            aws.Text = item.Text;
                            aws.Audio = item?.Audio; // Is audio needed? pending to review, it coud be only open-ended questions
                            _context.AnswerOptions.Add(aws);
                        }

                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }


        public async Task<CustomResponse> Create(SurveyCreateDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey created successfully");
            try
            {
                

                // Validate sections
                if (model.Sections == null || !model.Sections.Any())
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Survey must contain at least one section");
                }

                Survey survey = new Survey
                {
                    Name = model.Name,
                    Description = model.Description,
                    Instructions = model.Instructions,
                    CreatedDate = DateTime.Now,
                    Target = model.Target,
                    Place = model.Place,
                    UserId = model.UserId,
                    Sections = new List<Section>()
                };

                foreach (var section in model.Sections)
                {


                    // validate questions in section
                    if (section.Questions == null || !section.Questions.Any())
                    {
                        return new CustomResponse(TypeOfResponse.FailedResponse, "Each section must contain at least one question.");
                    }

                    Section sec = new Section
                    {
                        Description = section.Description,
                        Questions = new List<Question>()
                    };

                    foreach (var question in section.Questions)
                    {
                        // Question description validation based on type
                        if (string.IsNullOrWhiteSpace(question.Description)) //allow empty description for open-ended questions (?) pending to review
                        {
                            if (question.TypeId < 3) // pending to review
                            {
                                return new CustomResponse(TypeOfResponse.FailedResponse, "Questions of this type must have a description.");
                            }
                            question.Description = "Open-ended question"; 
                        }

                        Question qst = new Question
                        {
                            Description = question.Description,
                            TypeId = question.TypeId,
                            Options = new List<Option>()
                        };

                        if (question.TypeId <= 3) 
                        {
                            if (question.Options == null || !question.Options.Any())
                            {
                                return new CustomResponse(TypeOfResponse.FailedResponse, "Questions of this type must contain options.");
                            }

                            foreach (var option in question.Options)
                            {
                                if (string.IsNullOrWhiteSpace(option.Description))
                                {
                                    return new CustomResponse(TypeOfResponse.FailedResponse, "Options cannot be empty.");
                                }

                                Option opt = new Option
                                {
                                    Description = option.Description,
                                    IsOpen = option.IsOpen,
                                    SkipSection = option.SkipSection
                                };
                                qst.Options.Add(opt);
                            }
                        }

                        sec.Questions.Add(qst);
                    }
                    survey.Sections.Add(sec);
                }

                _context.Surveys.Add(survey);
                await _context.SaveChangesAsync();

                response.Data = new SurveyDTO(survey, true);
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> Publicar(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey published successfully.");
            try
            {
                var survey = await _context.Surveys.FindAsync(id);
                if (survey == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Survey not found");
                }
                survey.StatusId = (int)SurveyStatus.Published;
                _context.Surveys.Update(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        //update survey like add survey
        public async Task<CustomResponse> Update(SurveyCreateDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey updated successfully.");
            try
            {
                var survey = await _context.Surveys.FindAsync(model.id);
                if (survey == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Survey not found");
                }
                survey.Name = model.Name;
                survey.Description = model.Description;
                survey.Instructions = model.Instructions;
                survey.CreatedDate = DateTime.Now;
                survey.Place = model.Place;
                survey.UserId = model.UserId;
                _context.Surveys.Update(survey);
                await _context.SaveChangesAsync();

                response.Data = new SurveyDTO(survey, false);
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        //change status to finished
        public async Task<CustomResponse> Delete(int id)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey deleted successfully.");
            try
            {
                var survey = await _context.Surveys.FindAsync(id);
                if (survey == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Survey not found");
                }
                survey.StatusId = (int)SurveyStatus.Finished;
                _context.Surveys.Update(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return response;
        }

        public async Task<CustomResponse> UpdateSurveyWithoutSections(SurveyWithoutSectiosDTO model)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Survey updated successfully");
            try
            {
                var survey = await _context.Surveys.FindAsync(model.Id);
                if (survey == null)
                {
                    return new CustomResponse(TypeOfResponse.FailedResponse, "Survey not found");
                }

                survey.Name = model.Name;
                survey.Description = model.Description;
                survey.Instructions = model.Instructions;
                survey.Place = model.Place;
                survey.Target = model.Target;
                survey.UserId = model.UserId;
                _context.Surveys.Update(survey);
                await _context.SaveChangesAsync();

                response.Data = new SurveyDTO(survey, false);
            }

            catch (Exception ex)
            {
                response = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }

            return response;
        }

        
    }
}
