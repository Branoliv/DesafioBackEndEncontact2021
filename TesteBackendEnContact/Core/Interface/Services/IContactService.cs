using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IContactService : IServiceBase<Contact>
    {
        Task<int> CountCompanyInContacts(int companyId);
        Task<int> CountContactBookInContacts(int contactBookId);
        Task<Pagination<Contact>> GetAsync(string param, int pageNumber, int quantityItemsList);
        Task<Pagination<Contact>> GetAllByCompanyIdPaginatedAsync(int companyId, int pageNumber, int quantityItemsList);
        Task<Pagination<Contact>> GetAllByContactBookIdPaginatedAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
