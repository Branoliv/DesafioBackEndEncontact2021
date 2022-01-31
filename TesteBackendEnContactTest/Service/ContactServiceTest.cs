using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Core.Services;
using TesteBackendEnContact.Repository.Interface;
using Xunit;

namespace TesteBackendEnContactTest.Service
{
    public class ContactServiceTest
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactBookRepository _contactBookRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ContactService _contactService;
        private readonly Contact _contact;
        private readonly Contact _newContact;
        private readonly ContactBook _contactBook;
        private readonly Company _company;


        public ContactServiceTest()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _contactBookRepository = Substitute.For<IContactBookRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _contactService = new ContactService(_contactRepository, _contactBookRepository, _companyRepository);
            _contact = new Contact("UnixTextName", "", "test@email.com", 0, 0, "");
            _newContact = new Contact(2, "Test Contact", "", "test@email.com", 1, 1, "");
            _contactBook = new ContactBook(1, "UnitTextName");
            _company = new Company(1, "UnitTextName");
        }


        [Fact]
        public async Task AddContact_ContactBookDoesNotExist_ReturnArgumentNullException()
        {
            //Arrange
            _contactBookRepository.GetAsync(_contact.ContactBookId)
                .Returns((ContactBook)null);

            //Act 
            Func<Task> act = async () =>
            {
                await _contactService.AddAsync(_contact);
            };

            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
            Assert.Equal("A agenda informada não está cadastrada. (Parameter 'contactBookExist')", exception.Message);
        }

        [Fact]
        public async Task AddContact_InformedCompanyDoesNotExist_ReturnArgumentNullException()
        {
            //Arrange
            _contactBookRepository.GetAsync(_newContact.ContactBookId)
                .Returns(_contactBook);

            _companyRepository.GetAsync(_newContact.CompanyId)
                .Returns((Company)null);

            //Act
            Func<Task> act = async () =>
            {
                await _contactService.AddAsync(_newContact);
            };

            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
            Assert.Equal("A agenda informada não está cadastrada. (Parameter 'companyExist')", exception.Message);
        }

        [Fact]
        public async Task AddContact_InformingNewContact_ReturnContactObject()
        {
            //Arrange
            _contactBookRepository.GetAsync(_newContact.ContactBookId)
                .Returns(_contactBook);

            _companyRepository.GetAsync(_newContact.CompanyId)
                .Returns(_company);

            _contactRepository.InsertAsync(_newContact)
                .Returns(2);

            _contactRepository.GetAsync(_newContact.Id)
                .Returns(_newContact);

            //Act
            var contactResponse = await _contactService.AddAsync(_newContact);

            //Assert

            Assert.Equal("Test Contact", contactResponse.Name);
        }


        [Fact]
        public async Task DeleteContact_InformedContactDoesNotExist__ReturnFalse()
        {
            //Arrange
            _contactRepository.GetAsync(_contact.Id)
                .Returns((Contact)null);


            //Act

            var contactResponse = await _contactService.DeleteAsync(_contact.Id);

            //Assert
            Assert.False(contactResponse);
        }

        [Fact]
        public async Task DeleteContact_ExistingContact__ReturnTrue()
        {
            //Arrange
            _contactRepository.GetAsync(_contact.Id)
                .Returns(_contact);

            _contactRepository.DeleteAsync(_contact.Id)
                .Returns(true);

            //Act

            var contactResponse = await _contactService.DeleteAsync(_contact.Id);

            //Assert
            Assert.True(contactResponse);
        }
    }
}
