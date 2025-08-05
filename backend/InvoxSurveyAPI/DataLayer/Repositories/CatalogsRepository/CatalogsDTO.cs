using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.CatalogsRepository
{
    public class CatalogsDTO
    {
        public List<CatalogItemDTO> QuestionTypes { get; set; }
        public List<CatalogItemDTO> Places { get; set; }
    }

    public class CatalogItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
