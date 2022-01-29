using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;

namespace TesteBackendEnContact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro na requisição.", Type = typeof(string))]
    public class ContactBookController : ControllerBase
    {
        private readonly ILogger<ContactBookController> _logger;
        private readonly IContactBookService _contactBookService;
        private readonly IMapper _mapper;

        public ContactBookController(ILogger<ContactBookController> logger, IContactBookService contactBookService, IMapper mapper)
        {
            _logger = logger;
            _contactBookService = contactBookService;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria uma nova Agenda.
        /// </summary>
        /// <param name="addContactBookDTO">Modelo de objeto AddContactBookDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante do registro.</returns>
        [HttpPost]
        [SwaggerResponse(statusCode: 201, description: "Requisição concluída com sucesso. Retorna o objeto resultante do registro.", Type = typeof(ContactBookDTO))]
        public async Task<ActionResult<ContactBookDTO>> AddContactBookAsync([FromBody] AddContactBookDTO addContactBookDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var contactBook = _mapper.Map<ContactBook>(addContactBookDTO);

                var contactBookResponse = await _contactBookService.AddAsync(contactBook);

                if (contactBookResponse == null)
                    return UnprocessableEntity();

                var contactBookDTO = _mapper.Map<ContactBookDTO>(contactBookResponse);

                return Created("", contactBookDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Pesquisa de uma Agenda específica.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser pesquisado.</param>
        /// <returns>Retorna o objeto ContactBookDTO resultante da pesquisa.</returns>
        [HttpGet("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da pesquisa.")]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<ContactBookDTO>> GetContactBookByIdAsync(int id)
        {
            try
            {
                var contactBook = await _contactBookService.FindById(id);

                if (contactBook == null)
                    return NoContent();

                var contactBookDTO = _mapper.Map<ContactBookDTO>(contactBook);

                return Ok(contactBookDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de registro de uma Agenda
        /// </summary>
        /// <param name="contactBookDTO">Modelo de objeto ContactBookDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante da atualização de registro.</returns>
        [HttpPut]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da atualização.", Type = typeof(ContactBookDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível realizar a atualização", Type = typeof(string))]
        public async Task<ActionResult<ContactBookDTO>> UpdateContactBook([FromBody] ContactBookDTO contactBookDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var contacBook = _mapper.Map<ContactBook>(contactBookDTO);

                var contactBookUpdated = await _contactBookService.UpdateAsync(contacBook);

                if (contactBookUpdated == null)
                    return UnprocessableEntity("Não foi possível processar a atualização");

                var contactBookDTOResponse = _mapper.Map<ContactBookDTO>(contactBookUpdated);

                return Ok(contactBookDTOResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta Agenda referente ao identificador informado na URL da requisição.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser deletado.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao deletar o registro.")]
        public async Task<ActionResult> DeleteContactBookAsync(int id)
        {
            try
            {
                await _contactBookService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todas as Agendas cadastradas
        /// </summary>
        /// <returns>Retorna uma lista de ContactBookDTO se houver algum resgistro.</returns>
        [HttpGet]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactBookDTO.", Type = typeof(IEnumerable<ContactBookDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<ContactBookDTO>>> GetAllContactBooksAsync([FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contactBooksPaginated = await _contactBookService.GetAllPaginationAsync(pageNumber, quantityItemsList);

                if (!contactBooksPaginated.ListResult.Any())
                    return NoContent();

                var contactBooksPaginatedDTO = _mapper.Map<PaginationDTO<ContactBookDTO>>(contactBooksPaginated);

                return Ok(contactBooksPaginatedDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
