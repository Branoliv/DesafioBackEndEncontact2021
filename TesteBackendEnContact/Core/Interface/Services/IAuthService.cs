using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface IAuthService : IServiceBase<UserAuthentication>
    {
        Task<UserAuthentication> Auth(UserAuthentication user);   
    }
}
