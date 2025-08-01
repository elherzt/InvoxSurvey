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
    public class SurveysController : Controller
    {

        private readonly ISurveyRepository _surveyRepository;

        public SurveysController(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            CustomResponse response = await _surveyRepository.GetAll();
            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStatus(SurveyQueryDTO query)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.FailedResponse, "Status not found");

            switch (query.Status)
            {
                case SurveyStatus.Draft:
                    response = await _surveyRepository.GetCreated();
                    break;
                case SurveyStatus.Published:
                    response = await _surveyRepository.GetActive();
                    break;
                case SurveyStatus.Finished:
                    response = await _surveyRepository.GetFinished();
                    break;
                case SurveyStatus.Archived:
                    response = await _surveyRepository.GetArchived();
                    break;  
                default:
                    break;
            }   

            CustomResponse res = await _surveyRepository.GetActive();
            return Ok(res);
        }

        

        [HttpGet]
        public async Task<IActionResult> Results(int id)
        {
            CustomResponse response = await _surveyRepository.GetResults(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResultsByFilters(SurveyAnswerFilterDTO survey_filters)
        {
            CustomResponse response = await _surveyRepository.GetResultsByFilters(survey_filters.Id, survey_filters.Filters);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ShowOpenAnswer(SurveyOpenAnswerDTO model)
        {
            CustomResponse response = await _surveyRepository.GetOpenAnswer(model.SurveyId, model.QuestionId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Answer(int id)
        {
            CustomResponse response = await _surveyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
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
            CustomResponse response = await _surveyRepository.GetById(id);
            return Ok(response);
        }

        

        public async Task<IActionResult> Update(SurveyCreateDTO model)
        {
            CustomResponse response = await _surveyRepository.Update(model);
            return Ok(response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CustomResponse response = await _surveyRepository.Delete(id);
            return Ok(response);
        }

        public async Task<IActionResult> PublicarEncuesta(int id)
        {
            CustomResponse response = await _surveyRepository.Publicar(id);
            return Ok(response);
        }

        public async Task<IActionResult> UpdateSurveyWithoutSections(SurveyWithoutSectiosDTO model)
        {

            CustomResponse response = new CustomResponse();
            try
            {
                var userIdClaim = User?.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var userId = userIdClaim;
                model.UserId = Convert.ToInt32(userId);
                response = await _surveyRepository.UpdateSurveyWithoutSections(model);
                
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = "Error al obtener la encuesta";
            }
            return Ok(response);
        }

       

        public async Task<IActionResult> DeleteQuestion(int id)
        {
            CustomResponse res = await _surveyRepository.DeleteQuestion(id);
            return Ok(res);
        }


        public async Task<IActionResult> SaveQuestion(UpdateQuestionDTO model)
        {
            CustomResponse response = await _surveyRepository.AddNewQuestion(model);
            return Ok(response);
        }

        public async Task<IActionResult> SaveOption(OptionDTO model)
        {
            CustomResponse res = await _surveyRepository.AddOptions(model);
            return Ok(res);
        }

        public async Task<IActionResult> UpdateOption(OptionDTO model)
        {
            CustomResponse response = await _surveyRepository.UpdateOption(model);
            return Ok(response);
        }

        public async Task<IActionResult> DeleteOption(int id)
        {
            CustomResponse res = await _surveyRepository.DeleteOption(id);
            return Ok(res);
        }
    }
}
