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
        Task<IEnumerable<Company>> GetAllAsync(int pageNumber, int quantityItemsList);
        Task<Company> GetAsync(int id);
        Task<int> CountContactBookInCompanys(int contactBookId);
        Task<IEnumerable<Company>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
