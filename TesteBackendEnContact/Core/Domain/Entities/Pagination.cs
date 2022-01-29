using System.Collections.Generic;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    public class Pagination<TEntity> where TEntity : class
    {
        public Pagination(double countRows, double numberOfPages, IEnumerable<TEntity> listResult)
        {
            CountRows = countRows;
            NumberOfPages = numberOfPages;
            ListResult = listResult;
        }

        public double CountRows { get; private set; }
        public double NumberOfPages { get; private set; }
        public IEnumerable<TEntity> ListResult { get; private set; }
    }
}
