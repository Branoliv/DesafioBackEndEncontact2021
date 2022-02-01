using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public AuthRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<UserAuthentication> AuthAsync(UserAuthentication user)
        {
            var param = new { userName = user.UserName, password = user.Password };
            var query = @"SELECT * FROM UserAuth WHERE UserName = @userName AND Password = @password;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryAsync<UserAuthentication>(query, param);

            return result.FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var param = new { id };
            var sql = "DELETE FROM UserAuth WHERE Id = @id";
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.ExecuteAsync(sql, param);

            if (result <= 0)
                return false;
            else
                return true;
        }

        public async Task<Pagination<UserAuthentication>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
        {
            throw new NotImplementedException();
        }

        public async Task<UserAuthentication> GetAsync(int id)
        {
            var param = new { id };
            var query = @"SELECT * FROM UserAuth WHERE Id = @id;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

           return await connection.QueryFirstOrDefaultAsync<UserAuthentication>(query, param);
        }

        public async Task<UserAuthentication> GetAsync(string userName)
        {
            var param = new { userName };
            var query = @"SELECT * FROM UserAuth WHERE UserName = @userName;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryFirstOrDefaultAsync<UserAuthentication>(query, param);
            return result;
        }

        public async Task<int> InsertAsync(UserAuthentication entity)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.InsertAsync(entity);
        }

        public Task<bool> UpdateAsync(UserAuthentication entity)
        {
            throw new NotImplementedException();
        }
    }
}
