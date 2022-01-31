using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<int> InsertAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<TEntity> GetAsync(int id);
        Task<Pagination<TEntity>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList);
    }
}
