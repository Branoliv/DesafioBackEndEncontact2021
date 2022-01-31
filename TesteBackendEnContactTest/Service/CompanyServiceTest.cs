using NSubstitute;
using System;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Services;
using TesteBackendEnContact.Repository.Interface;
using Xunit;

namespace TesteBackendEnContactTest.Service
{

    public class CompanyServiceTest
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactRepository _contactRepository;
        private readonly CompanyService _companyService;
        private readonly Company _company;
        private readonly Company _newCompany;

        public CompanyServiceTest()
        {
            _companyRepository = Substitute.For<ICompanyRepository>();
            _contactRepository = Substitute.For<IContactRepository>();
            _companyService = new CompanyService(_companyRepository, _contactRepository);
            _company = new Company(1, "UnitTextName");
            _newCompany = new Company(2, "Test Company");
        }

        [Fact]
        public async Task AddCompany_withRepeatedName_ReturnArgumentException()
        {
            //Arrange
            _companyRepository.GetAsync(_company.Name)
                .Returns(_company);

            //Act
            Func<Task> act = async () =>
            {
                await _companyService.AddAsync(_company);
            };

            //Assert
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Equal("Existe uma empresa com esse nome.", exception.Message);
        }

        [Fact]
        public async Task Add_NewCompany__ReturnCompanyObject()
        {
            //Arrange

            _companyRepository.GetAsync(_newCompany.Name)
                .Returns((Company)null);

            _companyRepository.InsertAsync(_newCompany)
                .Returns(_newCompany.Id);

            _companyRepository.GetAsync(_newCompany.Id)
                .Returns(_newCompany);

            //Act
            var companyResponse = await _companyService.AddAsync(_newCompany);

            //Assert
            Assert.Equal("Test Company", companyResponse.Name);
        }

        [Fact]
        public async Task DeleteCompany_ExistingCompany__ReturnException()
        {
            //Arrange
            _companyRepository.GetAsync(_company.Id)
                .Returns(_company);

            _contactRepository.CountCompanyInContacts(_company.Id)
                .Returns(1);

            //Act
            Func<Task> act = async () =>
            {
                await _companyService.DeleteAsync(_company.Id);
            };

            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Existem registros com essa empresa.", exception.Message);
        }

        [Fact]
        public async Task DeleteCompany_DoesNotExistingCompanyWithRelationship__ReturnTrue()
        {
            //Arrange

            _companyRepository.GetAsync(1)
                .Returns(_company);

            _contactRepository.CountCompanyInContacts(_company.Id)
               .Returns(0);

            _companyRepository.DeleteAsync(_company.Id)
                .Returns(true);

            //Act
            var deleteCompanyResponse = await _companyService.DeleteAsync(_company.Id);

            //Assert
            Assert.True(deleteCompanyResponse);
        }
    }
}
