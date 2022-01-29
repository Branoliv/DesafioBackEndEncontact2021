using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class CsvService : ICsvService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactBookRepository _contactBookRepository;
        private readonly ICompanyRepository _companyRepository;

        public CsvService(IContactRepository contactRepository, IContactBookRepository contactBookRepository, ICompanyRepository companyRepository)
        {
            _contactRepository = contactRepository;
            _contactBookRepository = contactBookRepository;
            _companyRepository = companyRepository;
        }


        public async Task<string> CreateCsv(int pageNumber, int quantityItemsList)
        {
            var contacts = await _contactRepository.GetAllPaginatedAsync(pageNumber, int.MaxValue);

            var builder = new StringBuilder();
            builder.AppendLine("Id;Name;Phone;Email;CompanyId;Company;ContactBookId;ContactBook;Address");

            foreach (Contact item in contacts.ListResult)
            {
                builder.AppendLine(item);
            }

            return builder.ToString();
        }

        public async Task<IEnumerable<Company>> UploadCompanyCsvFileAsync(IFormFile file)
        {
            var companies = new List<Company>();

            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(';'); //Titulo

                while (!sreader.EndOfStream)
                {
                    Company company = sreader.ReadLine();

                    companies.Add(company);
                }
            }

            if (companies.Count <= 0)
                return null;

            List<Company> companiesSaveResult = new List<Company>();

            foreach (var company in companies)
            {
                try
                {
                    var responseAdd = await _companyRepository.InsertAsync(company);

                    if (responseAdd > 0)
                        companiesSaveResult.Add(new Company(responseAdd, company.Name));
                }
                catch
                {
                    continue;
                }
            }

            return companiesSaveResult;
        }

        public async Task<IEnumerable<ContactBook>> UploadContactBookCsvFileAsync(IFormFile file)
        {
            var contactBooks = new List<ContactBook>();

            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(';'); //Titulo

                while (!sreader.EndOfStream)
                {
                    ContactBook contactBook = sreader.ReadLine();

                    contactBooks.Add(contactBook);
                }
            }

            if (contactBooks.Count <= 0)
                return null;

            List<ContactBook> contactBooksSaveResult = new List<ContactBook>();

            foreach (var contact in contactBooks)
            {
                try
                {
                    var responseAdd = await _contactBookRepository.InsertAsync(contact);

                    if (responseAdd > 0)
                        contactBooksSaveResult.Add(new ContactBook(responseAdd, contact.Name));
                }
                catch
                {
                    continue;
                }
            }

            return contactBooksSaveResult.ToList();
        }

        public async Task<IEnumerable<Contact>> UploadContactCsvFileAsync(IFormFile file)
        {
            var contacts = new List<Contact>();

            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(';'); //Titulo

                while (!sreader.EndOfStream)
                {
                    Contact cont = sreader.ReadLine();

                    contacts.Add(cont);
                }
            }

            if (contacts.Count <= 0)
                return null;

            List<Contact> contactsSaveResult = new List<Contact>();

            foreach (var contact in contacts)
            {
                try
                {
                    var responseAdd = await _contactRepository.InsertAsync(contact);

                    if (responseAdd >0)
                        contactsSaveResult.Add(new Contact(responseAdd,contact.Name, contact.Phone, contact.Email, contact.CompanyId, contact.ContactBookId, contact.Address));
                }
                catch
                {
                    continue;
                }
            }

            return contactsSaveResult.ToList();
        }
    }

}
