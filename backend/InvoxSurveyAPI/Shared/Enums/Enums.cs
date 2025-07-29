using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
   
    public enum QuestionType
    {
        [Display(Name = "Multiple Choice")]
        MultipleChoice = 1,

        [Display(Name = "Single Choice")]
        SingleChoice = 2,

        [Display(Name = "Open Ended")]
        OpenEnded = 3
    }
    public enum SurveyStatus
    {
        Draft = 1,
        Published = 2,
        Finished = 3,
        Archived = 4
    }
    public enum UserRole
    {
        Admin = 1,
        Interviewer = 2
    }



    //parse the enum values to strings for display purposes
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field.GetCustomAttribute<DisplayAttribute>();

            return attr?.Name ?? value.ToString();
        }
    }


}
