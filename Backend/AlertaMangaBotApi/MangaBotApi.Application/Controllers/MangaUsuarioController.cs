using MangaBotApi.Application.Dtos;
using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Intefaces;
using MangaBotApi.Infra.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace MangaBotApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaUsuarioController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<Manga> _manga;
        private readonly IBaseRepository<Usuario> _usuario;
        private readonly IMangaUsuarioRepository _mangaUsuario;
        private readonly IBaseRepository<LogMangaBotApi> _logMangaBotApi;

        public MangaUsuarioController(
            UserManager<User> userManager,
            IBaseRepository<Manga> manga,
            IBaseRepository<Usuario> usuario,
            IMangaUsuarioRepository mangaUsuario,
            IBaseRepository<LogMangaBotApi> logMangaBotApi
            )
        {
            _userManager = userManager;
            _manga = manga;
            _usuario = usuario;
            _mangaUsuario = mangaUsuario;
            _logMangaBotApi = logMangaBotApi;
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> MangaUsuario(string username)
        {
            try
            {
                var userLogin = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

                if (userLogin is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"UserName não cadastrado.");

                var usuario = _usuario.Get(u => u.email.ToLower() == userLogin.Email.ToLower());

                if (usuario is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Usuario não cadastrado.");
                
                var mangaUsuario = _mangaUsuario.GetAllMangaUsuario();
                mangaUsuario = mangaUsuario.Where(u => u.Usuario.UsuarioId == usuario.UsuarioId).ToList();

                List<Manga> mangas = new List<Manga>();

                if (mangaUsuario.Count > 0)
                {
                    foreach (var mu in mangaUsuario)
                    {
                        mangas.Add(mu.Manga);
                    }
                }


                var userUsuarioDto = new UserUsuarioDto
                {
                    User = new UserDto
                    {
                        UserName = userLogin.UserName,
                        FullName = userLogin.FullName,
                        Email = userLogin.Email
                    },
                    Usuario = usuario,
                    Manga = mangas.Count <= 0 ? null : mangas
                };


                return Ok(userUsuarioDto);
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

        [HttpPost("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> MangaUsuario(string username, Manga manga)
        {
            try
            {
                var userLogin = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

                if (userLogin is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"UserName não cadastrado.");

                var usuario = _usuario.Get(u => u.email.ToLower() == userLogin.Email.ToLower());

                if (usuario is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Usuario não cadastrado.");


                var mangaUsuario = new MangaUsuario
                {
                    MangaId = manga.MangaId,
                    UsuarioId = usuario.UsuarioId
                };
                var result = _mangaUsuario.Add(mangaUsuario);

                return Ok(result);
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

        [HttpDelete("{username}/{mangaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMangaUsuario(string username, int mangaId)
        {
            try
            {
                var userLogin = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

                if (userLogin is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"UserName não cadastrado.");

                var usuario = _usuario.Get(u => u.email.ToLower() == userLogin.Email.ToLower());

                if (usuario is null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Usuario não cadastrado.");


                var mangaUsuario = _mangaUsuario.GetAllMangaUsuario();
                var mangaUsuarioFilter = mangaUsuario.Where(mu => mu.Usuario.UsuarioId == usuario.UsuarioId && mu.Manga.MangaId == mangaId).FirstOrDefault();

                var result = _mangaUsuario.Delete(mangaUsuarioFilter);

                return Ok(result);
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
    }
}
