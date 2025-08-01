using DataLayer.Repositories.CatalogsRepository;
using DataLayer.Repositories.OptionRepository;
using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using DataLayer.Repositories.Surveyrepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Enums;
using System.Security.Claims;
using static DataLayer.Repositories.Surveyrepository.SurveyDTO;

namespace InvoxSurveyAPI.Controllers
{

    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class SurveysController : UtilityController
    {
        public SurveysController(
            ISurveyRepository surveyRepository,
            ISectionRepository sectionRepository,
            IQuestionRepository questionRepository,
            IOptionRepository optionRepository)
            : base(surveyRepository, sectionRepository, questionRepository, optionRepository)
        { }

            [HttpGet]
        public async Task<IActionResult> All()
        {
            int userId = GetUserID();
            CustomResponse response = await _surveyRepository.GetAll(userId);
            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStatus(SurveyQueryDTO query)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.FailedResponse, "Status not found");
            int userId = GetUserID();
            CustomResponse res = await _surveyRepository.GetByStatus(query.Status.Value, userId);
            return Ok(res);
        }

        

        [HttpGet]
        public async Task<IActionResult> Results(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.GetResults(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResultsByFilters(SurveyAnswerFilterDTO survey_filters)
        {
            CheckOwnership(survey_id: survey_filters.Id);
            CustomResponse response = await _surveyRepository.GetResultsByFilters(survey_filters.Id, survey_filters.Filters);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ShowOpenAnswer(SurveyOpenAnswerDTO model)
        {
            CheckOwnership(survey_id: model.SurveyId);
            CustomResponse response = await _surveyRepository.GetOpenAnswer(model.SurveyId, model.QuestionId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Answer(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.GetById(id);
            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> Create(SurveyCreateDTO model)
        {
            CustomResponse response = new CustomResponse();
            try
            {
                var userIdClaim = User?.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                if (userIdClaim == null)
                {
                    response.Message = "User is not authenticated or NameIdentifier claim is missing.";
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    return Ok(response);
                }
                int userId = Convert.ToInt32(userIdClaim);
            
                model.UserId = userId;
                response = await _surveyRepository.Create(model);
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.TypeOfResponse = TypeOfResponse.Exception;
            }
            return Ok(response);
        }


     
        public async Task<IActionResult> Edit(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.GetById(id);
            return Ok(response);
        }

        

        public async Task<IActionResult> Update(SurveyCreateDTO model)
        {
            CheckOwnership(survey_id: model.id);
            CustomResponse response = await _surveyRepository.Update(model);
            return Ok(response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.Delete(id);
            return Ok(response);
        }

        public async Task<IActionResult> PublicarEncuesta(int id)
        {
            CheckOwnership(survey_id: id);
            CustomResponse response = await _surveyRepository.Publicar(id);
            return Ok(response);
        }

        public async Task<IActionResult> UpdateSurveyWithoutSections(SurveyWithoutSectiosDTO model)
        {
            CheckOwnership(survey_id: model.Id);
            CustomResponse response = new CustomResponse();
            try
            {
               
                response = await _surveyRepository.UpdateSurveyWithoutSections(model);
                
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = "Error al obtener la encuesta";
            }
            return Ok(response);
        }

       

       

       
    }
}
