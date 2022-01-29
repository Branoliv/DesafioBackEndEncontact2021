using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactBookRepository
    {
        Task<int> InsertAsync(ContactBook contactBook);
        Task<bool> UpdateAsync(ContactBook contactBook);
        Task DeleteAsync(int id);
        Task<ContactBook> GetAsync(int id);
        Task<ContactBook> GetAsync(string contactBookName);
        Task<Pagination<ContactBook>> GetAllPaginationAsync(int pageNumber, int quantityItemsList);
    }
}
