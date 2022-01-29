using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DatabaseConfig databaseConfig;


        public CompanyRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }


        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var state = connection.State;

            if (ConnectionState.Closed.Equals(state))
                connection.Open();

            using var transaction = connection.BeginTransaction();

            var sql = new StringBuilder();
            sql.AppendLine("DELETE FROM Company WHERE Id = @id;");
            sql.AppendLine("UPDATE Contact SET CompanyId = null WHERE CompanyId = @id;");

            var result = await connection.ExecuteAsync(sql.ToString(), new { id }, transaction);

            var x = transaction.IsolationLevel;
            transaction.Commit();

        }

        public async Task<Company> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM Company where Id = @id";
            var result = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });

            return result;
        }

        public async Task<Company> GetAsync(string companyName)
        {
            var query = "SELECT * FROM Company WHERE LOWER(Name) LIKE @companyName";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryAsync<Company>(query, new { companyName });

            return result.FirstOrDefault();
        }

        public async Task<int> InsertAsync(Company company)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.InsertAsync(company);
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            return await connection.UpdateAsync(company);
        }

        public async Task<int> CountContactBookInCompanys(int contactBookId)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT COUNT(*) FROM Company WHERE ContactBookId = @contactBookId";

            var result = await connection.QueryFirstAsync<int>(query.ToString(), new { contactBookId });

            return result;
        }

        public async Task<Pagination<Company>> GetAllPaginationAsync(int pageNumber, int quantityItemsList)
        {
            var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList };

            var query = @"SELECT * FROM Company LIMIT @quantityItemsList OFFSET @OffSet;";

            query += @"SELECT COUNT(1) FROM Company";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            connection.Open();

            using var result = await connection.QueryMultipleAsync(query, param).ConfigureAwait(false);

            var companies = result.Read<Company>().ToList();
            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList); ;


            return new Pagination<Company>(totalRows, totalPages, companies);
        }
    }
}
