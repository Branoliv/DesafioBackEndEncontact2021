using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactBookRepository : IRepositoryBase<ContactBook>
    {
        Task<ContactBook> GetAsync(string contactBookName);
    }
}
