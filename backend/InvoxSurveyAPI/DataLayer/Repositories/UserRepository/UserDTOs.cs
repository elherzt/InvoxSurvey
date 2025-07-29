using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.UserRepository
{
   

   public class UserLoginDTO
   {
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Email must be valid")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }
    }

    public class UserDTO
    {

        public UserDTO()
        {
           
        }

        public UserDTO(User model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Email = model.Email;
            this.CreatedDate = model.CreatedDate;
            this.IsActive = model.IsActive;
            this.RoleId = model.RoleId;
            this.RoleName = model.Role?.Name ?? string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
    }


}
