using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        Task<Company> GetAsync(string companyName);
        Task<int> CountContactBookInCompanys(int contactBookId);
    }
}
