using AutoMapper;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Helpers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region DtoToEntity

            CreateMap<AddCompanyDTO, Company>();
            CreateMap<CompanyDTO, Company>();

            CreateMap<AddContactBookDTO, ContactBook>();
            CreateMap<ContactBookDTO, ContactBook>();

            CreateMap<AddContactDTO, Contact>();
            CreateMap<ContactDTO, Contact>();

            #endregion

            #region EntityToDto

            CreateMap<Company, CompanyDTO>();
            CreateMap<ContactBook, ContactBookDTO>();
            CreateMap<Contact, ContactDTO>();

            #endregion

        }
    }
}
