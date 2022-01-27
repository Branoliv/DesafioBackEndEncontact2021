using System.Collections.Generic;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity contact);
        Task<TEntity> UpdateAsync(TEntity contact);
        Task<TEntity> FindById(int id);
        Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int quantityItemsList);
        Task DeleteAsync(int id);
    }
}
