using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface ICompanyRepository
    {
        Task<int> InsertAsync(Company company);
        Task<bool> UpdateAsync(Company company);
        Task DeleteAsync(int id);
        Task<Company> GetAsync(int id);
        Task<Company> GetAsync(string companyName);
        Task<int> CountContactBookInCompanys(int contactBookId);
        Task<Pagination<Company>> GetAllPaginationAsync(int pageNumber, int quantityItemsList);
    }
}
