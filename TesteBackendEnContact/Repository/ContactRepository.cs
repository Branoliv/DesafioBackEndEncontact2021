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
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;


        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }


        public async Task<int> CountCompanyInContacts(int companyId)
        {
            var param = new { companyId };
            var query = "SELECT COUNT(1) FROM Contact WHERE CompanyId = @companyId";
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            return await connection.QueryFirstAsync<int>(query, param);
        }

        public async Task<int> CountContactBookInContacts(int contactBookId)
        {
            var param = new { contactBookId };
            var query = "SELECT COUNT(1) FROM Contact AS c WHERE c.ContactBookId = @contactBookId";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryFirstAsync<int>(query, param);

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

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var sql = "DELETE FROM Contact WHERE ID = @id";

            await connection.ExecuteAsync(sql.ToString(), new { id });
        }

        //public async Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int quantityItemsList)
        //{
        //    var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList };
        //    var query = @"SELECT * 
        //                  FROM Contact AS c
        //                  LEFT JOIN Company AS co ON  co.Id =c.CompanyId
        //                  JOIN ContactBook AS cb ON cb.Id = c.ContactBookId
        //                  LIMIT @quantityItemsList OFFSET @OffSet;";

        //    using var connection = new SqliteConnection(databaseConfig.ConnectionString);

        //    var result = await connection.QueryAsync<Contact, Company, ContactBook, Contact>(query,
        //        (contact, company, contactBook) =>
        //        {
        //            contact.Company = company;
        //            contact.ContactBook = contactBook;
        //            return contact;
        //        }, param);

        //    return result;
        //}

        public async Task<Contact> GetAsync(int id)
        {
            var param = new { id };
            var query = @"SELECT * 
                          FROM Contact AS c
                          LEFT JOIN Company AS co ON  co.Id =c.CompanyId
                          JOIN ContactBook AS cb ON cb.Id = c.ContactBookId
                          WHERE c.Id = @id";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.QueryAsync<Contact, Company, ContactBook, Contact>(query,
                (contact, company, contactBook) =>
                {
                    contact.Company = company;
                    contact.ContactBook = contactBook;
                    return contact;
                }, param);

            return result.FirstOrDefault();
        }

        public async Task<Pagination<Contact>> GetAsync(string param, int pageNumber, int quantityItemsList)
        {
            var parameter = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList, param };
            var query = @"SELECT * FROM Contact AS c 
                        LEFT JOIN Company AS co ON  co.Id = c.CompanyId 
                        JOIN ContactBook AS cb ON cb.Id = c.ContactBookId 
                        WHERE LOWER(c.Name) || LOWER(c.Phone) || LOWER(c.Email) || LOWER(c.Address) || LOWER(co.Name) LIKE '%'||@param||'%'
                        LIMIT @quantityItemsList OFFSET @OffSet;
                        SELECT COUNT(1) FROM Contact AS c 
                        LEFT JOIN Company AS co ON  co.Id = c.CompanyId 
                        WHERE LOWER(c.Name) || LOWER(c.Phone) || LOWER(c.Email) || LOWER(c.Address) || LOWER(co.Name) LIKE '%'||@param||'%';";


            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            using var result = await connection.QueryMultipleAsync(query, parameter).ConfigureAwait(false);

            var contacts = result.Read<Contact, Company, ContactBook, Contact>(
                (contact, company, contactBook) =>
                {
                    contact.Company = company;
                    contact.ContactBook = contactBook;
                    return contact;
                });

            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList);

            return new Pagination<Contact>(totalRows, totalPages, contacts);


            //using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            //var result = await connection.QueryAsync<Contact, Company, ContactBook, Contact>(query,
            //    (contact, company, contactBook) =>
            //    {
            //        contact.Company = company;
            //        contact.ContactBook = contactBook;
            //        return contact;
            //    }, parameter);

            //return result;
        }

        public async Task<Pagination<Contact>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
        {
            var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList };
            var query = @"SELECT * 
                          FROM Contact AS c
                          LEFT JOIN Company AS co ON  co.Id = c.CompanyId
                          JOIN ContactBook AS cb ON cb.Id = c.ContactBookId
                          LIMIT @quantityItemsList OFFSET @OffSet;
                          SELECT COUNT(1) FROM Contact;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            using var result = await connection.QueryMultipleAsync(query, param).ConfigureAwait(false);

            var contacts = result.Read<Contact, Company, ContactBook, Contact>(
                (contact, company, contactBook) =>
                {
                    contact.Company = company;
                    contact.ContactBook = contactBook;
                    return contact;
                });

            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList); ;


            return new Pagination<Contact>(totalRows, totalPages, contacts);
        }

        public async Task<Pagination<Contact>> GetAllByCompanyIdPaginatedAsync(int companyId, int pageNumber, int quantityItemsList)
        {
            var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList, companyId };
            var query = @"SELECT * 
                          FROM Contact AS c
                          LEFT JOIN Company AS co ON  co.Id = c.CompanyId
                          JOIN ContactBook AS cb ON cb.Id = c.ContactBookId
                          WHERE c.CompanyId = @companyId  
                          ORDER BY c.ContactBookId 
                          NULLS LAST
                          LIMIT @quantityItemsList OFFSET @OffSet;
                          SELECT COUNT(1) FROM Contact WHERE CompanyId = @companyId;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            using var result = await connection.QueryMultipleAsync(query, param).ConfigureAwait(false);

            var contacts = result.Read<Contact, Company, ContactBook, Contact>(
                (contact, company, contactBook) =>
                {
                    contact.Company = company;
                    contact.ContactBook = contactBook;
                    return contact;
                });

            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList); ;

            return new Pagination<Contact>(totalRows, totalPages, contacts);
        }

        public async Task<Pagination<Contact>> GetAllByContactBookIdPaginatedAsync(int contactBookId, int pageNumber, int quantityItemsList)
        {
            var param = new { OffSet = (pageNumber - 1) * quantityItemsList, quantityItemsList, contactBookId };

            var query = @"SELECT * 
                          FROM Contact AS c
                          LEFT JOIN Company AS co ON  co.Id = c.CompanyId
                          JOIN ContactBook AS cb ON cb.Id = c.ContactBookId
                          WHERE c.ContactBookId = @contactBookId
                          ORDER BY c.CompanyId 
                          NULLS LAST
                          LIMIT @quantityItemsList OFFSET @OffSet;
                          SELECT COUNT(1) FROM Contact WHERE ContactBookId = @contactBookId;";

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            using var result = await connection.QueryMultipleAsync(query, param).ConfigureAwait(false);

            var contacts = result.Read<Contact, Company, ContactBook, Contact>(
                (contact, company, contactBook) =>
                {
                    contact.Company = company;
                    contact.ContactBook = contactBook;
                    return contact;
                });

            var totalRows = result.ReadFirst<double>();
            var totalPages = Math.Ceiling(totalRows / quantityItemsList); ;

            return new Pagination<Contact>(totalRows, totalPages, contacts);
        }
    }
}
