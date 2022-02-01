using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;

namespace TesteBackendEnContact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro na requisição.", Type = typeof(string))]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(ILogger<ContactController> logger, IContactService contactService, IMapper mapper)
        {
            _logger = logger;
            _contactService = contactService;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="addContactDTO">Modelo de objeto AddContactDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante do registro.</returns>
        [HttpPost]
        [SwaggerResponse(statusCode: 201, description: "Requisição concluída com sucesso. Retorna o objeto resultante do registro.", Type = typeof(ContactDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível completar o cadastro.", Type = typeof(string))]
        public async Task<ActionResult<ContactDTO>> CreateAsync([FromBody] AddContactDTO addContactDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var contact = _mapper.Map<Contact>(addContactDTO);

                var contactResult = await _contactService.AddAsync(contact);

                if (contactResult == null)
                    return UnprocessableEntity("Não foi possível  completar o cadastro.");

                var contactDTOResponse = _mapper.Map<ContactDTO>(contactResult);

                return Created("", contactDTOResponse);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Pesquisa um contato específico.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser pesquisado.</param>
        /// <returns>Retorna o objeto ContactDTO resultante da pesquisa.</returns>
        [HttpGet("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da pesquisa.", Type = typeof(ContactDTO))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<ContactDTO>> GetAsync(int id)
        {
            try
            {
                var contact = await _contactService.FindById(id);

                if (contact == null)
                    return NoContent();

                var contactDTOResponse = _mapper.Map<ContactDTO>(contact);

                return Ok(contactDTOResponse);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Atualiza os dados de registro de um contato.
        /// </summary>
        /// <param name="updateContactDTO">Modelo de objeto ContactDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante da atualização de registro.</returns>
        [HttpPut]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da atualização.", Type = typeof(ContactDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível realizar a atualização", Type = typeof(string))]
        public async Task<ActionResult<ContactDTO>> UpdateAsync([FromBody] UpdateContactDTO updateContactDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var contact = _mapper.Map<Contact>(updateContactDTO);

                var contactUpdated = await _contactService.UpdateAsync(contact);

                if (contactUpdated == null)
                    return UnprocessableEntity("Não foi possível processar a atualização");

                var contactDTOResponse = _mapper.Map<ContactDTO>(contactUpdated);

                return Ok(contactDTOResponse);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Deleta Agenda referente ao identificador informado na URL da requisição.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser deletado.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao deletar o registro.")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Pesquisa um contato por qualquer propriedade.
        /// </summary>
        /// <param name="param">Identificador referente ao registro a ser pesquisado.</param>
        /// <param name="pageNumber">Número da pagina</param>
        /// <param name="quantityItemsList">Quantidade e itens a ser retornado na lista.</param>
        /// <returns>Retorna uma lista de objeto ContactDTO resultante da pesquisa.</returns>
        [HttpGet("{param}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da pesquisa.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<Pagination<ContactDTO>>> GetAsync(string param, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            var inicio = DateTime.Now;
            try
            {
                var contacts = await _contactService.GetAsync(param, pageNumber, quantityItemsList);

                if (!contacts.ListResult.Any())
                    return NoContent();

                var contactsDTO = _mapper.Map<PaginationDTO<ContactDTO>>(contacts);

                return Ok(contactsDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

                var diff = final.Subtract(inicio);

                _logger.LogInformation($"======= Tempo decorrido: {diff.TotalMilliseconds}");
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados  com paginação.
        /// </summary>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<ContactDTO>>> GetAllPaginatedAsync([FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contactsPaginated = await _contactService.GetAllPaginatedAsync(pageNumber, quantityItemsList);

                if (!contactsPaginated.ListResult.Any())
                    return NoContent();

                var paginationDTO = _mapper.Map<PaginationDTO<ContactDTO>>(contactsPaginated);

                return Ok(paginationDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados para uma determinada empresa com paginação.
        /// Para listar contatos sem empresa informe o parametro companyId = 0.
        /// </summary>
        /// <param name="companyId">Identificador referente ao registro a ser pesquisado</param>
        /// <param name="pageNumber">Número da pagina</param>
        /// <param name="quantityItemsList">Quantidade e itens a ser retornado na lista.</param>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet("company/{companyId:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<ContactDTO>>> GetAllByCompanyIdPaginatedAsync(int companyId, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contactsPaginated = await _contactService.GetAllByCompanyIdPaginatedAsync(companyId, pageNumber, quantityItemsList);

                if (!contactsPaginated.ListResult.Any())
                    return NoContent();

                var contactsPaginatedDTO = _mapper.Map<PaginationDTO<ContactDTO>>(contactsPaginated);

                return Ok(contactsPaginatedDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados para um determinada agenda com paginação.
        /// </summary>
        /// <param name="contactBookId">Identificador referente ao registro a ser pesquisado</param>
        /// <param name="pageNumber">Número da pagina</param>
        /// <param name="quantityItemsList">Quantidade e itens a ser retornado na lista.</param>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet("contactbook/{contactBookId:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<ContactDTO>>> GetAllByContactBookIdPaginatedAsync(int contactBookId, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contactsPaginated = await _contactService.GetAllByContactBookIdPaginatedAsync(contactBookId, pageNumber, quantityItemsList);

                if (!contactsPaginated.ListResult.Any())
                    return NoContent();

                var contactsPaginatedDTO = _mapper.Map<PaginationDTO<ContactDTO>>(contactsPaginated);

                return Ok(contactsPaginatedDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados para um determinada agenda com paginação.
        /// </summary>
        /// <param name="contactBookId">Identificador referente ao registro a ser pesquisado</param>
        /// <param name="companyId">Identificador referente ao registro a ser pesquisado</param>
        /// <param name="pageNumber">Número da pagina</param>
        /// <param name="quantityItemsList">Quantidade e itens a ser retornado na lista.</param>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet("contactbook/{contactBookId:int}/company/{companyId}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<ContactDTO>>> GetAllByContactBookIdAndCompanyIdPaginatedAsync(int contactBookId,int companyId, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contactsPaginated = await _contactService.GetAllByContactBookIdAndCompanyIdPaginatedAsync(contactBookId, companyId, pageNumber, quantityItemsList);

                if (!contactsPaginated.ListResult.Any())
                    return NoContent();

                var contactsPaginatedDTO = _mapper.Map<PaginationDTO<ContactDTO>>(contactsPaginated);

                return Ok(contactsPaginatedDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"====={ex.StackTrace}");
                _logger.LogError(ex.Message);
                return BadRequest("Desculpe! Ocorreu um erro!");
            }
        }
    }
}
