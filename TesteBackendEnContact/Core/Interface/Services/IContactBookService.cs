using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IContactBookService : IServiceBase<ContactBook>
    {
        Task<ContactBook> FindAsync(string name);
    }
}
