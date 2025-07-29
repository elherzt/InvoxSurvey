using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CommonRepository
{
    public static class DbSeeder
    {
        

        public static async Task SeedDataAsync(AppDbContext dbContext, IPasswordHasherService hasher)
        {
            bool changes = false;
            // Seed initial data for places
            if (!await dbContext.Places.AnyAsync())
            {
                
                await dbContext.Places.AddRangeAsync(
                    new Place { Description = "Main office" },
                    new Place { Description = "Parking Lot" }
                );
                changes = true;
            }


            // Seed data for QuestionsType
            if (!await dbContext.QuestionsTypes.AnyAsync())
            {
                await dbContext.QuestionsTypes.AddRangeAsync(
                    new QuestionsType { Id = (int)QuestionType.MultipleChoice, Description = QuestionType.MultipleChoice.GetDisplayName() },
                    new QuestionsType { Id = (int)QuestionType.SingleChoice, Description = QuestionType.SingleChoice.GetDisplayName() },
                    new QuestionsType { Id = (int)QuestionType.OpenEnded, Description = QuestionType.OpenEnded.GetDisplayName() }
                );
                changes = true;
            }

            // Seed data for SurveysStatus
            if (!await dbContext.SurveysStatuses.AnyAsync())
            {
                await dbContext.SurveysStatuses.AddRangeAsync(
                    new SurveysStatus { Id = (int)SurveyStatus.Draft, Name = SurveyStatus.Draft.ToString() },
                    new SurveysStatus { Id = (int)SurveyStatus.Published, Name = SurveyStatus.Published.ToString() },
                    new SurveysStatus { Id = (int)SurveyStatus.Finished, Name = SurveyStatus.Finished.ToString() },
                    new SurveysStatus { Id = (int)SurveyStatus.Archived, Name = SurveyStatus.Archived.ToString() }
                );
                changes = true;
            }

            // Seed data for Roles
            if (!await dbContext.Roles.AnyAsync())
            {
                await dbContext.Roles.AddRangeAsync(
                    new Role { Id = (int)UserRole.Admin, Name = UserRole.Admin.ToString() },
                    new Role { Id = (int)UserRole.Interviewer, Name = UserRole.Interviewer.ToString() }
                );
                changes = true;
            }

            //sedd data for Users

            var adminPwd = Environment.GetEnvironmentVariable("USER_ADMIN_PWD") ?? "admin123!";
            var interviewerPwd = Environment.GetEnvironmentVariable("USER_INTERVIEWER_PWS") ?? "interview123!";

            string hashedAdminPassword = hasher.HashPassword(adminPwd);
            string hashedInterviewerPassword = hasher.HashPassword(interviewerPwd);

            if (!await dbContext.Users.AnyAsync())
            {
                await dbContext.Users.AddRangeAsync(
                    new User { Name = "Admin User", Email = "admin@invoxsurvey.com", Password = hashedAdminPassword, RoleId = (int)UserRole.Admin, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new User { Name = "Interviewer User", Email = "interviewer@invoxsurvey.com", Password = hashedInterviewerPassword, RoleId = (int)UserRole.Interviewer, IsActive = true, CreatedDate = DateTime.UtcNow }
                );
                changes = true;
            }


            // Save changes if any data was added
            if (changes)
            {
                await dbContext.SaveChangesAsync(); //aplply changes to the database
            }
            



        }
    }
}
