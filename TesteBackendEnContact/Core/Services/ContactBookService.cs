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
        private readonly IContactRepository _contactRepository;


        public ContactBookService(IContactBookRepository contactBookRepository, IContactRepository contactRepository)
        {
            _contactBookRepository = contactBookRepository;
            _contactRepository = contactRepository;
        }


        public async Task<ContactBook> AddAsync(ContactBook contactBook)
        {
            if (contactBook == null)
                return null;

            var contactBookExist = await _contactBookRepository.GetAsync(contactBook.Name);

            if (contactBookExist != null)
                throw new ArgumentException("Existe uma agenda com esse nome.");

            var resultId = await _contactBookRepository.InsertAsync(contactBook);

            if (resultId <= 0)
                return null;

            var contactBookResponse = await _contactBookRepository.GetAsync(resultId);

            return contactBookResponse;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contactBook = await _contactBookRepository.GetAsync(id);

            if (contactBook == null)
                return false;

            var contactBookInContacts = await _contactRepository.CountContactBookInContacts(id);

            if (contactBookInContacts > 0)
                throw new Exception("Existem registros com essa agenda.");

            return await _contactBookRepository.DeleteAsync(id);
        }

        public async Task<ContactBook> FindAsync(string name)
        {
            return await _contactBookRepository.GetAsync(name);
        }

        public async Task<ContactBook> FindById(int id)
        {
            var cb = await _contactBookRepository.GetAsync(id);

            if (cb == null)
                return null;

            var cbR = new ContactBook(cb.Id, cb.Name);
            return cbR;
        }

        public async Task<Pagination<ContactBook>> GetAllPaginatedAsync(int pageNumber, int quantityItemsList)
        {
            return await _contactBookRepository.GetAllPaginatedAsync(pageNumber, quantityItemsList);
        }

        public async Task<ContactBook> UpdateAsync(ContactBook contactBook)
        {
            var contactBookExist = await _contactBookRepository.GetAsync(contactBook.Id);

            if (contactBookExist == null)
                return null;

            var contactBookNameExist = await _contactBookRepository.GetAsync(contactBook.Name.ToLower());

            if (contactBookNameExist != null)
                throw new ArgumentException("Existe uma agenda com esse nome.");

            var updateResult = await _contactBookRepository.UpdateAsync(contactBook);

            if (!updateResult)
                throw new Exception("Não foi possível realizar a atualização");

            var contactBookUpdated = await _contactBookRepository.GetAsync(contactBook.Id);

            return contactBookUpdated;
        }
    }
}
