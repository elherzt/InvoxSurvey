using DataLayer.Models;
using DataLayer.Repositories.QuestionsRepository;
using DataLayer.Repositories.SectionRepository;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Surveyrepository
{
    public class SurveyDTO
    {
        public SurveyDTO()
        {
        }

        public SurveyDTO(Survey model, bool full = false)
        {
            if (full)
            {
                ParseFull(model);
            }
            else
            {
                Parse(model);
            }

        }


        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Place { get; set; }
        public int Target { get; set; }

        public int UserId { get; set; }
        public int StatusId { get; set; }

        public string Instructions { get; set; } = string.Empty;

        public virtual ICollection<SectionDTO> Sections { get; set; } = new List<SectionDTO>();

        private void ParseFull(Survey model)
        {
            try
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.Description = model.Description;
                this.Instructions = model.Instructions;
                this.CreatedDate = model.CreatedDate;
                this.Place = model.Place;
                this.UserId = model.UserId;
                this.Target = model.Target;
                this.StatusId = model.StatusId;
                this.Sections = model.Sections.Select(x => new SectionDTO(x, true)).ToList();
            }
            catch (Exception ex)
            {

            }
        }

        private void Parse(Survey model)
        {
            try
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.Description = model.Description;
                this.Instructions = model.Instructions;
                this.CreatedDate = model.CreatedDate;
                this.Place = model.Place;
                this.Target = model.Target;
                this.UserId = model.UserId;
            }
            catch (Exception ex)
            {
            }
        }

        public class SurveyCreateDTO
        {
            public int id { get; set; }
            [Required]
            [StringLength(255)]
            public string Name { get; set; }

            [Required]
            [StringLength(255)]
            public string Description { get; set; }

            [Required]
            public int PlaceId { get; set; } = 0;

            [Required]
            [StringLength(255)]
            public string Place { get; set; }

            [Required]
            public int Target { get; set; }
            [Required]
            [StringLength(255)]
            public string Instructions { get; set; } = string.Empty;
            [Required]
            public int UserId { get; set; }
            [Required]
            public DateTime CreatedDate { get; set; }
            [Required]
            public ICollection<Section> Sections { get; set; } = new List<Section>();
        }
    }

    public class SurveyOpenAnswerDTO
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
    }

    public class SurveyQueryDTO // DTO for querying surveys
    {
        public int? Id { get; set; }
        public int? UserId { get; set; } 
        public DateTime? CreatedDateStart { get; set; } //not implemented yet
        public DateTime? CreatedDateEnd { get; set; } //not implemented yet
        public SurveyStatus? Status { get; set; }
    }

    public class SurveyAnswerDTO
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Answers { get; set; } = 0;
        public string? Audio { get; set; }
        public int? Target { get; set; } = 0;
        public int PlaceId { get; set; } = 0;
        public string? Place { get; set; }
        public int? UserId { get; set; } = 0;
        public string? UserName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = DateTime.Now;

        public string? Filters { get; set; } = string.Empty;
        public List<QuestionAnswerDTO>? Questions { get; set; } = new List<QuestionAnswerDTO>();
        public List<SectionAnswerDTO>? Sections { get; set; } = new List<SectionAnswerDTO>();

        public SurveyAnswerDTO()
        {
        }

        public SurveyAnswerDTO(Survey model)
        {
            try
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.Description = model.Description;
                this.Target = model.Target;
                this.Sections = model.Sections.Select(x => new SectionAnswerDTO(x)).ToList();

            }
            catch (Exception ex)
            {
                // to do save log
            }
        }

    }

    public class SurveyAnswerFilterDTO
    {
        public int Id { get; set; }
        public List<int> Filters { get; set; } = new List<int>();
    }

    public class SurveyWithoutSectiosDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Place { get; set; }
        public int Target { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public SurveyWithoutSectiosDTO()
        {
        }
        public SurveyWithoutSectiosDTO(Survey model)
        {
            try
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.Description = model.Description;
                this.Instructions = model.Instructions;
                this.CreatedDate = model.CreatedDate;
                this.Place = model.Place;
                this.Target = model.Target;
                this.UserId = model.UserId;
            }
            catch (Exception ex)
            {
            }
        }

    }
}
