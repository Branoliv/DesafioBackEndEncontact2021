using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> FindById(int id);
        Task<bool> DeleteAsync(int id);
        Task<Pagination<TEntity>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList);
    }
}
