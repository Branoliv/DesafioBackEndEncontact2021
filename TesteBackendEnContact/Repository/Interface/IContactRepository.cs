using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<int> CountCompanyInContacts(int companyId);
        Task<int> CountContactBookInContacts(int contactBookId);
        Task<int> InsertAsync(Contact contactBook);
        Task<bool> UpdateAsync(Contact contactBook);
        Task DeleteAsync(int id);
        Task<Contact> GetAsync(int id);
        Task<Pagination<Contact>> GetAsync(string param, int pageNumber, int quantityItemsList);
        Task<Pagination<Contact>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList);
        Task<Pagination<Contact>> GetAllByCompanyIdPaginatedAsync(int companyId, int pageNumber, int quantityItemsList);
        Task<Pagination<Contact>> GetAllByContactBookIdPaginatedAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
