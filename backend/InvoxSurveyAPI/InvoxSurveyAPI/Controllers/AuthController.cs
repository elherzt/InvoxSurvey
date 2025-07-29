using DataLayer.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Enums;
using Shared.Security;
using System.Runtime.CompilerServices;

namespace InvoxSurveyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IJWTGenerator _jwtGenerator;

        public AuthController(IUserRepository userRepository, IJWTGenerator jWTGenerator)
        {
            _userRepository = userRepository;
            _jwtGenerator = jWTGenerator;
        }

        [HttpPost("Login")]
        public async Task<CustomResponse> Login(UserLoginDTO user)
        {
            CustomResponse response = new CustomResponse();
            try
            {
                CustomResponse userResponse = await _userRepository.Login(user);
                if (userResponse.TypeOfResponse == TypeOfResponse.OK)
                {
                    UserDTO loggedUser = (UserDTO)userResponse.Data;
                    BasicSessionModel sessionModel = new BasicSessionModel
                    {
                        UserId = loggedUser.Id,
                        Name = loggedUser.Name,
                        Email = loggedUser.Email,
                        Provider = "InvoxSurvey" // Not implemented in this version, but can be used for future enhancements
                    };

                    string token = "";
                    if (loggedUser.RoleId == (int)UserRole.Admin)
                    {
                        token = _jwtGenerator.GenerateTokenAdmin(sessionModel);
                    }
                    else if (loggedUser.RoleId == (int)UserRole.Interviewer)
                    {
                        token = _jwtGenerator.GenerateTokenInterviewer(sessionModel);
                    }
                    else
                    {
                        response.TypeOfResponse = TypeOfResponse.FailedResponse;
                        response.Message = "Invalid user role.";
                        return response;
                    }

                    loggedUser.JwtToken= token;
                    response.Data = loggedUser;

                }
                else
                {
                    response = userResponse;
                }
                
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.FailedResponse;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
    }
}
