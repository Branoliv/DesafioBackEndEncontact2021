using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Interface.Services;

namespace TesteBackendEnContact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro na requisição.", Type = typeof(string))]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ICsvService _csvService;
        private readonly IMapper _mapper;


        public FilesController(ILogger<ContactController> logger, ICsvService csvService, IMapper mapper)
        {
            _logger = logger;
            _csvService = csvService;
            _mapper = mapper;
        }


        /// <summary>
        /// Exporta todos os resgistros para um arquivo .csv.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("download-all")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna um arquivo csv.")]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult> GetAllCsvFileAsync([FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var csvFile = await _csvService.CreateCsv(pageNumber, quantityItemsList);

                if (string.IsNullOrEmpty(csvFile))
                    return NoContent();

                var file = File(Encoding.UTF8.GetBytes(csvFile), "text/csv", "contacts.csv");

                return file;
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Importa arquivo .csv 
        /// Para todos uploads do arquivo .csv, devem seguir o seguinte padrão de 
        /// Colunas: name;phone;email;companyId;companyNam;contactBookId;contactBookName;address
        /// </summary>
        /// <returns>Retorna uma lista de ContactDTO importados.</returns>
        [HttpPost]
        [Route("upload/contact")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto CompanyDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> UploadContactCsvFileAsync(IFormFile file)
        {
            var incio = DateTime.Now;

            if (file == null)
                return BadRequest("O arquivo não foi anexado.");

            if (!file.FileName.EndsWith(".csv"))
                return BadRequest("O arquivo carregado não é do tipo .csv");

            try
            {
                var contactsSaveResult = await _csvService.UploadContactCsvFileAsync(file);

                if (!contactsSaveResult.Any())
                    return UnprocessableEntity("Nem todos contatos foram salvos.");

                IEnumerable<ContactDTO> contactsResult = _mapper.Map<List<ContactDTO>>(contactsSaveResult);

                return Ok(contactsResult);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
            finally
            {
                var final = DateTime.Now;

                var diff = final.Subtract(incio);

                _logger.LogInformation($"======= Tempo decorrido: {diff}");
            }
        }


        /// <summary>
        /// Importa arquivo .csv  
        /// Para todos uploads do arquivo .csv, devem seguir o seguinte padrão de 
        /// Colunas: name;phone;email;companyId;companyNam;contactBookId;contactBookName;address
        /// </summary>
        /// <returns>Retorna uma lista de ContactBookDTO importados.</returns>
        [HttpPost]
        [Route("upload/contactbook")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto CompanyDTO.", Type = typeof(IEnumerable<ContactBookDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<IEnumerable<ContactBookDTO>>> UploadContactBookCsvFileAsync(IFormFile file)
        {
            var incio = DateTime.Now;

            if (file == null)
                return BadRequest("O arquivo não foi anexado.");

            if (!file.FileName.EndsWith(".csv"))
                return BadRequest("O arquivo carregado não é do tipo .csv");

            try
            {
                var contactBooksSaveResult = await _csvService.UploadContactBookCsvFileAsync(file);

                if (!contactBooksSaveResult.Any())
                    return UnprocessableEntity("Nem todos contatos foram salvos.");

                IEnumerable<ContactBookDTO> contactBooksResult = _mapper.Map<List<ContactBookDTO>>(contactBooksSaveResult);

                return Ok(contactBooksResult);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
            finally
            {
                var final = DateTime.Now;

                var diff = final.Subtract(incio);

                _logger.LogInformation($"======= Tempo decorrido: {diff}");
            }
        }

        /// <summary>
        /// Importa arquivo .csv 
        /// Para todos uploads do arquivo .csv, devem seguir o seguinte padrão de 
        /// Colunas: name;phone;email;companyId;companyNam;contactBookId;contactBookName;address
        /// </summary>
        /// <returns>Retorna uma lista de CompanyDTO importados.</returns>
        [HttpPost]
        [Route("upload/company")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto CompanyDTO.", Type = typeof(IEnumerable<CompanyDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> UploadCompanyCsvFileAsync(IFormFile file)
        {
            var incio = DateTime.Now;

            if (file == null)
                return BadRequest("O arquivo não foi anexado.");

            if (!file.FileName.EndsWith(".csv"))
                return BadRequest("O arquivo carregado não é do tipo .csv");

            try
            {
                var companiesSaveResult = await _csvService.UploadCompanyCsvFileAsync(file);

                if (!companiesSaveResult.Any())
                    return UnprocessableEntity("Nem todos contatos foram salvos.");

                IEnumerable<CompanyDTO> contactBooksResult = _mapper.Map<List<CompanyDTO>>(companiesSaveResult);

                return Ok(contactBooksResult);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
            finally
            {
                var final = DateTime.Now;

                var diff = final.Subtract(incio);

                _logger.LogInformation($"======= Tempo decorrido: {diff}");
            }
        }
    }
}
