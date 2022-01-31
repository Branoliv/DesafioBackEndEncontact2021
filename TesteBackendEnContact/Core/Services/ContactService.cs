using System;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactBookRepository _contactBookRepository;
        private readonly ICompanyRepository _companyRepository;


        public ContactService(IContactRepository contactRepository, IContactBookRepository contactBookRepository, ICompanyRepository companyRepository)
        {
            _contactRepository = contactRepository;
            _contactBookRepository = contactBookRepository;
            _companyRepository = companyRepository;
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
            var contactBookExist = await _contactBookRepository.GetAsync(entitie.ContactBookId);

            if (contactBookExist == null)
                throw new ArgumentNullException(nameof(contactBookExist), "A agenda informada não está cadastrada.");

            if (entitie.CompanyId > 0)
            {
                var companyExist = await _companyRepository.GetAsync(entitie.CompanyId);

                if (companyExist == null)
                    throw new ArgumentNullException(nameof(companyExist), "A agenda informada não está cadastrada.");
            }

            var resultId = await _contactRepository.InsertAsync(entitie);

            if (resultId <= 0)
                return null;

            return await _contactRepository.GetAsync(resultId);
        }

        public async Task<Contact> UpdateAsync(Contact contact)
        {
            var contactExist = await _contactRepository.GetAsync(contact.Id);

            if (contactExist == null)
                throw new ArgumentNullException(nameof(contactExist), "O contato informado não foi encontrado.");

            if (contact.CompanyId > 0)
            {
                var companyExist = await _companyRepository.GetAsync(contact.CompanyId);

                if (companyExist == null)
                    throw new ArgumentNullException(nameof(companyExist), "A agenda informada não está cadastrada.");
            }

            var updateResult = await _contactRepository.UpdateAsync(contact);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            return await _contactRepository.GetAsync(contact.Id); ;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact == null)
                return false;

            return await _contactRepository.DeleteAsync(id);
        }

        public async Task<Contact> FindById(int id)
        {
            return await _contactRepository.GetAsync(id);
        }

        public async Task<Pagination<Contact>> GetAsync(string param, int pageNumber, int quantityItemsList)
        {
            return await _contactRepository.GetAsync(param.ToLower().Trim(), pageNumber, quantityItemsList);
        }

        public async Task<Pagination<Contact>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
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

        public async Task<Pagination<Contact>> GetAllByContactBookIdAndCompanyIdPaginatedAsync(int contactBookId, int companyId, int pageNumber, int quantityItemsList)
        {
            return await _contactRepository.GetAllByContactBookIdAndCompanyIdPaginatedAsync(contactBookId, companyId, pageNumber, quantityItemsList);
        }
    }
}

