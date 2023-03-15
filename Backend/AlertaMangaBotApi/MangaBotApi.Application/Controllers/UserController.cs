using MangaBotApi.Application.Dtos;
using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Intefaces;
using MangaBotApi.Service.Authenticator.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaBotApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseRepository<Usuario> _usuario;
        private readonly IBaseRepository<LogMangaBotApi> _logMangaBotApi;
        public UserController(IConfiguration config,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IBaseRepository<Usuario> usuario,
                              IBaseRepository<LogMangaBotApi> logMangaBotApi)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _usuario = usuario;
            _logMangaBotApi = logMangaBotApi;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                User user = new User
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PasswordHash = userDto.Password,
                    FullName = userDto.FullName
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);

                var userToReturn = new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Password = user.PasswordHash
                };

                if (result.Succeeded)
                {
                    return Created("GetUser", userToReturn);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                var userPermission = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());


                    return Ok(new
                    {
                        token = Authenticate.GenerateJWToken(appUser, _config, _userManager).Result,
                        username = appUser.UserName,
                        permissao = userPermission
                    });

                }

                return Unauthorized();
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

        [HttpPost("loginGoogle")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGoogle(string code)
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Login com Google falhou: {ex.Message}");
            }
        }
    }
}
