using DataLayer.Repositories.OptionRepository;
using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using DataLayer.Repositories.Surveyrepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace InvoxSurveyAPI.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : UtilityController
    {

        public QuestionsController(
          ISurveyRepository surveyRepository,
          ISectionRepository sectionRepository,
          IQuestionRepository questionRepository,
          IOptionRepository optionRepository)
          : base(surveyRepository, sectionRepository, questionRepository, optionRepository)
        { }

        [HttpPost]
        public async Task<IActionResult> Add(AddUpdateQuestionDTO model)
        {
            CheckOwnership(question_id: model.Id);
            CustomResponse response = await _questionRepository.Add(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AddUpdateQuestionDTO model)
        {
            CheckOwnership(question_id: model.Id);
            CustomResponse response = await _questionRepository.Update(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CheckOwnership(question_id: id);
            CustomResponse res = await _questionRepository.Delete(id);
            return Ok(res);
        }

        
    }
}
