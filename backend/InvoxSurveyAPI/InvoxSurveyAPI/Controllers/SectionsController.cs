using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace InvoxSurveyAPI.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : Controller
    {
        public async Task<IActionResult> Add(AddSectionDTO model)
        {
            CustomResponse response = await _surveyRepository.AddSection(model);
            return Ok(response);
        }

        public async Task<IActionResult> Update(UpdateQuestionDTO model)
        {
            CustomResponse response = await _surveyRepository.UpdateQuestion(model);
            return Ok(response);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CustomResponse response = await _surveyRepository.DeleteSection(id);
            return Ok(response);
        }
        
    }
}
