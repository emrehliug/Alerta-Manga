using MangaBotApi.Application.Dtos;
using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaBotApi.Application.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IBaseRepository<LogMangaBotApi> _logMangaBotApi;

        public RoleController(
            RoleManager<Role> roleManager,
            IBaseRepository<LogMangaBotApi> logMangaBotApi)
        {
            this._roleManager = roleManager;
            this._logMangaBotApi = logMangaBotApi;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Roles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();

                var rolesDto = new List<RoleDto>();
                foreach (var role in roles) 
                {
                    rolesDto.Add(new RoleDto 
                    {
                        Id = role.Id,
                        Name = role.Name
                    });
                }
                if (rolesDto.Count > 0)
                    return Ok(rolesDto);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Lista de Roles vazia.");
            }
            catch (Exception ex)
            {
                _logMangaBotApi.Add(new LogMangaBotApi
                {
                    execucao = "Erro",
                    mensagem = ex.Message,
                    dataExecucao = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                });
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou: " + ex.Message + "");
            }
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Roles(int Id)
        {
            try
            {
                var role = _roleManager.Roles.Where(x => x.Id == Id).FirstOrDefault();

                var rolesDto = new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name
                    };

                if (rolesDto != null)
                    return Ok(rolesDto);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Lista de Roles vazia.");
            }
            catch (Exception ex)
            {
                _logMangaBotApi.Add(new LogMangaBotApi
                {
                    execucao = "Erro",
                    mensagem = ex.Message,
                    dataExecucao = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                });
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou: " + ex.Message + "");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Role(RoleDto roleDto)
        {
            try
            {
                var role = new Role
                {
                    Id= roleDto.Id,
                    Name= roleDto.Name
                };
                var result = await _roleManager.CreateAsync(role);


                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logMangaBotApi.Add(new LogMangaBotApi
                {
                    execucao = "Erro",
                    mensagem = ex.Message,
                    dataExecucao = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                });
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {ex.Message}");
            }
        }
    }
}
