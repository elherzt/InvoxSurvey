using DataLayer.Repositories.OptionRepository;
using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using DataLayer.Repositories.Surveyrepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace InvoxSurveyAPI.Controllers
{
    public class UtilityController : Controller
    {
        protected readonly ISurveyRepository _surveyRepository;
        protected readonly ISectionRepository _sectionRepository;
        protected readonly IQuestionRepository _questionRepository;
        protected readonly IOptionRepository _optionRepository;

        public UtilityController(ISurveyRepository surveyRepository, ISectionRepository sectionRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository)
        {
            _surveyRepository = surveyRepository;
            _sectionRepository = sectionRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
        }


        protected int GetUserID()
        {
            try
            {
                var userIdClaim = User?.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                int userId = Convert.ToInt32(userIdClaim);
                return userId;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("User is not authenticated or NameIdentifier claim is missing.", ex);
            }
        }

        protected string GetUserRole()
        {
            try
            {
                var roleClaim = User?.FindFirst(ClaimTypes.Role)?.Value;
                return roleClaim ?? throw new Exception("User role is not available.");
            }
            catch (Exception ex)
            {
                throw new Exception("User role is not available.", ex);
            }
        }


        /// <summary>
        /// Centralized ownership validation for surveys, sections, questions, and options.
        /// This method will throw UnauthorizedAccessException when ownership constraints are violated.
        /// </summary>
        protected void CheckOwnership(int? survey_id = 0, int? section_id = 0, int? question_id = 0, int? option_id = 0)
        {
            int userId = GetUserID();  //it could be used to check ownership by user or customer or company

            if (survey_id != 0)
            {
                // Check if the user owns the survey - pending implementation, it should be done in the repository
            }
            if (section_id != 0)
            {
                // Check if the user owns the section - pending implementation, it should be done in the repository
            }
            if (question_id != 0)
            {
                // Check if the user owns the question - pending implementation, it should be done in the repository
            }
            if (option_id != 0)
            {
                // Check if the user owns the option - pending implementation, it should be done in the repository
            }

            // If any of the checks fail, throw an UnauthorizedAccessException. it cloud be handled by customer or provider,
            // now it is supposed to be all users and surveys of the same customer or company.

        }




    }
}
