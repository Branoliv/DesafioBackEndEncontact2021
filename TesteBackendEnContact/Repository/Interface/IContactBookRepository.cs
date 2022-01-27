using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactBookRepository
    {
        Task<int> InsertAsync(ContactBook contactBook);
        Task<bool> UpdateAsync(ContactBook contactBook);
        Task DeleteAsync(int id);
        Task<IEnumerable<ContactBook>> GetAllAsync(int pageNumber, int quantityItemsList);
        Task<ContactBook> GetAsync(int id);
    }
}
