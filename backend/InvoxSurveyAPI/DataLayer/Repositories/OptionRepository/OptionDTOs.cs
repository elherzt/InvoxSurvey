using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.OptionRepository
{
    public class OptionDTO
    {
        public OptionDTO()
        {
        }
        public OptionDTO(Option Model, bool isFull = false)
        {
            if (isFull)
            {
                ParseFull(Model);
            }
            else
            {
                Parse(Model);
            }
        }

        private void ParseFull(Option model)
        {
            try
            {
                this.Id = model.Id;
                this.Description = model.Description;
                this.question_id = model.QuestionId;
                this.IsOpen = model.IsOpen;
                this.SkipSection = model.SkipSection;
                this.NextQuestionId = model.NextQuestionId ?? 0;

            }
            catch (Exception ex)
            {
            }
        }

        private void Parse(Option model)
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

        public int Id { get; set; }
        public int question_id { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }

        public bool SkipSection { get; set; }

        public int? NextQuestionId { get; set; }


        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    }
}
