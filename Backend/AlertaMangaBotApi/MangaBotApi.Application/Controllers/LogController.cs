using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaBotApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IBaseRepository<LogMangaBotApi> _logMangaBotApi;
        private readonly IBaseRepository<LogMangaBot> _logMangaBot;

        public LogController(
            IBaseRepository<LogMangaBot> logMangaBot,
            IBaseRepository<LogMangaBotApi> logMangaBotApi)
        {
            _logMangaBot = logMangaBot;
            _logMangaBotApi = logMangaBotApi;
        }

        [HttpGet("logApi")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLogApi()
        {
            try
            {
                var logsApi = _logMangaBotApi.GetAll().OrderByDescending(api => api.dataExecucao).ToList();
                if (logsApi.Count > 0)
                    return Ok(logsApi);

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
        [HttpGet("logBot")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLogBot()
        {
            try
            {
                var logsBot = _logMangaBot.GetAll().OrderByDescending(bot => bot.dataExecucao).ToList();
                if (logsBot.Count > 0)
                    return Ok(logsBot);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Lista de Mangas vazia.");
            }
            catch (Exception ex)
            {
                _logMangaBotApi.Add(new LogMangaBotApi
                {
                    execucao = "Erro",
                    mensagem = $"MangaBot: {ex.Message}",
                    dataExecucao = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                });
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou: " + ex.Message + "");
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        public IActionResult DeleteLogAll()
        {
            try
            {
                var logsBot = _logMangaBot.GetAll().OrderByDescending(bot => bot.dataExecucao).ToList();
                if (logsBot.Count <= 0)
                    return this.StatusCode(StatusCodes.Status404NotFound, $"Lista MangaBot Vazia.");

                var logsApi = _logMangaBotApi.GetAll().OrderByDescending(api => api.dataExecucao).ToList();
                if (logsApi.Count <= 0)
                    return this.StatusCode(StatusCodes.Status404NotFound, $"Lista MangaBotApi Vazia.");

                bool ignorar = true;
                foreach (var log in logsBot)
                {
                    if (!ignorar)
                    {
                        _logMangaBot.Delete(log.Id);
                    }
                    ignorar = false;
                }
                ignorar = true;
                foreach (var log in logsApi)
                {
                    if (!ignorar)
                    {
                        _logMangaBotApi.Delete(log.Id);
                    }
                    ignorar = false;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logMangaBotApi.Add(new LogMangaBotApi
                {
                    execucao = "Erro",
                    mensagem = $"DeleteLogAll: {ex.Message}",
                    dataExecucao = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                });
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou: " + ex.Message + "");
            }
        }
    }
}
