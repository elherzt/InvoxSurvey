using DataLayer.Models;
using DataLayer.Repositories.OptionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.QuestionsRepository
{
    public class QuestionDTO
    {
        public QuestionDTO()
        {
        }

        public QuestionDTO(Question model, bool full = false)
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
        public string Description { get; set; }
        public int? SectionId { get; set; }
        public int? TypeId { get; set; }

        public bool? Other { get; set; }

        public virtual List<OptionDTO> Options { get; set; } = new List<OptionDTO>();

        private void ParseFull(Question model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
                this.SectionId = model.SectionId;
                this.Other = model.HasOther;
                this.TypeId = model.TypeId;
                this.Options = model.Options.Select(o => new OptionDTO(o, true)).ToList();
            }
            catch (Exception ex)
            {
            }
        }

        private void Parse(Question model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class QuestionAnswerDTO
    {
        public QuestionAnswerDTO()
        {
        }

        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;
        public List<QuestionAnswerOptionDTO> Answers { get; set; } = new List<QuestionAnswerOptionDTO>();

        public QuestionAnswerDTO(Question model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
                this.Answers = new List<QuestionAnswerOptionDTO>();
            }
            catch (Exception ex)
            {
            }

        }



    }

    public class QuestionAnswerOptionDTO
    {

        public int Option_Id { get; set; }
        public string? Text { get; set; }
        public string? Audio { get; set; }
        public int? Count { get; set; }

        public bool? IsOpen { get; set; }
    }

    public class QuestionTypesDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class AddUpdateQuestionDTO
    {
        public AddUpdateQuestionDTO()
        {
        }

        public int? Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int TypeId { get; set; }
        public bool? Other { get; set; }
    }
}
