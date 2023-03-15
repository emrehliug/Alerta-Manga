using MangaBotApi.Application.Dtos;
using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MangaBotApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBaseRepository<LogMangaBotApi> _logMangaBotApi;
        private readonly IBaseRepository<Manga> _manga;
        private readonly IMangaUsuarioRepository _mangaUsuario;

        public MangaController(IConfiguration config,
                              IBaseRepository<LogMangaBotApi> logMangaBotApi,
                              IBaseRepository<Manga> manga,
                              IMangaUsuarioRepository mangaUsuario)
        {
            _config = config;
            _logMangaBotApi = logMangaBotApi;
            _manga = manga;
            _mangaUsuario = mangaUsuario;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMangas()
        {
            try
            {
                var mangas = _manga.GetAll();
                if (mangas.Count > 0)
                    return Ok(mangas);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Lista de Mangas vazia.");
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
        public async Task<IActionResult> Manga(Manga manga)
        {
            if (manga is null)
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possivel adicionar um registro Vazio.");

            try
            {
                int result = _manga.Add(manga);
                if (result > 0)
                {
                    return Ok(manga);
                }
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possivel adicionar o registro: {manga.Nome}");
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
