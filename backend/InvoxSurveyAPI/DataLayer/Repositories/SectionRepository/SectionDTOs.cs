using DataLayer.Models;
using DataLayer.Repositories.QuestionsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.SectionRepository
{
    public class SectionDTO
    {
        public SectionDTO()
        {
        }

        public SectionDTO(Section model, bool full = false)
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
        public int SurveyId { get; set; }
        public virtual ICollection<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();

        private void ParseFull(Section model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
                this.SurveyId = model.SurveyId;
                this.Questions = model.Questions.Select(x => new QuestionDTO(x, true)).ToList();
            }
            catch (Exception ex)
            {

            }
        }

        private void Parse(Section model)
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

    public class AddSectionDTO
    {
        public AddSectionDTO()
        {
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public int SurveyId { get; set; }
    }


    public class SectionAnswerDTO

    {
        public SectionAnswerDTO() { }
        public int Id { get; set; }
        public string Description { get; set; }
        public List<QuestionAnswerDTO> Questions { get; set; } = new List<QuestionAnswerDTO>();

        public SectionAnswerDTO(Section model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
                this.Questions = model.Questions.Select(x => new QuestionAnswerDTO(x)).ToList();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
