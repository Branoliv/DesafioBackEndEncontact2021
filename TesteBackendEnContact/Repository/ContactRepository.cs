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
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;


        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }


        public async Task<int> CountCompanyInContacts(int companyId)
        {
            var query = "SELECT COUNT(*) FROM Contact WHERE CompanyId = @companyId";
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            return await connection.QueryFirstAsync<int>(query.ToString(), new { companyId });
        }

        public async Task<int> CountContactBookInContacts(int contactBookId)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT COUNT(*) FROM Contact WHERE ContactBookId = @contactBookId";

            var result = await connection.QueryFirstAsync<int>(query.ToString(), new { contactBookId });

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var sql = "DELETE FROM Contact WHERE ID = @id";

            await connection.ExecuteAsync(sql.ToString(), new { id });
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int quantityItemsList)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Contact LIMIT @quantityItemsList OFFSET @OffSet;";
            var result = await connection.QueryAsync<Contact>(query, new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList });

            return result;
        }

        public async Task<Contact> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM Contact WHERE Id = @id";
            var result = await connection.QuerySingleOrDefaultAsync<Contact>(query, new { id });

            return result;
        }

        public async Task<int> InsertAsync(Contact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.InsertAsync(contact);
        }

        public async Task<bool> UpdateAsync(Contact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.UpdateAsync(contact);
        }
        
        public async Task<IEnumerable<Contact>> GetAllByCompanyIdAsync(int companyId, int pageNumber, int quantityItemsList)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Contact WHERE companyId = @companyId LIMIT @quantityItemsList OFFSET @OffSet;";
            var result = await connection.QueryAsync<Contact>(query, new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList, companyId });

            return result;
        }

        public async Task<IEnumerable<Contact>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Contact WHERE ContactBookId = @contactBookId LIMIT @quantityItemsList OFFSET @OffSet;";
            var result = await connection.QueryAsync<Contact>(query, new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList, contactBookId });

            return result;
        }
    }
}
