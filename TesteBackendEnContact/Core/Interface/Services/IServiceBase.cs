using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity contact);
        Task<TEntity> UpdateAsync(TEntity contact);
        Task<TEntity> FindById(int id);
        Task DeleteAsync(int id);
        Task<Pagination<TEntity>> GetAllPaginationAsync(int pageNumber, int quantityItemsList);
    }
}
