using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.Entities;

namespace TesteBackendEnContact.Core.Interface.Services
{
    public interface ICsvService
    {
        Task<string> CreateCsv(int pageNumber, int quantityItemsList);
        Task<IEnumerable<Contact>> UploadContactCsvFileAsync(IFormFile file);
        Task<IEnumerable<ContactBook>> UploadContactBookCsvFileAsync(IFormFile file);
        Task<IEnumerable<Company>> UploadCompanyCsvFileAsync(IFormFile file);
    }
}
