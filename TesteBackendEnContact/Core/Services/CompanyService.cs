using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactRepository _contactRepository;


        public CompanyService(ICompanyRepository companyRepository,  IContactRepository contactRepository)
        {
            _companyRepository = companyRepository;
            _contactRepository = contactRepository;
        }


        public async Task<Company> AddAsync(Company company)
        {
            var companyNameExist = await _companyRepository.GetAsync(company.Name);

            if (companyNameExist != null)
                throw new Exception("Existe uma empresa com esse nome.");

            //var contactBook = await _contactBookService.FindById(company.ContactBookId);

            //if (contactBook == null)
            //    return null;

            var newcompany = new Company(company.Name);

            var resultId = await _companyRepository.InsertAsync(newcompany);

            if (resultId <= 0)
            {
                return null;
            }

            var companyResult = await _companyRepository.GetAsync(resultId);

            return companyResult;
        }

        public async Task<int> CountContactBookInCompanys(int contactBookId)
        {
            return await _companyRepository.CountContactBookInCompanys(contactBookId);
        }

        public async Task DeleteAsync(int id)
        {
            var companyInContacts = await _contactRepository.CountCompanyInContacts(id);

            if (companyInContacts > 0)
                throw new Exception("Existem registros com essa empresa.");

            var company = await _companyRepository.GetAsync(id);

            if (company == null)
                return;

            await _companyRepository.DeleteAsync(id);
        }

        public async Task<Company> FindAsync(string name)
        {
            return await _companyRepository.GetAsync(name);
        }

        public async Task<Company> FindById(int id)
        {
            return await _companyRepository.GetAsync(id);
        }

        public async Task<Pagination<Company>> GetAllPaginationAsync(int pageNumber, int quantityItemsList)
        {
            return await _companyRepository.GetAllPaginationAsync(pageNumber, quantityItemsList);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var companyExist = await _companyRepository.GetAsync(company.Id);

            if (companyExist == null)
                return null;

            var companyNameExist = await _companyRepository.GetAsync(company.Name);

            if (companyNameExist != null)
                throw new Exception("Existe uma empresa com esse nome.");

            var updateResult = await _companyRepository.UpdateAsync(company);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            return company;
        }
    }
}
