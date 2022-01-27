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
        private readonly IContactBookService _contactBookService;
        private readonly IContactRepository _contactRepository;


        public CompanyService(ICompanyRepository companyRepository, IContactBookService contactBookService, IContactRepository contactRepository)
        {
            _companyRepository = companyRepository;
            _contactBookService = contactBookService;
            _contactRepository = contactRepository;
        }


        public async Task<Company> AddAsync(Company addCompanyDTO)
        {
            var contactBook = await _contactBookService.FindById(addCompanyDTO.ContactBookId);

            if (contactBook == null)
                return null;

            var company = new Company(addCompanyDTO.ContactBookId, addCompanyDTO.Name);
            var resultId = await _companyRepository.InsertAsync(company);

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

        public async Task<Company> FindById(int id)
        {
            return await _companyRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Company>> GetAllAsync(int pageNumber, int quantityItemsList)
        {
            return await _companyRepository.GetAllAsync(pageNumber, quantityItemsList);
        }

        public async Task<IEnumerable<Company>> GetAllByContactBookIdAsync(int contactBookId, int pageNumber, int quantityItemsList)
        {
            return await _companyRepository.GetAllByContactBookIdAsync(contactBookId, pageNumber, quantityItemsList);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var companyExist = await _companyRepository.GetAsync(company.Id);

            if (companyExist == null)
                return null;

            var updateResult = await _companyRepository.UpdateAsync(company);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            return company;
        }
    }
}
