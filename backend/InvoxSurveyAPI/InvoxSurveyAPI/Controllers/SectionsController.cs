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
    public class SectionsController : UtilityController
    {
        public SectionsController(
            ISurveyRepository surveyRepository,
            ISectionRepository sectionRepository,
            IQuestionRepository questionRepository,
            IOptionRepository optionRepository)
            : base(surveyRepository, sectionRepository, questionRepository, optionRepository)
        { }


        public async Task<IActionResult> Add(AddSectionDTO model)
        {
            CustomResponse response = await _sectionRepository.Add(model);
            return Ok(response);
        }

        public async Task<IActionResult> Update(AddSectionDTO model)
        {
            CustomResponse response = await _sectionRepository.Update(model);
            return Ok(response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CustomResponse response = await _sectionRepository.Delete(id);
            return Ok(response);
        }
        
    }
}
