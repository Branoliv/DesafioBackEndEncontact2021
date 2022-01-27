using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
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

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            // TODO 
            var sql = "DELETE FROM ContactBook  WHERE Id = @id";

            await connection.ExecuteAsync(sql.ToString(), new { id });
        }

        public async Task<IEnumerable<ContactBook>> GetAllAsync(int pageNumber, int quantityItemsList)
        {
            //var query = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {2}) AS Row, {0} FROM {3} {4}) AS Paged ", columns, pageSize, orderBy, TableName, where);
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            
            var query = "SELECT * FROM ContactBook LIMIT @quantityItemsList OFFSET @OffSet;";
            var result = await connection.QueryAsync<ContactBook>(query, new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList });

            return result;
        }

        public async Task<ContactBook> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook WHERE Id = @id";
            var result = await connection.QuerySingleOrDefaultAsync<ContactBook>(query, new { id });

            return result;
        }

        
    }
}
