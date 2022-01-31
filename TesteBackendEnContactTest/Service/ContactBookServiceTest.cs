using NSubstitute;
using System;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Services;
using TesteBackendEnContact.Repository.Interface;
using Xunit;

namespace TesteBackendEnContactTest.Service
{
    public class ContactBookServiceTest
    {
        private readonly IContactBookRepository _contactBookRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ContactBookService _contactBookService;
        private readonly ContactBook _contactBook;
        private readonly ContactBook _newContactBook;

        public ContactBookServiceTest()
        {
            _contactBookRepository = Substitute.For<IContactBookRepository>();
            _contactRepository = Substitute.For<IContactRepository>();
            _contactBookService = new ContactBookService(_contactBookRepository, _contactRepository);
            _contactBook = new ContactBook(1, "UnitTextName");
            _newContactBook = new ContactBook(2, "Test ContactBook");
        }

        [Fact]
        public async Task AddContactBook_WithRepeatedName_ReturnArgumentException()
        {
            //Arrange
            _contactBookRepository.GetAsync(_contactBook.Name)
                .Returns(_contactBook);


            //Act
            Func<Task> act = async () =>
           {
               await _contactBookService.AddAsync(_contactBook);
           };


            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Equal("Existe uma agenda com esse nome.", exception.Message);
        }

        [Fact]
        public async Task Add_NewContactBook_ReurnContactBookObject()
        {
            //Arrange
            _contactBookRepository.GetAsync(_newContactBook.Name)
                .Returns((ContactBook)null);

            _contactBookRepository.InsertAsync(_newContactBook)
                .Returns(_newContactBook.Id);

            _contactBookRepository.GetAsync(_newContactBook.Id)
                .Returns(_newContactBook);

            //Act
            var addContactBookResponse = await _contactBookService.AddAsync(_newContactBook);

            //Assert
            Assert.Equal("Test ContactBook", addContactBookResponse.Name);
        }

        [Fact]
        public async Task DeleteContactBook_ExistingContactBookWithRelationshig_ReturnException()
        {
            //Arrange
            _contactBookRepository.GetAsync(_contactBook.Id)
                .Returns(_contactBook);

            _contactRepository.CountContactBookInContacts(_contactBook.Id)
                .Returns(1);

            //Act
            Func<Task> act = async () =>
            {
                await _contactBookService.DeleteAsync(_contactBook.Id);
            };

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Existem registros com essa agenda.", exception.Message);
        }
    
        [Fact]
        public async Task DeleteContactBook_DoesNotExistingContactBookWithRelationship_ReturnTrue()
        {
            //Arrange
            _contactBookRepository.GetAsync(_contactBook.Id)
                .Returns(_contactBook);

            _contactRepository.CountContactBookInContacts(_contactBook.Id)
                .Returns(0);

            _contactBookRepository.DeleteAsync(_contactBook.Id)
                .Returns(true);

            //Act
            var deleteContactBookResponse = await _contactBookRepository.DeleteAsync(_contactBook.Id);

            //Assert
            Assert.True(deleteContactBookResponse);
        }
    }
}
