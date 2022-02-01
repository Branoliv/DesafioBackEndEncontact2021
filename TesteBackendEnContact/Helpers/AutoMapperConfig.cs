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
            CreateMap<UpdateContactDTO, Contact>();
            CreateMap<ContactDTO, Contact>();

            CreateMap<AddUserAuthDTO, UserAuthentication>();
            CreateMap<UserAuthDTO, UserAuthentication>();

            #endregion

            #region EntityToDto

            CreateMap<Company, CompanyDTO>();

            CreateMap<ContactBook, ContactBookDTO>();

            CreateMap<UserAuthentication, UserAuthDTO>();

            CreateMap<Contact, UpdateContactDTO>()
                .ForMember(c => c.Id, map => map.MapFrom(cd => cd.Id))
                .ForMember(c => c.Name, map => map.MapFrom(cd => cd.Name))
                .ForMember(c => c.Phone, map => map.MapFrom(cd => cd.Phone))
                .ForMember(c => c.Email, map => map.MapFrom(cd => cd.Email))
                .ForMember(c => c.Address, map => map.MapFrom(cd => cd.Address))
                .ForMember(c => c.CompanyId, map => map.MapFrom(cd => cd.CompanyId))
                .ForMember(c => c.ContactBookId, map => map.MapFrom(cd => cd.ContactBookId));

            CreateMap<Contact, ContactDTO>()
               .ForMember(c => c.Id, map => map.MapFrom(cd => cd.Id))
               .ForMember(c => c.Name, map => map.MapFrom(cd => cd.Name))
               .ForMember(c => c.Phone, map => map.MapFrom(cd => cd.Phone))
               .ForMember(c => c.Email, map => map.MapFrom(cd => cd.Email))
               .ForMember(c => c.Address, map => map.MapFrom(cd => cd.Address))
               .ForMember(c => c.CompanyId, map => map.MapFrom(cd => cd.CompanyId))
               .ForMember(c => c.ContactBookId, map => map.MapFrom(cd => cd.ContactBookId))
               .ForMember(c => c.CompanyDTO, map => map.MapFrom(cd => cd.Company))
               .ForMember(c => c.ContactBookDTO, map => map.MapFrom(cd => cd.ContactBook));

            CreateMap<Pagination<ContactBook>, PaginationDTO<ContactBookDTO>>()
                .ForMember(p => p.CountRows, map => map.MapFrom(p => p.CountRows))
                .ForMember(p => p.NumberOfPages, map => map.MapFrom(p => p.NumberOfPages))
                .ForMember(p => p.ListResult, map => map.MapFrom(p => p.ListResult));

            CreateMap<Pagination<Company>, PaginationDTO<CompanyDTO>>()
                .ForMember(p => p.CountRows, map => map.MapFrom(p => p.CountRows))
                .ForMember(p => p.NumberOfPages, map => map.MapFrom(p => p.NumberOfPages))
                .ForMember(p => p.ListResult, map => map.MapFrom(p => p.ListResult));

            CreateMap<Pagination<Contact>, PaginationDTO<ContactDTO>>()
                .ForMember(p => p.CountRows, map => map.MapFrom(p => p.CountRows))
                .ForMember(p => p.NumberOfPages, map => map.MapFrom(p => p.NumberOfPages))
                .ForMember(p => p.ListResult, map => map.MapFrom(p => p.ListResult));

            #endregion

        }
    }
}
