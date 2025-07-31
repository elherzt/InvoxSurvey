using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.SectionRepository
{
    public interface ISectionRepository
    {
        Task<CustomResponse> GetAll();
    }

    internal class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;

        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse> GetAll()
        {
            CustomResponse res = new CustomResponse(TypeOfResponse.OK, "Sections Retrieved Successfully");
            try
            {
                var sections = await _context.Sections.Select(s => new SectionDTO(s, true)).ToListAsync();
                if (sections.Count == 0)
                {
                    res = new CustomResponse(TypeOfResponse.FailedResponse, "No Sections Found");
                }
                else
                {
                    res.Data = sections;
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
