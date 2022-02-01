using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.DTOs;
using TesteBackendEnContact.Core.Domain.Entities;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Security;

namespace TesteBackendEnContact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro na requisição.", Type = typeof(string))]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, IAuthService authService)
        {
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="addUserAuthDTO">Modelo de objeto addCompanyDTO a ser informado no corpo da requisição.</param>
        /// <returns>Retorna o objeto resultante do registro.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        [SwaggerResponse(statusCode: 201, description: "Requisição concluída com sucesso. Retorna o objeto resultante do registro.", Type = typeof(UserAuthDTO))]
        [SwaggerResponse(statusCode: 422, description: "Não foi possível completar o cadastro.", Type = typeof(string))]
        public async Task<ActionResult<UserAuthDTO>> AddAsync([FromBody] AddUserAuthDTO addUserAuthDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de dados invalido.");

            try
            {
                var userAuth = _mapper.Map<UserAuthentication>(addUserAuthDTO);

                var userCreated = await _authService.AddAsync(userAuth);

                if (userCreated == null)
                    return UnprocessableEntity("Não foi possível criar o usuário.");

                var userDTO = _mapper.Map<UserAuthDTO>(userCreated);

                return Created("", userDTO);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public object Auth([FromBody] UserAuthDTO userAuthDTO, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            bool validCredentials = false;

            var user = _mapper.Map<UserAuthentication>(userAuthDTO);

            var response = _authService.Auth(user).Result;

            validCredentials = response != null;

            if (validCredentials)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(response.Id.ToString(), "Id"),
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),
                        new Claim("UserAuth", JsonConvert.SerializeObject(response))
                    });

                DateTime creationDate = DateTime.Now;
                DateTime expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.signingCredentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expirationDate
                });

                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    userName = response.UserName,
                    userId = response.Id
                };
            }
            else
            {
                return new
                {
                    authenticated = false
                };
            }
        }
    }
}
