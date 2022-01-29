using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class PaginationDTO<TEntity> where TEntity : class
    {
        public PaginationDTO(double countRows, double numberOfPages, IEnumerable<TEntity> listResult)
        {
            CountRows = countRows;
            NumberOfPages = numberOfPages;
            ListResult = listResult;
        }

        public double CountRows { get; set; }
        public double NumberOfPages { get; set; }
        public IEnumerable<TEntity> ListResult { get; set; }
    }
}
