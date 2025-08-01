using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.PlacesRepository
{
    public class PlaceDTO
    {
        public PlaceDTO()
        {
        }

        public PlaceDTO(Place model, bool isFull = false)
        {
            if (isFull)
            {
                ParseFull(model);
            }
            else
            {
                Parse(model);
            }
        }
        private void ParseFull(Place model)
        {
            try
            {

                Id = model.Id;
                Description = model.Description;
            }
            catch (Exception ex)
            {
            }
        }
        private void Parse(Place model)
        {
            try
            {
                Id = model.Id;
                Description = model.Description;
            }
            catch (Exception ex)
            {
            }
        }

        public int Id { get; set; }
        public string Description { get; set; } = "";

    }
}
