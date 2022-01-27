using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IContactService : IServiceBase<Contact>
    {
        Task<int> CountCompanyInContacts(int companyId);
        Task<int> CountContactBookInContacts(int contactBookId);
        Task<IEnumerable<Contact>> GetAllByCompanyIdAsync(int companyId, int pageNumber, int quantityItemsList);
        Task<IEnumerable<Contact>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList);
    }
}
