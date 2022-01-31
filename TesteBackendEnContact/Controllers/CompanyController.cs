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
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService, IMapper mapper)
        {
            _logger = logger;
            _companyService = companyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastra uma nova empresa.
        /// </summary>
        /// <param name="addCompanyDTO">Modelo de objeto addCompanyDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante do registro.</returns>
        [HttpPost]
        [SwaggerResponse(statusCode: 201, description: "Requisição concluída com sucesso. Retorna o objeto resultante do registro.", Type = typeof(CompanyDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível completar o cadastro.", Type = typeof(string))]
        public async Task<ActionResult<CompanyDTO>> AddCompanyAsync([FromBody] AddCompanyDTO addCompanyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Modelo não é valido");

                var company = _mapper.Map<Company>(addCompanyDTO);

                var companyResponse = await _companyService.AddAsync(company);

                if (companyResponse == null)
                    return UnprocessableEntity("Não foi possível  completar o cadastro.");

                var companyDTO = _mapper.Map<CompanyDTO>(companyResponse);

                return Ok(companyDTO);
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
        /// Pesquisa o cadastro de uma empresa específica.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser pesquisado.</param>
        /// <returns>Retorna o objeto CompanyDTO resultante da pesquisa.</returns>
        [HttpGet("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da pesquisa.", Type = typeof(CompanyDTO))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<CompanyDTO>> GetCompanyByIdAsync(int id)
        {
            var company = await _companyService.FindById(id);

            if (company == null)
                return NoContent();

            var companyDTO = _mapper.Map<CompanyDTO>(company);

            return Ok(companyDTO);
        }

        /// <summary>
        /// Atualiza os dados de registro de uma empresa
        /// </summary>
        /// <param name="companyDTO">Modelo de objeto CompanyDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante da atualização de registro.</returns>
        [HttpPut]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna o objeto resultante da atualização.", Type = typeof(CompanyDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível realizar a atualização", Type = typeof(string))]
        public async Task<ActionResult<CompanyDTO>> UpdateContactBook([FromBody] CompanyDTO companyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("O modelo de dados informado não é valido.");

                var company = _mapper.Map<Company>(companyDTO);

                var companyUpdated = await _companyService.UpdateAsync(company);

                if (companyUpdated == null)
                    return UnprocessableEntity("Não foi possível processar a atualização");

                var companyDTOResponse = _mapper.Map<CompanyDTO>(companyUpdated);

                return Ok(companyDTOResponse);
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
        /// Deleta cadastro da empresa referente ao identificador informado na URL da requisição.
        /// </summary>
        /// <param name="id">Identificador referente ao registro a ser deletado.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao deletar o registro.")]
        public async Task<ActionResult> DeleteCompanyAsync(int id)
        {
            try
            {
                await _companyService.DeleteAsync(id);

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
        /// Lista todas as empresas cadastrados.
        /// </summary>
        /// <returns>Retorna uma lista de CompanyDTO se houver algum resgistro.</returns>
        [HttpGet]
        [SwaggerResponse(statusCode: 200, description: "Requisição concluída com sucesso. Retorna uma lista do objeto ContactDTO.", Type = typeof(IEnumerable<CompanyDTO>))]
        [SwaggerResponse(statusCode: 204, description: "Requisição concluída com sucesso. Não há nenhum resgistro a ser retornado.")]
        public async Task<ActionResult<PaginationDTO<CompanyDTO>>> GetAllCompanysAsync([FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery, Range(1, 50)] int quantityItemsList = 5)
        {
            var inicio = DateTime.Now;
            try
            {
                var companysPaginated = await _companyService.GetAllPaginatedAsync(pageNumber, quantityItemsList);

                var paginationDTO = _mapper.Map<PaginationDTO<CompanyDTO>>(companysPaginated);

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
            finally
            {
                var final = DateTime.Now;

                var diff = final.Subtract(inicio);

                _logger.LogInformation($"======= Tempo decorrido: {diff.TotalMilliseconds}");
            }
        }
    }
}
