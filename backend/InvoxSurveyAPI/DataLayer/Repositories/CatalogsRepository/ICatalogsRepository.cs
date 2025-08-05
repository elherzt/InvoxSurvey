using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.CatalogsRepository
{
    public interface ICatalogsRepository
    {
        public Task<CustomResponse> GetAllCatalogs();

    }

    public class CatalogsRepository : ICatalogsRepository
    {
        private readonly AppDbContext _context;
        public CatalogsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomResponse> GetAllCatalogs()
        {
            CustomResponse response = new CustomResponse();
            var catalogs = new CatalogsDTO();
            try
            {
                var questionTypesResponse = await GetQuestionTypes();
                var questions = await _context.Questions.ToListAsync();

                if (questionTypesResponse.TypeOfResponse != TypeOfResponse.OK)
                {
                    throw new Exception($"Error fetching categories: {questionTypesResponse.Message}");
                }

                catalogs.QuestionTypes = (List<CatalogItemDTO>)questionTypesResponse.Data;

                response.Data = catalogs;
            }

            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.FailedResponse;
                response.Message = ex.Message;
            }

            return response;

        }


        public async Task<CustomResponse> GetQuestionTypes()
        {
            CustomResponse response = new CustomResponse();
            try
            {
                var questionTypes = await _context.QuestionsTypes.ToListAsync();
                response.Data = questionTypes.Select(x => new CatalogItemDTO
                {
                    Id = x.Id,
                    Name = x.Description
                }).ToList();
                response.TypeOfResponse = TypeOfResponse.OK;
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.FailedResponse;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
