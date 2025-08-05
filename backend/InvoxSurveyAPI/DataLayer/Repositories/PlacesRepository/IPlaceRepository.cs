using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.PlacesRepository
{
    public interface IPlaceRepository
    {
        Task<CustomResponse> GetAll();
        Task<CustomResponse> GetById(int id);

    }

    public class PlaceRepository : IPlaceRepository
    {
        private readonly AppDbContext _context;
        public PlaceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse> GetAll()
        {
            CustomResponse res = new CustomResponse(TypeOfResponse.OK, "location found");
            try
            {
                var places = await _context.Places.Select(p => new PlaceDTO(p, true)).ToListAsync();
                if (places.Count == 0)
                {
                    res = new CustomResponse(TypeOfResponse.FailedResponse, "Locations not found");
                }
                else
                {
                    res.Data = places;
                }
            }
            catch (Exception ex)
            {
                res = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return res;
        }

        public async Task<CustomResponse> GetById(int id)
        {
            CustomResponse res = new CustomResponse(TypeOfResponse.OK, "Location retrieved successfully");
            try
            {
                var place = await _context.Places.Where(p => p.Id == id).Select(p => new PlaceDTO(p, false)).FirstOrDefaultAsync();
                if (place == null)
                {
                    res = new CustomResponse(TypeOfResponse.FailedResponse, "Location not found");
                }
                else
                {
                    res.Data = place;
                }
            }
            catch (Exception ex)
            {
                res = new CustomResponse(TypeOfResponse.Exception, ex.Message);
            }
            return res;
        }
    }
}
