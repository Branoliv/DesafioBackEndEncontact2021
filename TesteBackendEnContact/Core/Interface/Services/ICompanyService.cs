using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface ICompanyService : IServiceBase<Company>
    {
        Task<int> CountContactBookInCompanys(int contactBookId);
        Task<IEnumerable<Company>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
