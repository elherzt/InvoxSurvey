using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<CustomResponse> Login(UserLoginDTO userLogin);
    }

    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserRepository(AppDbContext context, IPasswordHasherService hasher)
        {
            _context = context;
            _passwordHasherService = hasher;
        }

        public async Task<CustomResponse> Login(UserLoginDTO userLogin)
        {
            CustomResponse response = new CustomResponse(TypeOfResponse.OK, "Login successfully");
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role) 
                    .Where(u => u.Email == userLogin.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "User not found.";
                    return response;
                }

                if (!user.IsActive)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "User is inactive. Please contact the adminstrator.";
                    return response;
                }

                if (!_passwordHasherService.VerifyPassword(user.Password, userLogin.Password))
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid password.";
                    return response;
                }

                response.Data = new UserDTO(user);

            }
            catch(Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = "An error occurred during login: " + ex.Message;
            }
            return response;
        }
    }
}
