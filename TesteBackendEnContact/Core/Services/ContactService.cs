using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactBookService _contactBookService;
        private readonly ICompanyService _companyService;


        public ContactService(IContactRepository contactRepository, IContactBookService contactBookService, ICompanyService companyService)
        {
            _contactRepository = contactRepository;
            _contactBookService = contactBookService;
            _companyService = companyService;
        }


        public async Task<int> CountCompanyInContacts(int companyId)
        {
            return await _contactRepository.CountCompanyInContacts(companyId);
        }

        public async Task<int> CountContactBookInContacts(int contactBookId)
        {
            return await _contactRepository.CountContactBookInContacts(contactBookId);
        }

        public async Task<Contact> AddAsync(Contact entitie)
        {
            var contactBook = await _contactBookService.FindById(entitie.ContactBookId);

            if (contactBook == null)
                throw new Exception("A agenda informada não está cadastrada.");

            if (entitie.CompanyId > 0)
            {
                var company = await _companyService.FindById(entitie.CompanyId);

                if (company == null)
                    throw new Exception("A empresa informada não está cadastrada.");
            }

            var contact = new Contact(
                entitie.Name,
                entitie.Phone,
                entitie.Email,
                entitie.CompanyId,
                entitie.ContactBookId,
                entitie.Address);

            var resultId = await _contactRepository.InsertAsync(contact);

            if (resultId <= 0)
                return null;

            return await _contactRepository.GetAsync(resultId);
        }

        public async Task<Contact> UpdateAsync(Contact contact)
        {
            var contactExist = await _contactRepository.GetAsync(contact.Id);

            if (contactExist == null)
                return null;

            if (contact.CompanyId > 0)
            {
                var company = await _companyService.FindById(contact.CompanyId);

                if (company == null)
                    throw new Exception("A empresa informada não está cadastrada.");
            }

            var updateResult = await _contactRepository.UpdateAsync(contact);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            return await _contactRepository.GetAsync(contact.Id); ;
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact == null)
                return;

            await _contactRepository.DeleteAsync(id);
        }

        public async Task<Contact> FindById(int id)
        {
            return await _contactRepository.GetAsync(id);
        }

        public async Task<Pagination<Contact>> GetAsync(string param, int pageNumber, int quantityItemsList)
        {
            var result = await _contactRepository.GetAsync(param.ToLower().Trim(), pageNumber, quantityItemsList);

            return result;
        }

        public async Task<Pagination<Contact>> GetAllPaginationAsync(int pageNumber, int quantityItemsList)
        {
            return await _contactRepository.GetAllPaginatedAsync(pageNumber, quantityItemsList);
        }

        public async Task<Pagination<Contact>> GetAllByCompanyIdPaginatedAsync(int companyId, int pageNumber, int quantityItemsList)
        {
            return await _contactRepository.GetAllByCompanyIdPaginatedAsync(companyId, pageNumber, quantityItemsList);
        }

        public async Task<Pagination<Contact>> GetAllByContactBookIdPaginatedAsync(int contactBookId, int pageNumber, int quantityItemsList)
        {
            return await _contactRepository.GetAllByContactBookIdPaginatedAsync(contactBookId, pageNumber, quantityItemsList);
        }
    }
}

