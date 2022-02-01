using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IAuthRepository : IRepositoryBase<UserAuthentication>
    {
        Task<UserAuthentication> GetAsync(string userName);
        Task<UserAuthentication> AuthAsync(UserAuthentication user);
    }
}
