using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<int> InsertAsync(Contact contactBook);
        Task<bool> UpdateAsync(Contact contactBook);
        Task DeleteAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int quantityItemsList);
        Task<Contact> GetAsync(int id);
        Task<int> CountCompanyInContacts(int companyId);
        Task<int> CountContactBookInContacts(int contactBookId);
        Task<IEnumerable<Contact>> GetAllByCompanyIdAsync(int companyId, int pageNumber, int quantityItemsList);
        Task<IEnumerable<Contact>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
