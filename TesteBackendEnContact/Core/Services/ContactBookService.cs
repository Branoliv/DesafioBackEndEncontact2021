using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Core.Services
{
    public class ContactBookService : IContactBookService
    {
        private readonly IContactBookRepository _contactBookRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactRepository _contactRepository;


        public ContactBookService(IContactBookRepository contactBookRepository, ICompanyRepository companyRepository, IContactRepository contactRepository)
        {
            _contactBookRepository = contactBookRepository;
            _companyRepository = companyRepository;
            _contactRepository = contactRepository;
        }


        public async Task<ContactBook> AddAsync(ContactBook contactBook)
        {
            if (contactBook == null)
                return null;

            //TODO - Verificar se nome da agenda já existe

            var resultId = await _contactBookRepository.InsertAsync(contactBook);

            if (resultId <= 0)
                return null;

            var contactBookResponse = await _contactBookRepository.GetAsync(resultId);

            return contactBookResponse;
        }

        public async Task DeleteAsync(int id)
        {
            var contactBook = await _contactBookRepository.GetAsync(id);

            if (contactBook == null)
                return;

            var contactBookInContacts = await _contactRepository.CountContactBookInContacts(id);

            if (contactBookInContacts > 0)
                throw new Exception("Existem registros com essa agenda.");

            var contactBookInCompanys = await _companyRepository.CountContactBookInCompanys(id);

            if (contactBookInCompanys > 0)
                throw new Exception("Existem registros com essa agenda.");

            await _contactBookRepository.DeleteAsync(id);
        }

        public async Task<ContactBook> FindById(int id)
        {
            var cb = await _contactBookRepository.GetAsync(id);

            if (cb == null)
                return null;

            var cbR = new ContactBook(cb.Id, cb.Name);
            return cbR;
        }

        public async Task<IEnumerable<ContactBook>> GetAllAsync(int pageNumber, int quantityItemsList)
        {
            return await _contactBookRepository.GetAllAsync(pageNumber, quantityItemsList);
        }

        public async Task<ContactBook> UpdateAsync(ContactBook contactBook)
        {
            var contactBookExist = await _contactBookRepository.GetAsync(contactBook.Id);

            if (contactBookExist == null)
                return null;

            var updateResult = await _contactBookRepository.UpdateAsync(contactBook);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            var contactBookUpdated = await _contactBookRepository.GetAsync(contactBook.Id);

            return contactBookUpdated;
        }
    }
}
