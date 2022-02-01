using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserAuthentication> AddAsync(UserAuthentication entity)
        {
            if (entity == null)
                throw new Exception("Modelo de dados invalido.");

            var userExist = await _authRepository.GetAsync(entity.UserName);

            if (userExist != null)
                throw new Exception("Nome de usuário já existente.");

            var addUserResponse = await _authRepository.InsertAsync(entity);

            if (addUserResponse <= 0)
                return null;

            return await _authRepository.GetAsync(addUserResponse);
        }

        public async Task<UserAuthentication> Auth(UserAuthentication user)
        {
            return await _authRepository.AuthAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userExist = await _authRepository.GetAsync(id);

            if (userExist == null)
                return false;

            return await _authRepository.DeleteAsync(id);
        }

        public async Task<UserAuthentication> FindById(int id)
        {
            return await _authRepository.GetAsync(id);
        }

        public async Task<Pagination<UserAuthentication>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
        {
            return await _authRepository.GetAllPaginatedAsync(pageNumber, quantityItemsList);
        }

        public async Task<UserAuthentication> UpdateAsync(UserAuthentication entity)
        {
            var userExist = await _authRepository.GetAsync(entity.Id);

            if (userExist == null)
                throw new Exception("Usuário não existe");

            var updateResult = await _authRepository.UpdateAsync(entity);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            return await _authRepository.GetAsync(entity.Id);
        }
    }
}
