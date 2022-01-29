using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface ICompanyService : IServiceBase<Company>
    {
        Task<Company> FindAsync(string name);
        Task<int> CountContactBookInCompanys(int contactBookId);
    }
}
