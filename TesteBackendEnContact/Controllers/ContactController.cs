﻿using AutoMapper;
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
    [ApiController]
    [Route("[controller]")]
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
        public async Task<ActionResult<ContactDTO>> AddContactAsync([FromBody] AddContactDTO addContactDTO)
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
        public async Task<ActionResult> DeleteContactAsync(int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados  com paginação.
        /// </summary>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContacstAsync([FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contacts = await _contactService.GetAllAsync(pageNumber, quantityItemsList);
                var contactsDTO = new List<ContactDTO>();

                if (!contacts.Any())
                    return NoContent();

                foreach (var contact in contacts)
                {
                    var contactDTO = _mapper.Map<ContactDTO>(contact);
                    contactsDTO.Add(contactDTO);
                }

                return Ok(contactsDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<ActionResult<ContactDTO>> GetContactByIdAsync(int id)
        {
            try
            {
                var contact = await _contactService.FindById(id);

                if (contact == null)
                    return NoContent();

                var contactDTOResponse = _mapper.Map<ContactDTO>(contact);

                return Ok(contactDTOResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de registro de um contato.
        /// </summary>
        /// <param name="contactDTO">Modelo de objeto ContactDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante da atualização de registro.</returns>
        [HttpPut]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da atualização.", Type = typeof(ContactDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível realizar a atualização", Type = typeof(string))]
        public async Task<ActionResult<ContactDTO>> UpdateContactBook([FromBody] ContactDTO contactDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var contact = _mapper.Map<Contact>(contactDTO);

                var contactUpdated = await _contactService.UpdateAsync(contact);

                if (contactUpdated == null)
                    return UnprocessableEntity("Não foi possível processar a atualização");

                var contactDTOResponse = _mapper.Map<ContactDTO>(contactUpdated);

                return Ok(contactDTOResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os contatos cadastrados para um determinada empresa com paginação.
        /// </summary>
        /// <param name="companyId">Identificador referente ao registro a ser pesquisado</param>
        /// <param name="pageNumber">Número da pagina</param>
        /// <param name="quantityItemsList">Quantidade e itens a ser retornado na lista.</param>
        /// <returns>Retorna uma lista de ContactDTO se houver algum resgistro paginado.</returns>
        [HttpGet("company/{companyId:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<ContactDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContactsByCompanyIdAsync(int companyId, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contacts = await _contactService.GetAllByCompanyIdAsync(companyId, pageNumber, quantityItemsList);
                var contactsDTO = new List<ContactDTO>();

                if (!contacts.Any())
                    return NoContent();

                foreach (var contact in contacts)
                {
                    var contactDTO = _mapper.Map<ContactDTO>(contact);
                    contactsDTO.Add(contactDTO);
                }

                return Ok(contactsDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContactsByContactBookIdAsync(int contactBookId, [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            try
            {
                var contacts = await _contactService.GetAllByContactBookIdAsync(contactBookId, pageNumber, quantityItemsList);
                var contactsDTO = new List<ContactDTO>();

                if (!contacts.Any())
                    return NoContent();

                foreach (var contact in contacts)
                {
                    var contactDTO = _mapper.Map<ContactDTO>(contact);
                    contactsDTO.Add(contactDTO);
                }

                return Ok(contactsDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
