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
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly DatabaseConfig databaseConfig;


        public ContactBookRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }


        public async Task<int> InsertAsync(ContactBook contactBook)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.InsertAsync(contactBook);
        }

        public async Task<bool> UpdateAsync(ContactBook contactBook)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.UpdateAsync(contactBook);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM ContactBook  WHERE Id = @id";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.ExecuteAsync(sql.ToString(), new { id });

            if (result <= 0)
                return false;
            else
                return true;
        }

        public async Task<IEnumerable<ContactBook>> GetAllAsync(int pageNumber, int quantityItemsList)
        {
            var query = "SELECT * FROM ContactBook LIMIT @quantityItemsList OFFSET @OffSet;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryAsync<ContactBook>(query, new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList });

            return result;
        }

        public async Task<ContactBook> GetAsync(int id)
        {
            var query = "SELECT * FROM ContactBook WHERE Id = @id";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QuerySingleOrDefaultAsync<ContactBook>(query, new { id });

            return result;
        }

        public async Task<ContactBook> GetAsync(string contactBookName)
        {
            var param = new { contactBookName };

            var query = @"SELECT * FROM ContactBook WHERE LOWER(Name) LIKE LOWER(@contactBookName)";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryAsync<ContactBook>(query, param);

            return result.FirstOrDefault();
        }

        public async Task<Pagination<ContactBook>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
        {
            var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList };

            var query = @"SELECT * FROM ContactBook LIMIT @quantityItemsList OFFSET @OffSet;";

            query += @"SELECT COUNT(1) FROM ContactBook";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            connection.Open();

            using var result = await connection.QueryMultipleAsync(query, param).ConfigureAwait(false);

            var contactBooks = result.Read<ContactBook>().ToList();
            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList); ;


            return new Pagination<ContactBook>(totalRows, totalPages, contactBooks);
        }
    }
}
