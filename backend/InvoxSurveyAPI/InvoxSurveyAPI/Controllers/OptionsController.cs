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
    public class OptionsController : UtilityController
    {

        public OptionsController(
           ISurveyRepository surveyRepository,
           ISectionRepository sectionRepository,
           IQuestionRepository questionRepository,
           IOptionRepository optionRepository)
           : base(surveyRepository, sectionRepository, questionRepository, optionRepository)
        { }


        public async Task<IActionResult> Add(OptionDTO model)
        {
            CustomResponse res = await _optionRepository.Add(model);
            return Ok(res);
        }

        public async Task<IActionResult> Update(OptionDTO model)
        {
            CheckOwnership(question_id: model.question_id, option_id: model.Id);
            CustomResponse response = await _optionRepository.Update(model);
            return Ok(response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CheckOwnership(option_id: id);
            CustomResponse res = await _optionRepository.Delete(id);
            return Ok(res);
        }
    }
}
